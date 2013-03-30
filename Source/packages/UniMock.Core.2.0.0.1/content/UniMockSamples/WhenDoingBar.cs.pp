namespace $rootnamespace$.UniMockSamples
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

    //TODO remove one of these depending on whether you're using NUnit or MSTest
    [TestFixture]
    [TestClass]
    public class WhenDoingBar : TestFor<CanDoFoo>
    {
        protected override void When()
        {
            Subject.DoBar();
        }

        //TODO remove one of these depending on whether you're using NUnit or MSTest
        [Test]
        [TestMethod]
        public void FooShouldAlsoBeDone()
        {
            //TODO: Assert on your favourite mocking framework that DoFoo was called.
            // You can get access to the stub that UniMock created for ICanDoBar by
            // calling Stub<ICanDoBar>().
        }
    }

}