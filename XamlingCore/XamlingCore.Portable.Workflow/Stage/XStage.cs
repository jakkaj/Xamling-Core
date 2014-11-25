using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.Workflow.Contract;

namespace XamlingCore.Portable.Workflow.Stage
{
    public class XStage<TEntityType> where TEntityType : IEntity
    {
        private Func<TEntityType, Task<XStageResult<TEntityType>>> _processor;

        ILifetimeScope Scope { get; set; }

        readonly List<IXBucket> _buckets = new List<IXBucket>();

        public bool RequiresInternet { get; protected set; }

        public bool IsProcessing { get; internal set; }

        public int MaximumRetries { get; protected set; }
        public int RetryCount { get; set; }

        public string StageName { get; protected set; }

        public async Task<XStageResult<TEntityType>> Process(TEntityType entity)
        {
            if (_processor == null) throw new NullReferenceException(_getErrorString("Processor function callback not set"));

            //update the buckets

            //do the process

            var result = await _processor(entity);

            return result;
        }


        public XStage<TEntityType> SetProcessor(Func<TEntityType, Task<XStageResult<TEntityType>>> processor)
        {
            if (processor == null) throw new ArgumentNullException("processor");
            _processor = processor;
            return this;
        }

        public XStage<TEntityType> AddBucket<TBucketType>() where TBucketType : class, IXBucket
        {
            var bucket = Scope.Resolve<TBucketType>();

            if (bucket == null) throw new NullReferenceException(_getErrorString("Could not resolve bucket " + typeof(TBucketType).FullName));

            if (_buckets.Contains(bucket))
            {
                _buckets.Add(bucket);
            }

            return this;
        }

        string _getErrorString(string text)
        {
            return string.Format("[{0}] - Processor - {1}", StageName ?? "NoNameStage", text);
        }
    }
}
