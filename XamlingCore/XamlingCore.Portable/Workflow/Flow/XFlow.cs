using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Contract.Serialise;
using XamlingCore.Portable.Util.Lock;
using XamlingCore.Portable.Workflow.Stage;

namespace XamlingCore.Portable.Workflow.Flow
{
    public class XFlow
    {
        private readonly IDeviceNetworkStatus _networkStatus;
        private readonly IEntitySerialiser _entitySerialiser;
        private readonly ILocalStorage _localStorage;
        private string _flowId;
        private string _friendlyName;

        readonly List<XStage> _stages = new List<XStage>();

        readonly List<XFlowState> _state = new List<XFlowState>();

        private XAsyncLock _stateLock = new XAsyncLock();
        private XAsyncLock _saveLock = new XAsyncLock();

        public event EventHandler FlowsUpdated;

        private CancellationTokenSource _cancelWaitToken;

        private List<XFlowState> _inProgressStates = new List<XFlowState>(); 

        private bool _persistOnComplete = true;

        public XFlow(IDeviceNetworkStatus networkStatus, IEntitySerialiser entitySerialiser, ILocalStorage localStorage)
        {
            _networkStatus = networkStatus;
            _entitySerialiser = entitySerialiser;
            _localStorage = localStorage;
        }

        public XFlow Setup(string flowId, string friendlyName)
        {
            _flowId = flowId;
            _friendlyName = friendlyName;

            return this;
        }

        public XFlow Merge(XFlow other)
        {
            _stages.AddRange(other._stages);
            return this;
        }

        public XFlow AddStage(string stageId, string processingText, string failText, Func<Guid, Task<XStageResult>> function,
            bool isDisconnectedProcess = false, bool requiresNetwork = false, int retries = 0, Func<Guid, Task<XStageResult>> failFunction = null)
        {

            var stageCount = _stages.Count(_ => _.StageId.Contains(stageId));

            if (stageCount > 0)
            {
                stageId += string.Format("_{0}", stageCount);
            }

            var stage = new XStage(stageId, processingText, failText, function, isDisconnectedProcess, requiresNetwork, retries, failFunction);
            _stages.Add(stage);

            return this;
        }

        public XFlow PersistOnComplete(bool persist)
        {
            _persistOnComplete = persist;
            return this;
        }

        public async Task<XFlow> Complete()
        {
            await _load();

            IsComplete = true;

            Process();

            return this;
        }

        public XStage GetStage(string stageId)
        {
            return _stages.FirstOrDefault(_ => _.StageId == stageId);
        }

        public XFlowState GetState(Guid id)
        {
            return _state.FirstOrDefault(_ => _.ItemId == id);
        }

        public async Task<List<XFlowState>> GetInProgressItems()
        {
            using (var l = await _stateLock.LockAsync())
            {
                return
                    _state.Where(item => item.State != XFlowStates.Success && item.State != XFlowStates.Fail).ToList();
            }
        }

        public async Task<bool> ResumeDisconnected(Guid id, bool result)
        {
            var state = GetState(id);

            if (state == null)
            {
                return false;
            }

            var stage = _getStage(state);

            if (!stage.IsDisconnectedProcess)
            {
                return false;
            }

            Debug.WriteLine("Resuming disconnected flow: {0}, with result {1}", state, result);

            if (result)
            {
                await _successResult(state);
            }
            else
            {
                await _failResult(state);
            }
          
            _cancelProcessWait();

            return true;
        }

        public async Task<List<XFlowState>> GetFailedItems()
        {
            using (var l = await _stateLock.LockAsync())
            {
                return _state.Where(item => item.State == XFlowStates.Fail).ToList();
            }
        }

        public async Task<List<XFlowState>> GetSuccessItems()
        {
            using (var l = await _stateLock.LockAsync())
            {
                return _state.Where(item => item.State == XFlowStates.Success).ToList();
            }
        }

        public async Task<List<XFlowState>> GetAllItems()
        {
            using (var l = await _stateLock.LockAsync())
            {
                return _state.ToList();
            }
        }

        public async Task<XFlowState> Start(Guid id)
        {
            if (!IsComplete)
            {
                throw new InvalidOperationException("Cannot start flows until Complete() is called");
            }

            var flowState = new XFlowState
            {
                Id = Guid.NewGuid(),
                ItemId = id,
                State = XFlowStates.WaitingForNextStage,
                StageId = null,
                Timestamp = DateTime.UtcNow,
                ParentFlow = this
            };

            using (var l = await _stateLock.LockAsync())
            {
                var existing = _state.FirstOrDefault(_ => _.ItemId == id);

                if (existing != null)
                {
                    //if it's not failed or completed then clear it and start gain. 
                    if (existing.State != XFlowStates.Fail && existing.State != XFlowStates.Success)
                    {
                        return null;
                    }

                    _state.Remove(existing);
                }

                _state.Add(flowState);
            }

            await _save();

            _cancelProcessWait();

            return flowState;

        }

