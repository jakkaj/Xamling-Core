using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.EventArgs
{
    public class BucketUpdatedEventArgs : System.EventArgs
    {
        private readonly string _bucket;

        public BucketUpdatedEventArgs(string bucket)
        {
            _bucket = bucket;
        }

        public string Bucket
        {
            get { return _bucket; }
        }
    }
}
