using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogMonster.Extensions
{
    public static class StringExtensions
    {
        public static string FormatWith(this string s, params object[] args)
        {
            return String.Format(s, args);
        }

        public static string CoalesceIfWhiteSpace(this string s, string other)
        {
            return String.IsNullOrWhiteSpace(s) ? other : s;
        }

        public static IEnumerable<string> NotNullOrWhitespace(this IEnumerable<string> source)
        {
            return source
                .Where(s => !String.IsNullOrWhiteSpace(s));
        }
    }
}