        void _cancelProcessWait()
        {
            Task.Run(() =>
            {
                if (_cancelWaitToken != null)
                {
                    _cancelWaitToken.Cancel();
                }
            });

        }

        async Task<List<XFlowState>> _getWaitingStates()
        {
            using (var l = await _stateLock.LockAsync())
            {
                var states = _state.Where(_ => _.State == XFlowStates.WaitingForNextStage
                                               || _.State == XFlowStates.WaitingToStart
                                               || _.State == XFlowStates.WaitingForNetwork
                                               || _.State == XFlowStates.WaitingForRetry)
                    .ToList();

                return states;
            }
        }

        public async void Process()
        {
            while (true)
            {
                var states = await _getWaitingStates();

                var currentTasks = new List<Task>();

                if (states != null)
                {
                    foreach (var state in states)
                    {
                        if (state.State == XFlowStates.WaitingForNextStage)
                        {

                            var nextStageResult = _moveNextStage(state);

                            if (!nextStageResult)
                            {
                                _finish(state);
                                continue;
                            }

                            state.State = XFlowStates.WaitingToStart;
                        }

                        if (state.State == XFlowStates.WaitingForRetry)
                        {
                            if (DateTime.UtcNow.Subtract(state.Timestamp) < TimeSpan.FromSeconds(15))
                            {
                                continue;
                            }
                        }

                        if (state.State == XFlowStates.WaitingToStart ||
                            state.State == XFlowStates.WaitingForNetwork ||
                            state.State == XFlowStates.WaitingForRetry)
                        {
                            var t = _runStage(state);

                            if (t != null)
                            {
                                currentTasks.Add(t);
                            }
                        }
                    }

                    //await Task.WhenAll(currentTasks);
                }

               await Task.Yield();

                _cancelWaitToken = new CancellationTokenSource();

                try
                {
                    await Task.Delay(8000, _cancelWaitToken.Token);
                }
                catch
                {
                    Debug.WriteLine("XFlow cancelled wait");
                }

                await Task.Delay(2000);

                _cancelWaitToken = null;
            }
        }

        Task _runStage(XFlowState state)
        {
            if (state.State != XFlowStates.WaitingForNetwork && state.State != XFlowStates.WaitingForRetry &&
                state.State != XFlowStates.WaitingToStart)
            {
                return null;
            }

            if (_inProgressStates.Contains(state))
            {
                return null;
            }
                

            var t = Task.Run(async () =>
            {
                var stage = _getStage(state);

                if (stage.RequiresNetwork && !_networkStatus.QuickNetworkCheck())
                {
                    state.State = XFlowStates.WaitingForNetwork;
                    await Task.Yield();
                    return;
                }

                state.Text = stage.ProcessingText;

                state.State = XFlowStates.InProgress;
                state.Timestamp = DateTime.UtcNow;


                if (stage.IsDisconnectedProcess)
                {
                    state.State = XFlowStates.DisconnectedProcessing;
                    await _save();
                    Debug.WriteLine("Stopping flow for disconnected state, will resume when asked. {0}", state);
                    return;
                }

                await _save();

                XStageResult failResult = null;

                try
                {
                    if (_inProgressStates.Contains(state))
                    {
                        return;
                    }

                    _inProgressStates.Add(state);
                    
                    var result = await stage.Function(state.ItemId);
                    
                    if (result.Id != Guid.Empty)
                    {
                        state.ItemId = result.Id;
                    }

                    state.PreviousStageResult = result;

                    _inProgressStates.Remove(state);

                    if (!result.IsSuccess)
                    {
                        await _failResult(state);
                    }
                    else
                    {
                        await _successResult(state);
                    }

                }
                catch (Exception ex)
                {
                    _inProgressStates.Remove(state);
                    failResult = new XStageResult(false, state.ItemId, null, exception: ex.ToString());
                }

                if (failResult != null)
                {
                    state.PreviousStageResult = failResult;

                    var stateString = _entitySerialiser.Serialise(state);

                    Debug.WriteLine("Caught exception process: {0}", stateString);

                    await _failResult(state);
                }
               
                _cancelProcessWait();
            });

            return t;
        }

