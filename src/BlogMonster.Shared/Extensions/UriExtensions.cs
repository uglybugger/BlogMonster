using System;

namespace BlogMonster.Extensions
{
    internal static class UriExtensions
    {
        public static Uri EnsureTrailingSlash(this Uri uri)
        {
            if (uri.ToString().EndsWith("/")) return uri;

            var uriWithTrailingSlash = new Uri($"{uri}/", UriKind.RelativeOrAbsolute);
            return uriWithTrailingSlash;
        }
    }
}