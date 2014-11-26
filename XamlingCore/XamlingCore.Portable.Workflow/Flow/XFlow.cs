using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.Util.Lock;
using XamlingCore.Portable.Workflow.Contract;
using XamlingCore.Portable.Workflow.Stage;

namespace XamlingCore.Portable.Workflow.Flow
{
    public class XStageWrapper<TEntityType> where TEntityType : IEntity
    {
        public XStage<TEntityType> Current { get; set; }
        public XStageWrapper<TEntityType> Next { get; set; }
        public XStage<TEntityType> Fail { get; set; }
        public Guid //this is wher im up to - 
           //entire system should be using entity manager and id's only to 
           //make rehydreation much easier. 
    }

    public class XFlow<TEntityType> where TEntityType : IEntity
    {
        List<XStageWrapper<TEntityType>> _stages = new List<XStageWrapper<TEntityType>>();

        readonly Dictionary<XStageWrapper<TEntityType>, TEntityType> _liveStages = new Dictionary<XStageWrapper<TEntityType>, TEntityType>();
        Dictionary<XStage<TEntityType>, TEntityType> _failedStages = new Dictionary<XStage<TEntityType>, TEntityType>();

        AsyncLock _liveStageLock = new AsyncLock();

        public XStage<TEntityType> DefaultFailStage { get; set; } 

        public XFlow<TEntityType> Add(XStage<TEntityType> stage)
        {
            var lastStage = _stages.LastOrDefault();

            var wrapper = new XStageWrapper<TEntityType> {Current = stage};

            if (lastStage != null)
            {
                lastStage.Next = wrapper;
            }

            _stages.Add(wrapper);

            return this;
        }

        public XFlow<TEntityType> Fail(XStage<TEntityType> stage)
        {
            var lastStage = _stages.Last();

            if (lastStage == null)
            {
                throw new ArgumentException("Attempt to add fail when no previous stages");
            }

            lastStage.Fail = stage;

            return this;
        }

        public async Task Start(TEntityType entity)
        {
            var firstStage = _stages.First();

            if (firstStage == null)
            {
                throw new ArgumentException("Attempt to start a flow when no stages configured");
            }

            _liveStages.Add(firstStage, entity);

            _processStages();
        }

        private async void _processStages()
        {
            using (var l = await _liveStageLock.LockAsync())
            {
                foreach (var stagingPair in _liveStages.Where(_ => !_.Key.Current.IsProcessing))
                {
                    var c = stagingPair;
                    //get the next and process it
                    await Task.Run(() => _processNext(c.Key, c.Value));
                }
            }
        }

        async void _processNext(XStageWrapper<TEntityType> wrapper, TEntityType entity)
        {
            await Task.Yield();
            //keep in mind here that the process can return a different entity
            //for example, it might go to the server and get an entirely new entity
            //to replace the current entity with
            var result = await wrapper.Current.Process(entity);

            Task.Yield();

            if (result.IsSuccess)
            {
                _nextStage(wrapper, result.Entity);
            }
            else
            {
                //if it has a fail the put in to that bucket, else just put in to the generic fail bucket for this flow
                _failStage(wrapper, result.Entity);
            }
        }

        async void _failStage(XStageWrapper<TEntityType> wrapper, TEntityType entity)
        {
            using (var l = await _liveStageLock.LockAsync())
            {
                _liveStages.Remove(wrapper);
                if (wrapper.Fail != null)
                {
                    _failedStages.Add(wrapper.Fail, entity);
                }
                else
                {
                    if (DefaultFailStage == null)
                    {
                        Debugger.Break();
                        return;
                    }
                    _failedStages.Add(DefaultFailStage, entity);
                }
            }
        }

        async void _nextStage(XStageWrapper<TEntityType> wrapper, TEntityType entity)
        {
            using (var l = await _liveStageLock.LockAsync())
            {
                _liveStages.Remove(wrapper);
                _liveStages.Add(wrapper.Next, entity);
            }
        }
    }
}
