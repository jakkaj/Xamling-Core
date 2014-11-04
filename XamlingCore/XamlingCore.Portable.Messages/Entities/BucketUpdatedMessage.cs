using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.Messages.Entities
{
    public class BucketUpdatedMessage<T> : XMessage where T: class, IEntity, new()
    {
        
        private readonly string _bucketName;
        private readonly BucketUpdatedTypes _operationType;

        public BucketUpdatedMessage(BucketUpdatedTypes operationType)
        {
            _operationType = operationType;
        }

        public BucketUpdatedMessage(string bucketName, BucketUpdatedTypes operationType)
        {
            _bucketName = bucketName;
            _operationType = operationType;
        }
      
        public string BucketName
        {
            get { return _bucketName; }
        }

        public BucketUpdatedTypes OperationType
        {
            get { return _operationType; }
        }

      
    }

    public enum BucketUpdatedTypes
    {
        Add,
        Remove,
        Clear
    }
}
