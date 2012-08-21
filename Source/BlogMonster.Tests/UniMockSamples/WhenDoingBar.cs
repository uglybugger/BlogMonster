using NSubstitute;
using NUnit.Framework;

namespace BlogMonster.Tests.UniMockSamples
{
    public class CanDoFoo
    {
        private readonly ICanDoBar _canDoBar;

        public CanDoFoo(ICanDoBar canDoBar)
        {
            _canDoBar = canDoBar;
        }

        public void DoBar()
        {
            _canDoBar.DoBar();
        }
    }

    public interface ICanDoBar
    {
        void DoBar();
    }

    [TestFixture]
    public class WhenDoingBar : TestFor<CanDoFoo>
    {
        protected override void When()
        {
            Subject.DoBar();
        }

        [Test]
        public void FooShouldAlsoBeDone()
        {
            //TODO: Assert on your favourite mocking framework that DoFoo was called.
            // You can get access to the stub that UniMock created for ICanDoBar by
            // calling Stub<ICanDoBar>().
            Stub<ICanDoBar>().Received().DoBar();
        }
    }
}