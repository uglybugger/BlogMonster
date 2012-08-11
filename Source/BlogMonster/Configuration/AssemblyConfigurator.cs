using System.Reflection;

namespace BlogMonster.Configuration
{
    public class AssemblyConfigurator
    {
        internal AssemblyConfigurator()
        {
        }

        public ControllerConfigurator WithAssembliesContainingPosts(params Assembly[] assemblies)
        {
            return new ControllerConfigurator(assemblies);
        }
    }
}