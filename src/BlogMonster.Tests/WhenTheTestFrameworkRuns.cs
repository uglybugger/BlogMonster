using NUnit.Framework;
using UniMock.Core.BaseTests;

namespace BlogMonster.Tests
{
    [TestFixture]
    public class WhenTheTestFrameworkRuns : TestFor<object>
    {
        protected override void When()
        {
        }

        [Test]
        public void ThePeopleShallRejoice()
        {
        }
    }
}