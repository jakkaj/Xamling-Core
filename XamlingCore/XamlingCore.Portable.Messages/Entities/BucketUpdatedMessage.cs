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
        private readonly bool _isCleared;
        private readonly string _bucketName;
        private readonly Type _bucketType;

        public BucketUpdatedMessage(bool isCleared)
        {
            _isCleared = isCleared;
        }

        public BucketUpdatedMessage(string bucketName, Type bucketType)
        {
            _bucketName = bucketName;
            _bucketType = bucketType;
        }

        public Type BucketType
        {
            get { return _bucketType; }
        }

        public string BucketName
        {
            get { return _bucketName; }
        }

        public bool IsCleared
        {
            get { return _isCleared; }
        }
    }
}
