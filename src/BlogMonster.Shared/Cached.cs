using System;
using System.Threading;
using BlogMonster.Infrastructure;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("BlogMonster.TestsStandard")]
namespace BlogMonster
{
    public class Cached<T> where T : class
    {
        private readonly TimeSpan _cacheTimeout;
        private readonly IClock _clock;
        private readonly object _mutex = new object();
        private readonly Func<T> _valueFunc;
        private T _cachedValue;
        private DateTimeOffset _cachedValueExpiryTime;

        public Cached(TimeSpan cacheTimeout, IClock clock, Func<T> valueFunc)
        {
            _cacheTimeout = cacheTimeout;
            _clock = clock;
            _valueFunc = valueFunc;
            _cachedValueExpiryTime = DateTimeOffset.MinValue;
        }

        public T Value
        {
            get
            {
                var cachedValue = _cachedValue;
                if (cachedValue != null && _clock.UtcNow < _cachedValueExpiryTime) return cachedValue;

                lock (_mutex)
                {
                    Thread.MemoryBarrier();

                    cachedValue = _cachedValue;
                    if (cachedValue != null && _clock.UtcNow < _cachedValueExpiryTime) return cachedValue;

                    _cachedValue = _valueFunc();
                    _cachedValueExpiryTime = _clock.UtcNow.Add(_cacheTimeout);
                    return _cachedValue;
                }
            }
        }
    }
}