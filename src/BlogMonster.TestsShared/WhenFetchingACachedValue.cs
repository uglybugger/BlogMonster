using System;
using BlogMonster.Infrastructure;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace BlogMonster.Tests
{
    [TestFixture]
    public class WhenFetchingACachedValue
    {
        [Test]
        public void NothingShouldGoBang()
        {
            var clock = Substitute.For<IClock>();
            clock.UtcNow.Returns(new DateTimeOffset(2013, 11, 30, 17, 16, 00, TimeSpan.FromHours(10)));
            var cached = new Cached<object>(TimeSpan.FromSeconds(1), clock, () => new object());

            cached.Value.ShouldNotBe(null);
        }
    }

    [TestFixture]
    public class WhenFetchingAnExpiredCachedValue
    {
        private int _calls;

        [Test]
        public void TheFactoryMethodShouldBeCalledTheCorrectNumberOfTimes()
        {
            var clock = Substitute.For<IClock>();
            clock.UtcNow.Returns(new DateTimeOffset(2013, 11, 30, 17, 16, 00, TimeSpan.FromHours(10)));
            var cached = new Cached<object>(TimeSpan.FromSeconds(10),
                                            clock,
                                            delegate
                                            {
                                                _calls++;
                                                return new object();
                                            });

            cached.Value.ShouldNotBe(null);
            _calls.ShouldBe(1);

            clock.UtcNow.Returns(new DateTimeOffset(2013, 11, 30, 17, 16, 10, TimeSpan.FromHours(10)));
            cached.Value.ShouldNotBe(null);
            _calls.ShouldBe(2);

            clock.UtcNow.Returns(new DateTimeOffset(2013, 11, 30, 17, 16, 19, TimeSpan.FromHours(10)));
            cached.Value.ShouldNotBe(null);
            _calls.ShouldBe(2);
        }
    }
}