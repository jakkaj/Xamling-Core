using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using XamlingCore.Portable.Contract.Helpers;

namespace XamlingCore.Windows8.Implementations.Helpers
{
    public class HashHelper : IHashHelper
    {
        public string Hash(byte[] data)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = data.AsBuffer();
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }

        public string HMAC256(string input, string key)
        {
            throw new NotImplementedException();
        }
    }
}
