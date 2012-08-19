using System;
using System.Reflection;

namespace BlogMonster.Configuration
{
    public class RssFeedConfigurator
    {
        private readonly Assembly[] _assemblies;
        private readonly Type _controllerType;
        private readonly Func<string, bool> _resourceNameFilter;

        internal RssFeedConfigurator(Assembly[] assemblies, Type controllerType, Func<string, bool> resourceNameFilter)
        {
            _assemblies = assemblies;
            _controllerType = controllerType;
            _resourceNameFilter = resourceNameFilter;
        }

        public SiteBaseUrlConfigurator WithRssSettings(RssFeedSettings rssFeedSettings)
        {
            return new SiteBaseUrlConfigurator(_assemblies, _controllerType, _resourceNameFilter, rssFeedSettings);
        }
    }
}