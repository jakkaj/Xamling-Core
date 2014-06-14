using System;
using System.Net;
using XamlingCore.Portable.Contract.Downloaders;

namespace XamlingCore.Portable.Net.Downloaders
{
    public class DownloadResult : IDownloadResult
    {
        public string Result { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public Exception DownloadException { get; set; }

        public bool IsSuccessCode { get; set; }
    }
}
