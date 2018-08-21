using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BlogMonster.Extensions
{
    internal static class StringExtensions
    {
        public static string RegexReplace(this string s, string pattern, string replacement)
        {
            return Regex.Replace(s, pattern, replacement);
        }

        public static string Join(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source.ToArray());
        }
    }
}