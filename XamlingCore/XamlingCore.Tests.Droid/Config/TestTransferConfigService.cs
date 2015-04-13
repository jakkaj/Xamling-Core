using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Serialise;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Net.DownloadConfig;
using XamlingCore.Portable.Net.Service;

namespace XamlingCore.Tests.Droid.Config
{
    public class TestTransferConfigService : HttpTransferConfigServiceBase
    {
        private readonly IEntitySerialiser _entitySerialiser;
        //private readonly ITokenGetService _tokenService;
        private string _authBase = "https://ptauth.azurewebsites.net/api/";
        //private string _authBase = "http://192.168.0.9:8082/PTAuth/api/";
        public TestTransferConfigService(IEntitySerialiser entitySerialiser)
        {
            _entitySerialiser = entitySerialiser;
            //_tokenService = tokenService;

            BaseUrl = "https://api.projecttripod.com/API/";
        }

        public async override Task<IHttpTransferConfig> GetConfig(string url, string verb)
        {
            var burl = BaseUrl;

            if (url.EndsWith("token") || url.Contains("auth/") || url.Contains("/auth") || url.Contains("auth?"))
            {
                burl = _authBase;
            }

            var adjustedUrl = url.StartsWith("http") ? url : string.Format("{0}{1}", burl, url);



            var config = new StandardHttpConfig
            {
                Accept = "application/json",
                IsValid = true,
                Url = adjustedUrl,
                BaseUrl = burl,
                Verb = verb,
                Headers = new Dictionary<string, string>()
            };

            if (url.EndsWith("token"))
            {
                config.ContentEncoding = "text/plain";
                config.Accept = "text/plain";
            }

            if (!_noToken(adjustedUrl))
            {
                var token = await _getToken();

                if (token == null)
                {
                    //this should not be null, means some kind of expiry. 
                    //new TokenProblemMessage().Send();
                    return null;
                }

                config.Auth = token;
                config.AuthScheme = "Token";
            }

            return config;
        }

        async Task<string> _getToken()
        {
            return "AbcEasyAs123";
            //var token = new JwtToken(); ;//await _tokenService.GetMainToken(true);

            //if (token == null)
            //{
            //    //there was a problem getting a token
            //    return null;
            //}

            //var tokenRaw = token.Raw;

            //return tokenRaw;
        }

        bool _noToken(string url)
        {
            if (url.IndexOf("api/auth") != -1)
            {
                return true;
            }

            if (url.IndexOf("api//auth") != -1)
            {
                return true;
            }

            if (url.IndexOf("auth?") != -1)
            {
                return true;
            }

            if (url.IndexOf("ptauth.") != -1)
            {
                return true;
            }

            if (url.IndexOf("v2/tripods/") != -1)
            {
                return true;
            }

            if (url.IndexOf("token") != -1)
            {
                return true;
            }

            if (url.IndexOf("v2/opentripodlist/list") != -1)
            {
                return true;
            }

            return false;
        }
    }
}
