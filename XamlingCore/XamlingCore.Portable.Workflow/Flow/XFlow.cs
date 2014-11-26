using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        private AsyncLock _stateLock = new AsyncLock();
        private AsyncLock _saveLock = new AsyncLock();

        public event EventHandler FlowsUpdated;
        
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

        public XFlow AddStage(string stageId, string processingText, string failText, Func<Guid, Task<XStageResult>> function,
            bool isLongProcess = false, bool requiresNetwork = false, int retries = 0)
        {
           
            var stage = new XStage(stageId, processingText, failText, function, isLongProcess, requiresNetwork, retries);
            _stages.Add(stage);

            return this;
        }

        public async Task<XFlow> Complete()
        {
            await _load();

            IsComplete = true;

            Process();

            return this;
        }

        public async Task<List<XFlowState>> GetInProgressItems()
        {
            using (var l = await _stateLock.LockAsync())
            {
                return
                    _state.Where(item => item.State != XFlowStates.Success && item.State != XFlowStates.Fail).ToList();
            }

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

        public async Task<bool> Start(Guid id)
        {
            var flowState = new XFlowState
            {
                Id = Guid.NewGuid(),
                ItemId = id,
                State = XFlowStates.WaitingForNextStage,
                StageId = null
            };

            using (var l = await _stateLock.LockAsync())
            {
                var existing = _state.FirstOrDefault(_ => _.ItemId == id);

                if (existing != null)
                {
                    //if it's not failed or completed then clear it and start gain. 
                    if (existing.State != XFlowStates.Fail && existing.State != XFlowStates.Success)
                    {
                        return false;
                    }

                    _state.Remove(existing);
                }

                _state.Add(flowState);
            }

            Process();

            return true;
          
        }

        public async void Process()
        {
            using (var l = await _stateLock.LockAsync())
            {
                foreach (var state in _state)
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

                    if (state.State == XFlowStates.WaitingToStart || 
                        state.State != XFlowStates.WaitingForNetwork || 
                        state.State == XFlowStates.WaitingForRetry)
                    {
                        _runStage(state);
                    }
                }
            }
        }

        void _runStage(XFlowState state)
        {
            if (state.State != XFlowStates.WaitingForNetwork && state.State != XFlowStates.WaitingForNextStage &&
                state.State != XFlowStates.WaitingToStart)
            {
                return;
            }

            Task.Run(async () =>
            {
                var stage = _getStage(state);

                if (stage.RequiresNetwork && !_networkStatus.QuickNetworkCheck())
                {
                    state.State = XFlowStates.WaitingForNetwork;
                    return;
                }

                state.Text = stage.ProcessingText;

                state.State = XFlowStates.InProgress;



                var result = await stage.Function(state.ItemId);

                if (result.Id != Guid.Empty)
                {
                    state.ItemId = result.Id;
                }

                if (!result.IsSuccess)
                {
                    _failResult(state);
                }
                else
                {
                    _successResult(state);
                }
            });
        }

        async void _failResult(XFlowState state)
        {
            state.PreviousStageSuccess = false;
            
            var stage = _getStage(state);

            state.Text = stage.FailText;

            if (stage.Retries > 0)
            {
                if (state.FailureCount <= stage.Retries)
                {
                    //let's retry
                    state.FailureCount ++;
                    state.State = XFlowStates.WaitingForRetry;
                    await _save();
                    return;
                }
            }

            _finish(state);
        }

        async void _successResult(XFlowState state)
        {
            state.Text = "";
            state.PreviousStageSuccess = true;
            state.State = XFlowStates.WaitingForNextStage;

            await _save();

            Process();
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

        async void _finish(XFlowState state)
        {
            if (state.PreviousStageSuccess)
            {
               state.State = XFlowStates.Success;
               await _save();
               return;
            }

            state.State = XFlowStates.Fail;
            await _save();
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
                    if (_stages.FirstOrDefault(_ => _.StageId == item.StageId) == null)
                    {
                        Debug.WriteLine("Warning ** Missing stage when loading workflow state: " + item.StageId);
                        continue;
                    }

                    if (item.State == XFlowStates.Processing)
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
                var ser = _entitySerialiser.Serialise(_state);
                await _localStorage.SaveString(_getFileName(), ser);
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
