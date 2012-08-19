using System;
using System.Reflection;
using System.ServiceModel.Syndication;

namespace BlogMonster.Configuration
{
    public class SiteBaseUrlConfigurator
    {
        private readonly Assembly[] _assemblies;
        private readonly Type _controllerType;
        private readonly Func<string, bool> _resourceNameFilter;
        private readonly SyndicationPerson _author;

        internal SiteBaseUrlConfigurator(Assembly[] assemblies, Type controllerType, Func<string, bool> resourceNameFilter, SyndicationPerson author)
        {
            _assemblies = assemblies;
            _controllerType = controllerType;
            _resourceNameFilter = resourceNameFilter;
            _author = author;
        }

        public FinalConfigurator WithSiteBaseUrl(string url)
        {
            return new FinalConfigurator(_assemblies, _controllerType, _resourceNameFilter, _author, url);
        }
    }
}