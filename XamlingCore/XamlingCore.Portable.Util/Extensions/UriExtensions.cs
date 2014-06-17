using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XamlingCore.Portable.Util.Extensions
{
    public static class UriExtensions
    {
        private static readonly Regex QueryStringRegex = new Regex(@"[\?&](?<name>[^&=]+)=(?<value>[^&=]+)");

        public static Dictionary<string, string> ParseQueryString(this Uri uri)
        {
            if (uri == null)
                throw new ArgumentException("uri");

            var matches = QueryStringRegex.Matches(uri.OriginalString);

            var restul = new Dictionary<string, string>();

            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];

                if (restul.ContainsKey(match.Groups["name"].Value))
                {
                    continue;
                }

                restul.Add(match.Groups["name"].Value, match.Groups["value"].Value);

            }

            return restul;
        }
    }
}
