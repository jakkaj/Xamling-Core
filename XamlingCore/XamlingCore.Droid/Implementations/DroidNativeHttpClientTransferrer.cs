using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Messages.Network;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Net.Downloaders;

namespace XamlingCore.Droid.Implementations
{
    public class DroidNativeHttpClientTransferrer : IHttpTransferrer
    {
        private readonly IHttpTransferConfigService _httpTransferService;

        public DroidNativeHttpClientTransferrer(IHttpTransferConfigService httpTransferService)
        {
            _httpTransferService = httpTransferService;
        }

        public async Task<IHttpTransferResult> Upload(string url, string verb = "GET", byte[] data = null)
        {
            var downloadConfig = await _httpTransferService.GetConfig(url, verb);

            if (downloadConfig == null)
            {
                Debug.WriteLine("No download config for URL: {0}", url);
                return null;
            }

            if (!downloadConfig.IsValid)
            {
                return new HttpTransferResult
                {
                    IsSuccessCode = false
                };
            }

            var obj = new DownloadQueueObject
            {
                ByteData = data,
                Verb = downloadConfig.Verb,
                Url = downloadConfig.Url
            };

            return await _doDownload(obj, downloadConfig);
        }

        public async Task<IHttpTransferResult> Download(string url, string verb = "GET", string data = null)
        {
            var downloadConfig = await _httpTransferService.GetConfig(url, verb);

            if (downloadConfig == null)
            {
                Debug.WriteLine("No download config for URL: {0}", url);
                return null;
            }

            if (!downloadConfig.IsValid)
            {
                return new HttpTransferResult
                {
                    IsSuccessCode = false
                };
            }

            var obj = new DownloadQueueObject
            {
                Data = data,
                Verb = downloadConfig.Verb,
                Url = downloadConfig.Url
            };

            return await _retryDownload(obj, downloadConfig);
        }

        async Task<IHttpTransferResult> _retryDownload(DownloadQueueObject obj, IHttpTransferConfig downloadConfig)
        {
            var succeed = false;

            IHttpTransferResult result = null;

            var retryCount = 0;

            do
            {
                result = await _doDownload(obj, downloadConfig);

                if (retryCount < downloadConfig.Retries &&
                    (result == null || result.DownloadException != null ||
                     (!result.IsSuccessCode && downloadConfig.RetryOnNonSuccessCode)))
                {
                    succeed = false;
                }
                else
                {
                    succeed = true;
                }

                retryCount++;

            } while (succeed == false);

            return result;
        }

        async Task<IHttpTransferResult> _doDownload(DownloadQueueObject obj, IHttpTransferConfig downloadConfig)
        {
            // add support for Gzip decompression
            var native = new NativeMessageHandler();

            var httpClient = new HttpClient(native);

            using (httpClient)
            {
                var method = HttpMethod.Get;

                switch (obj.Verb)
                {
                    case "GET":
                        method = HttpMethod.Get;
                        break;
                    case "POST":
                        method = HttpMethod.Post;
                        break;
                    case "PUT":
                        method = HttpMethod.Put;
                        break;
                    case "DELETE":
                        method = HttpMethod.Delete;
                        break;
                }

                using (var message = new HttpRequestMessage(method, obj.Url))
                {

                    native.RegisterForProgress(message, (bytes, totalBytes, expected) => new TransferProgressMessage(obj.Url, bytes, totalBytes, expected, downloadConfig.Verb.ToLower() != "get").Send());

                    if (downloadConfig.Headers != null)
                    {
                        foreach (var item in downloadConfig.Headers)
                        {
                            message.Headers.Add(item.Key, item.Value);
                        }
                    }

                    // Accept-Encoding:
                    if (downloadConfig.AcceptEncoding != null)
                    {
                        //message.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(""));
                        message.Headers.Add("Accept-Encoding", downloadConfig.AcceptEncoding);
                    }


                    // Accept:
                    if (!string.IsNullOrWhiteSpace(downloadConfig.Accept))
                    {
                        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(downloadConfig.Accept));
                    }


                    if (!string.IsNullOrWhiteSpace(obj.Data))
                    {
                        var content = new StringContent(obj.Data, Encoding.UTF8,
                            downloadConfig.ContentEncoding ?? "application/json");
                        message.Content = content;
                    }

                    if (obj.ByteData != null)
                    {
                        var content = new ByteArrayContent(obj.ByteData, 0, obj.ByteData.Length);

                        message.Content = content;
                    }

                    if (downloadConfig.Auth != null && downloadConfig.AuthScheme != null)
                    {
                        message.Headers.Authorization = new AuthenticationHeaderValue(downloadConfig.AuthScheme,
                            downloadConfig.Auth);
                    }

                    try
                    {
                        Debug.WriteLine("{0}: {1}", downloadConfig.Verb.ToLower() == "get" ? "Downloading" : "Uploading", obj.Url);

                        using (var result = await httpClient.SendAsync(message))
                        {
                            Debug.WriteLine("Finished: {0}", obj.Url);
                            return await _httpTransferService.GetResult(result, downloadConfig);
                        }



                    }
                    catch (HttpRequestException ex)
                    {
                        Debug.WriteLine("Warning - HttpRequestException encountered: {0}", ex.Message);

                        return _httpTransferService.GetExceptionResult(ex,
                            "XamlingCore.Portable.Net.Downloaders.HttpClientDownloader", downloadConfig);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Warning - general HTTP exception encountered: {0}", ex.Message);
                        return _httpTransferService.GetExceptionResult(ex,
                            "XamlingCore.Portable.Net.Downloaders.HttpClientDownloader", downloadConfig);
                    }
                }
            }


        }

        protected class DownloadQueueObject
        {
            public string Url { get; set; }
            public string Data { get; set; }
            public byte[] ByteData { get; set; }
            public string Verb { get; set; }
            public int Retries { get; set; }
            public bool Cancelled { get; set; }
        }
    }
}
