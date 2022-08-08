using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace ProxyKit
{
    public static class HttpRequestHeadersExtensions
    {
        public static void FixHeaderValues(this HttpRequestHeaders headers)
        {
            var keys = headers.Select(u => u.Key).ToList();
            foreach (var key in keys)
            {
                IEnumerable<string> value;
                if (headers.Contains(key)
                    && headers.TryGetValues(key, out value)
                    && ShouldEncode(value))
                {
                    headers.Remove(key);
                    headers.Add(key, Encode(value));
                }
            }
        }
        private static bool ShouldEncode(IEnumerable<string> value)
        {
            foreach (var item in value)
            {
                var x = item
                    .Replace(" ", "")
                    .Replace(">", "")
                    .Replace("<", "")
                    .Replace("\\", "")
                    .Replace("\"", "")
                    .Replace("&", "");

                if (x != System.Web.HttpUtility.HtmlEncode(x))
                    return true;
            }
            return false;
        }

        private static IEnumerable<string> Encode(IEnumerable<string> value)
        {
            List<string> result = new List<string>();
            foreach (var item in value)
                result.Add(System.Web.HttpUtility.HtmlEncode(item));
            return result;
        }
    }
}
