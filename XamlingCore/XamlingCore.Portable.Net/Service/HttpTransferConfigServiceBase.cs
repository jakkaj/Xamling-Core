using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Net.Downloaders;

namespace XamlingCore.Portable.Net.Service
{
    public abstract class HttpTransferConfigServiceBase : IHttpTransferConfigService
    {
        public string BaseUrl { get; protected set; }

        public abstract Task<IHttpTransferConfig> GetConfig(string url, string verb);

        public async virtual Task OnUnauthorizedResult(HttpResponseMessage result, IHttpTransferConfig originalConfig)
        {

        }

        public async virtual Task OnUnsuccessfulResult(HttpResponseMessage result, IHttpTransferConfig originalConfig)
        {

        }

        protected virtual void OnDownloadException(Exception ex, string source, IHttpTransferConfig originalConfig)
        {

        }

        public virtual IHttpTransferResult GetExceptionResult(Exception ex, string source, IHttpTransferConfig originalConfig)
        {
            OnDownloadException(ex, source, originalConfig);
            return new HttpTransferResult {DownloadException = ex, Result = null, IsSuccessCode = false};
        }

        public async virtual Task<IHttpTransferResult> GetResult(HttpResponseMessage result, IHttpTransferConfig originalConfig)
        {
            try
            {
                var resultText = "";

                var isSuccess = true;

                if (result.Content != null)
                {
                    resultText = await result.Content.ReadAsStringAsync();
                }

                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await OnUnauthorizedResult(result, originalConfig);
                    isSuccess = false;
                }

                if (!result.IsSuccessStatusCode)
                {
                    await OnUnsuccessfulResult(result, originalConfig);
                    isSuccess = false;
                }

                return new HttpTransferResult
                {
                    HttpStatusCode = result.StatusCode,
                    Result = resultText,
                    IsSuccessCode = isSuccess
                };
            }
            catch (Exception ex)
            {
                return GetExceptionResult(ex, "DownloadConfigService", originalConfig);
            }
        }
        
    }
}
