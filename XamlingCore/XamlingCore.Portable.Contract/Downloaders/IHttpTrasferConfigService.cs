using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Downloaders
{
    public interface IHttpTransferConfigService
    {
        string BaseUrl { get; }
        Task<IHttpTransferConfig> GetConfig(string url, string verb);
        Task<IHttpTransferResult> GetResult(HttpResponseMessage result, IHttpTransferConfig originalConfig);
        Task OnUnauthorizedResult(HttpResponseMessage result, IHttpTransferConfig originalConfig);
        Task OnUnsuccessfulResult(HttpResponseMessage result, IHttpTransferConfig originalConfig);
        IHttpTransferResult GetExceptionResult(Exception ex, string source, IHttpTransferConfig originalConfig);
    }
}
