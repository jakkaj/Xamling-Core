using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Downloaders;

namespace XamlingCore.Portable.Net.DownloadConfig
{
    public class StandardDownloadConfig : IDownloadConfig
    {
        public string Url { get; set; }

        public string Verb { get; set; }

        public string BaseUrl { get; set; }

        public string Auth { get; set; }
        public string AuthScheme { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string Accept { get; set; }
        public List<string> AcceptEncoding { get; set; }

        public string ContentEncoding { get; set; }

        public bool IsValid { get; set; }

        public bool Gzip { get; set; }
    }
}
