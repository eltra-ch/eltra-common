using System;
using System.Collections.Specialized;

#pragma warning disable 1591

namespace EltraCommon.Helpers
{
    public static class UrlHelper
    {
        public static string BuildUrl(string url, string path, NameValueCollection query)
        {
            var builder = new UriBuilder(url) { Path = path, Query = query.ToString() };

            return builder.ToString();
        }
    }
}
