using Autofac;
using UniMock.Core.BaseTests;

namespace $rootnamespace$
{
    public abstract class IntegrationTestFor<TSubject> : IntegrationTestFor<TSubject, NullScenario> where TSubject : class
    {
    }

    public abstract class IntegrationTestFor<TSubject, TScenario> : TestFor<TSubject, TScenario> where TScenario : IScenario where TSubject : class
    {
        protected abstract IContainer ConstructContainer();

        protected override IContainer CreateContainer()
        {
            return ConstructContainer();
        }

        // no overriding this - we're not going to use it if the caller constructs their own container.
        protected override sealed void OverrideRegistrations(ContainerBuilder builder)
        {
            base.OverrideRegistrations(builder);
        }
    }
}