        async Task _failResult(XFlowState state)
        {
            state.PreviousStageSuccess = false;

            var stage = _getStage(state);

            state.Text = stage.FailText;

            if (stage.Retries > 0)
            {
                if (state.FailureCount <= stage.Retries)
                {
                    //let's retry
                    state.FailureCount++;
                    state.State = XFlowStates.WaitingForRetry;
                    state.Timestamp = DateTime.UtcNow;
                    await _save();
                    _cancelProcessWait();
                    return;
                }
            }

            await stage.Fail(state.ItemId);
            Debug.WriteLine("Stage Failed [{0}] {1} - {2}",
                state.ItemId,
                state.PreviousStageResult != null ? state.PreviousStageResult.ExtraText : "No Extra Text",
                state.PreviousStageResult != null ? state.PreviousStageResult.Exception : "No Exception");
            Debug.WriteLine("Failure stage: {0}", stage.ProcessingText);
            _finish(state);
        }

        async Task _successResult(XFlowState state)
        {
            //state.Text = "";
            state.PreviousStageSuccess = true;

            if (state.PreviousStageResult != null && state.PreviousStageResult.CompleteNow)
            {
                //the result has asked for the workflow to complete early
                //this could be because the rest isn't needed, e.g. an entity was detected
                //locally so there is no need to get it form the server. 
                _finish(state);
                return;
            }

            state.State = XFlowStates.WaitingForNextStage;
            state.Timestamp = DateTime.UtcNow;

            await _save();

            _cancelProcessWait();
        }

        async void _finish(XFlowState state)
        {
            if (!_persistOnComplete)
            {
                if (_state.Contains(state))
                {
                    _state.Remove(state);
                }
                await _save();
                return;
            }

            state.IsComplete = true;

            Debug.WriteLine("Completed State Flow with {0}: {1}", state.PreviousStageSuccess, state);

            if (state.PreviousStageSuccess)
            {
                state.State = XFlowStates.Success;
                state.IsSuccessful = true;
                state.Timestamp = DateTime.UtcNow;
                await _save();
                return;
            }

            state.State = XFlowStates.Fail;
            state.IsSuccessful = false;
            state.Timestamp = DateTime.UtcNow;

            await _save();
        }

        bool _moveNextStage(XFlowState state)
        {
            if (state.StageId == null)
            {
                var firstStage = _stages.First();
                state.StageId = firstStage.StageId;
                return true;
            }

            var stage = _getStage(state);

            var index = _stages.IndexOf(stage);

            var newIndex = index + 1;

            if (newIndex >= _stages.Count)
            {
                //this is the last stage, so return false;
                return false;
            }

            var newStage = _stages[newIndex];

            state.StageId = newStage.StageId;
            state.FailureCount = 0;
            return true;
        }



        XStage _getStage(XFlowState state)
        {
            var stage = _stages.First(_ => _.StageId == state.StageId);

            return stage;
        }

        async Task _load()
        {
            using (var l = await _saveLock.LockAsync())
            {
                var file = _getFileName();
                if (!await _localStorage.FileExists(file))
                {
                    return;
                }

                var ser = await _localStorage.LoadString(file);

                if (string.IsNullOrWhiteSpace(ser))
                {
                    return;
                }

                var loadedState = _entitySerialiser.Deserialise<List<XFlowState>>(ser);

                if (loadedState == null || loadedState.Count == 0)
                {
                    return;
                }

                foreach (var item in loadedState)
                {
                    item.ParentFlow = this;
                    if (string.IsNullOrWhiteSpace(item.StageId))
                    {
                        Debug.WriteLine("Dud stage id from saved item, ignoring");
                        continue;
                    }

                    if (_stages.FirstOrDefault(_ => _.StageId == item.StageId) == null)
                    {
                        Debug.WriteLine("Warning ** Missing stage when loading workflow state: " + item.StageId);
                        continue;
                    }

                    if (item.State == XFlowStates.InProgress)
                    {
                        //this item was processing when hte app quit... set it to wiating to run to try again
                        item.State = XFlowStates.WaitingToStart;
                    }

                    _state.Add(item);
                }
            }

            await _save();
        }

        async Task _save()
        {
            using (var l = await _saveLock.LockAsync())
            {
                using (var l2 = await _stateLock.LockAsync())
                {
                    var ser = _entitySerialiser.Serialise(_state);
                    await _localStorage.SaveString(_getFileName(), ser);
                }
            }

            _fireUpdated();
        }

        string _getFileName()
        {
            return string.Format("{0}.flow", FlowId);
        }

        void _fireUpdated()
        {
            if (FlowsUpdated != null)
            {
                FlowsUpdated(this, EventArgs.Empty);
            }
        }

        public string FriendlyName
        {
            get { return _friendlyName; }
        }

        public string FlowId
        {
            get { return _flowId; }
        }

        public bool IsComplete { get; private set; }
    }
}
