using System;
using System.Reflection;

namespace BlogMonster.Configuration
{
    public class FilterConfigurator
    {
        private readonly Assembly[] _assemblies;
        private readonly Type _controllerType;

        internal FilterConfigurator(Assembly[] assemblies, Type controllerType)
        {
            _assemblies = assemblies;
            _controllerType = controllerType;
        }

        public RssFeedConfigurator WithResourceNameFilter(Func<string, bool> resourceNameFilter)
        {
            return new RssFeedConfigurator(_assemblies, _controllerType, resourceNameFilter);
        }
    }
}