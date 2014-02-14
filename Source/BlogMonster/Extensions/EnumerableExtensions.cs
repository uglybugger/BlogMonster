using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogMonster.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> DepthFirst<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> children)
        {
            foreach (var item in source)
            {
                yield return item;
                foreach (var descendant in children(item).DepthFirst<T>(children)) yield return descendant;
            }
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source) where T : class
        {
            return source.Where(item => item != null);
        }

        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        ///     Forces enumeration to allow chains of Do(xxx).Do(yyy).Do(zzz).Done() to be used.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="source"> </param>
        public static void Done<T>(this IEnumerable<T> source)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            source.Count();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}