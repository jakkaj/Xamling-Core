using System;
using System.Text;
using XamlingCore.Portable.Contract.Helpers;

namespace XamlingCore.Droid.Implementations.Helpers
{
    public class HashHelper : IHashHelper
    {
        public string Hash(byte[] data)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var hash = md5.ComputeHash(data);

            // step 2, convert byte array to hex string
            var result = new StringBuilder(hash.Length * 2);

            for (int i = 0; i < hash.Length; i++)
                result.Append(hash[i].ToString("X2"));

            return result.ToString();
        }

        public string HMAC256(string input, string key)
        {
            throw new NotImplementedException();
        }
    }
}
