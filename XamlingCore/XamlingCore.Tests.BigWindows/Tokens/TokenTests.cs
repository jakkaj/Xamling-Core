using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using XamlingCore.Windows8.Auth;

namespace XamlingCore.Tests.BigWindows.Tokens
{
    [TestClass]
    public class TokenTests 
    {
        [TestMethod]
        public async Task TokenParseTest()
        {
            var result = JsonWebTokenParser.Decode(
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJuYW1laWQiOiJGMzgzNDQ0Ny0zRUFBLTRCQ0MtQkZDNS0zMzgxQjREMjQ3MjAiLCJwcm9qZWN0dHJpcG9kL3Rva2VuaWQiOiJjYTc1YzM0Ny0wZTMxLTQxNmUtYTdhYy04ZmIwN2E4YmYzOTQiLCJwcm9qZWN0dHJpcG9kL2F1dGhUb2tlbiI6InRydWUiLCJlbWFpbCI6ImpvcmRhbkBwcm9qZWN0dHJpcG9kLmNvbSIsImlzcyI6IlByb2plY3RUcmlwb2RBdXRoZW50aWNhdG9yIiwiYXVkIjoiaHR0cDovL2FwaS5wcm9qZWN0dHJpcG9kLmNvbSIsImV4cCI6MTQyMzE4MDQxNCwibmJmIjoxNDIzMDkzMTE0fQ.D2uVun-WIhLiNU-KPx029jfdi7dkktZqqZnthTJLdTrCzn2wSI6zFzAHH_qkYocsmYd7MxNiYA5QGvnH_WC5jrD_zPIDxnI69AhroJ8EQrCtoeJN-bfl7OZdk5GqO63lJX3nDljS535lp9XC_EOY0XSZaiRmdYV3kzp8GIg723E-dZnsFoKmfaYOvozTiRmQGbXg7Xz4SrasYpVOTxdSC8nE1FM0Q9tbZvWPs8mIFm13YumfTSFD5-6pfh7fNPp2ZPYz8bYWy9Id6uKGGvG_iL7tvLIK77x_xC3Azo9RcaKtcwrQsDWXGPHSzXIeL3i0KQwr9dunVYFD8FSMc7OeGQ",
                "PFJTQUtleVZhbHVlPjxNb2R1bHVzPjMwazZvWUk0bEJiMHlCeWN3K29PNW5DYVIvOFoxdU9QNmhiMjVhNGJmVnl3SCtZbUhsZUdtSy82cG1nbDc0SFpNb1pLU1hZelo0Rnl1TmUwTjJjak1jbEhtWXE5TGc1UjVjSHNPQmF5MEp6b29zajhmNFUrdWdEamdzZGsveEY2SFlaVll2MmxZcDV0NVFGVTFOUmIwbk9aZVoySWJzT1dSM1RtaVU4aE4rcThGd3VmdG5sTUN4Ym5wdzZpKzNJa2EwcnhtTjhPK2hjUUFyb0ZqMDFMY3Fhdk9CQ3ZKOUpFTkRCK0cvVXgwR1FwdWI1WFNPTit1OEIvNUV5anNRak8zYVZKQk1FYTRJdUlCcFM0VTRzbmpWZjVMTUMvNzN5bVhHSjFTT1B5VE4rQXZOaTBWK3c2QzNJc0M1Vk1yT0ZJZlQwT0FHNDRHazh1QTEwNnoyem5Ddz09PC9Nb2R1bHVzPjxFeHBvbmVudD5BUUFCPC9FeHBvbmVudD48L1JTQUtleVZhbHVlPg==",
                true);

            Assert.IsNotNull(result);
        }
    }
}
