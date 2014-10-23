using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace XamlingCore.Portable.Messages.Entities
{
    public class BucketUpdatedMessage : XMessage
    {
        
        private readonly string _bucketName;
        private readonly Type _bucketType;
        private readonly BucketUpdatedTypes _operationType;

        public BucketUpdatedMessage(BucketUpdatedTypes operationType)
        {
            _operationType = operationType;
        }

        public BucketUpdatedMessage(string bucketName, Type bucketType, BucketUpdatedTypes operationType)
        {
            _bucketName = bucketName;
            _bucketType = bucketType;
            _operationType = operationType;
        }

        public Type BucketType
        {
            get { return _bucketType; }
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
