using System.Collections.Generic;

namespace XamlingCore.Portable.Contract.Downloaders
{
    /// <summary>
    /// Represents a configuration used for making a download request
    /// </summary>
    public interface IDownloadConfig
    {
        string Url { get; }
        string Verb { get; }
        string BaseUrl { get; }

        string Auth { get; }
        string AuthScheme { get; }

        Dictionary<string, string> Headers { get; }

        /// <summary>
        /// Accept: (eg. 'Accept: text/plain')
        /// </summary>
        string Accept { get; set; }

        /// <summary>
        /// Accept-Encoding: (eg 'Accept-Encoding: gzip, deflate')
        /// </summary>
        List<string> AcceptEncoding { get; }

        string ContentEncoding { get; }
        
        bool IsValid { get; }
        bool Gzip { get; set; }
    }
}