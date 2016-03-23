//This is from http://stackoverflow.com/questions/10055158/is-there-a-json-web-token-jwt-example-in-c
//Cannot find the original author sorry!

using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace XamlingCore.UWP.Auth
{
    public enum JwtHashAlgorithm
    {
        RS256,
        HS384,
        HS512
    }

    //class Hasher
    //{
    //    private readonly string _key;
    //    private MacAlgorithmProvider macAlgorithmProvider;
    //    public Hasher(string hashType, string key)
    //    {
    //        _key = key;
    //        macAlgorithmProvider = MacAlgorithmProvider.OpenAlgorithm(hashType);
            
    //    }


    //    public string ComputeHash(string value)
    //    {
    //        byte[] hash;

    //        IBuffer buffKey = CryptographicBuffer.ConvertStringToBinary(_key, BinaryStringEncoding.Utf8);
    //        IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf8);

    //        CryptographicKey hmacKey = macAlgorithmProvider.CreateKey(buffKey);
    //        // Sign the key and message together.
    //        IBuffer buffHMAC = CryptographicEngine.Sign(hmacKey, buffMsg);
            
    //        CryptographicBuffer.CopyToByteArray(buffHMAC, out hash);
    //        var shaStr = string.Empty;

    //        for (var i = 0; i < hash.Length; i++)
    //        {
    //            shaStr += hash[i].ToString("X2");
    //        }

    //        return shaStr;
    //    }
    //}

    public class JsonWebTokenParser
    {
        //private static Dictionary<JwtHashAlgorithm, Func<string, string, string>> HashAlgorithms;

        static JsonWebTokenParser()
        {
            //HashAlgorithms = new Dictionary<JwtHashAlgorithm, Func<string, string, string>>
            //{
            //    { JwtHashAlgorithm.RS256, (key, value) =>
            //    {
            //        var sha = new Hasher("HMAC_SHA256", key);  return sha.ComputeHash(value);  } },
            //    { JwtHashAlgorithm.HS384, (key, value) =>
            //    {
            //        var sha = new Hasher("HMAC_SHA384", key);  return sha.ComputeHash(value);  } },
            //    { JwtHashAlgorithm.HS512, (key, value) =>
            //    {
            //        var sha = new Hasher("HMAC_SHA512", key);  return sha.ComputeHash(value);  } }
            //};
        }

        //public static string Encode(object payload, string key, JwtHashAlgorithm algorithm)
        //{
        //    return Encode(payload, Encoding.UTF8.GetBytes(key), algorithm);
        //}

        //public static string Encode(object payload, byte[] keyBytes, JwtHashAlgorithm algorithm)
        //{
        //    var segments = new List<string>();
        //    var header = new { alg = algorithm.ToString(), typ = "JWT" };

        //    byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
        //    byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None));
        //    //byte[] payloadBytes = Encoding.UTF8.GetBytes(@"{"iss":"761326798069-r5mljlln1rd4lrbhg75efgigp36m78j5@developer.gserviceaccount.com","scope":"https://www.googleapis.com/auth/prediction","aud":"https://accounts.google.com/o/oauth2/token","exp":1328554385,"iat":1328550785}");

        //    segments.Add(Base64UrlEncode(headerBytes));
        //    segments.Add(Base64UrlEncode(payloadBytes));

        //    var stringToSign = string.Join(".", segments.ToArray());

        //    var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

        //    byte[] signature = HashAlgorithms[algorithm](keyBytes, bytesToSign);
        //    segments.Add(Base64UrlEncode(signature));

        //    return string.Join(".", segments.ToArray());
        //}

        public static string Decode(string token, string key, bool verify = true)
        {
            var parts = token.Split('.');
            var header = parts[0];
            var payload = parts[1];
            byte[] crypto = Base64UrlDecode(parts[2]);

            var b1 = Base64UrlDecode(header);
            var headerJson = Encoding.UTF8.GetString(b1, 0, b1.Length);
            var headerData = JObject.Parse(headerJson);
            var b2 = Base64UrlDecode(payload);
            var payloadJson = Encoding.UTF8.GetString(b2, 0, b2.Length);
            var payloadData = JObject.Parse(payloadJson);

            //if (verify)
            //{
            //    var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
            //    var keyBytes = Encoding.UTF8.GetBytes(key);
            //    var algorithm = (string)headerData["alg"];

            //    var signature = HashAlgorithms[GetHashAlgorithm(algorithm)](key, string.Concat(header, ".", payload));
            //    var decodedCrypto = Convert.ToBase64String(crypto);
            //   // var decodedSignature = Convert.ToBase64String(signature);

            //    if (decodedCrypto != signature)
            //    {
            //        throw new InvalidOperationException(string.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, signature));
            //    }
            //}

            return payloadData.ToString();
        }

        private static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "RS256": return JwtHashAlgorithm.RS256;
                case "HS384": return JwtHashAlgorithm.HS384;
                case "HS512": return JwtHashAlgorithm.HS512;
                default: throw new InvalidOperationException("Algorithm not supported.");
            }
        }

        // from JWT spec
        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        // from JWT spec
        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
    }
}