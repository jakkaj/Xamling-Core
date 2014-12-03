using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace XamlingCore.Portable.Messages.Network
{
    public class TransferProgressMessage : XMessage
    {
        private readonly string _url;
        private readonly long _bytes;
        private readonly long _totalBytes;
        private readonly long _expected;
        private readonly bool _isUpload;

        public TransferProgressMessage(string url, long bytes, long totalBytes, long expected, bool isUpload)
        {
            _url = url;
            _bytes = bytes;
            _totalBytes = totalBytes;
            _expected = expected;
            _isUpload = isUpload;
        }

        public string Url
        {
            get { return _url; }
        }

        public long Bytes
        {
            get { return _bytes; }
        }

        public long TotalBytes
        {
            get { return _totalBytes; }
        }

        public long Expected
        {
            get { return _expected; }
        }

        public bool IsUpload
        {
            get { return _isUpload; }
        }
    }
}
