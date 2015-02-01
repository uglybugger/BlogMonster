using NUnit.Framework;
using UniMock.Core;
using UniMock.Core.BaseTests;

namespace BlogMonster.Tests
{
    /*
     * Hi, and welcome to UniMock!
     * 
     * UniMock is designed to give your tests a simple look and feel
     * You'll need to remove the inappropriate attributes from here and include the namespace of your favourite testing framework.  -andrewh
     * 
     * 
     */

    [TestFixture]
    public abstract class TestFor<TSubject> : TestFor<TSubject, NullScenario>
        where TSubject : class
    {
    }

    [TestFixture]
    public abstract class TestFor<TSubject, TScenario> : UniverseFor<TSubject, TScenario>
        where TSubject : class where TScenario : IScenario
    {
        [SetUp]
        public virtual void Initialize()
        {
            BigBang();
            When();
        }

        protected abstract void When();

        [TearDown]
        public void Cleanup()
        {
            BigCrunch();
        }
    }
}