using System;

namespace BlogMonster.Extensions
{
    public static class ClassExtensions
    {
        public static TProp Coalesce<T, TProp>(this T o, Func<T, TProp> getter, TProp ifNull) where T : class
        {
            return o != null ? getter(o) : ifNull;
        }
    }
}