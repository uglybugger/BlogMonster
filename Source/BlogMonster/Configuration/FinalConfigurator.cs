using System;
using System.Reflection;

namespace BlogMonster.Configuration
{
    public class FinalConfigurator
    {
        private readonly Assembly[] _blogPostAssemblies;
        private readonly Type _controllerType;
        private readonly Func<string, bool> _resourceNameFilter;

        internal FinalConfigurator(Assembly[] blogPostAssemblies, Type controllerType, Func<string, bool> resourceNameFilter)
        {
            _blogPostAssemblies = blogPostAssemblies;
            _controllerType = controllerType;
            _resourceNameFilter = resourceNameFilter;
        }

        public void Grr()
        {
            Settings.Configure(_blogPostAssemblies, _controllerType, _resourceNameFilter);
        }
    }
}