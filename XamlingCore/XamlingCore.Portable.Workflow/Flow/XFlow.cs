using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.Workflow.Contract;
using XamlingCore.Portable.Workflow.Stage;

namespace XamlingCore.Portable.Workflow.Flow
{
    public class XStageWrapper<TEntityType> where TEntityType : IEntity
    {
        public XStage<TEntityType> Current { get; set; }
        public XStageWrapper<TEntityType> Next { get; set; }
        public XStage<TEntityType> Fail { get; set; }
    }

    public class XFlow<TEntityType> where TEntityType : IEntity
    {
        List<XStageWrapper<TEntityType>> _stages = new List<XStageWrapper<TEntityType>>();
        
        Dictionary<XStageWrapper<TEntityType>, TEntityType> _liveStages = new Dictionary<XStageWrapper<TEntityType>, TEntityType>();

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

        private void _processStages()
        {
            foreach (var stagingPair in _liveStages.Where(_=>!_.Key.Current.IsProcessing))
            {
                var c = stagingPair;
                //get the next and process it
                Task.Run(() => _processNext(c.Key, c.Value));
            }
        }

        async void _processNext(XStageWrapper<TEntityType> wrapper, TEntityType entity)
        {
            var result = await wrapper.Current.Process(entity);

            if (result.IsSuccess)
            {

            }
            else
            {
                
            }
        }
    }
}
