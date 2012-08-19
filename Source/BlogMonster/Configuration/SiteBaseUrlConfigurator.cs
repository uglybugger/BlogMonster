using System;
using System.Reflection;

namespace BlogMonster.Configuration
{
    public class SiteBaseUrlConfigurator
    {
        private readonly Assembly[] _assemblies;
        private readonly Type _controllerType;
        private readonly Func<string, bool> _resourceNameFilter;
        private readonly RssFeedSettings _rssFeedSettings;

        internal SiteBaseUrlConfigurator(Assembly[] assemblies, Type controllerType, Func<string, bool> resourceNameFilter, RssFeedSettings rssFeedSettings)
        {
            _assemblies = assemblies;
            _controllerType = controllerType;
            _resourceNameFilter = resourceNameFilter;
            _rssFeedSettings = rssFeedSettings;
        }

        public FinalConfigurator WithSiteBaseUrl(string url)
        {
            return new FinalConfigurator(_assemblies, _controllerType, _resourceNameFilter, _rssFeedSettings, url);
        }
    }
}