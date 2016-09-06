using NUnit.Framework;

namespace BlogMonster.Tests
{
    [TestFixture]
    public abstract class TestFor<TSubject> where TSubject : class
    {
        [SetUp]
        public virtual void SetUp()
        {
            Subject = Given();
            When();
        }

        [TearDown]
        public virtual void TearDown()
        {
        }

        protected TSubject Subject { get; private set; }
        protected abstract TSubject Given();
        protected abstract void When();
    }
}