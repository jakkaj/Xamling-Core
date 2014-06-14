using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Downloaders
{
    public interface IDownloadConfigService
    {
        string BaseUrl { get; }
        Task<IDownloadConfig> GetConfig(string url, string verb);
        Task<IDownloadResult> GetResult(HttpResponseMessage result, IDownloadConfig originalConfig);
        Task OnUnauthorizedResult(HttpResponseMessage result, IDownloadConfig originalConfig);
        Task OnUnsuccessfulResult(HttpResponseMessage result, IDownloadConfig originalConfig);
        IDownloadResult GetExceptionResult(Exception ex, string source, IDownloadConfig originalConfig);
    }
}
