using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Helpers;

namespace XamlingCore.NET.Implementations
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
            var hmacsha256 = new HMACSHA256(Encoding.Default.GetBytes(key));
            hmacsha256.ComputeHash(Encoding.Default.GetBytes(input));
            var k = hmacsha256.Hash.Aggregate("", (current, test) => current + test.ToString("X2"));

            return k;
        }

        public static string CreateSHA1(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
