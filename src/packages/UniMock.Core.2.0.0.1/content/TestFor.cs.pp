using UniMock.Core;
using UniMock.Core.BaseTests;

namespace $rootnamespace$
{
#error Hi! You need to tweak a couple of things before UniMock will work.
    /*
     * Hi, and welcome to UniMock!
     * 
     * UniMock is designed to give your tests a simple look and feel
     * You'll need to remove the inappropriate attributes from here and include the namespace of your favourite testing framework.  -andrewh
     * 
     * 
     */

    [TestClass]
    [TestFixture]
    public abstract class TestFor<TSubject> : TestFor<TSubject, NullScenario>
        where TSubject : class
    {
    }

    [TestClass]
    [TestFixture]
    public abstract class TestFor<TSubject, TScenario> : UniverseFor<TSubject, TScenario>
        where TSubject : class where TScenario : IScenario
    {
        [TestInitialize]
        [SetUp]
        public virtual void Initialize()
        {
            BigBang();
            When();
        }

        protected abstract void When();

        [TestCleanup]
        [TearDown]
        public void Cleanup()
        {
            BigCrunch();
        }
    }
}