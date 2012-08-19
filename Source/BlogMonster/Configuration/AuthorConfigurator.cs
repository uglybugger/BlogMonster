using System;
using System.Reflection;
using System.ServiceModel.Syndication;

namespace BlogMonster.Configuration
{
    public class AuthorConfigurator
    {
        private readonly Assembly[] _assemblies;
        private readonly Type _controllerType;
        private readonly Func<string, bool> _resourceNameFilter;

        internal AuthorConfigurator(Assembly[] assemblies, Type controllerType, Func<string, bool> resourceNameFilter)
        {
            _assemblies = assemblies;
            _controllerType = controllerType;
            _resourceNameFilter = resourceNameFilter;
        }

        public SiteBaseUrlConfigurator WithAuthor(SyndicationPerson author)
        {
            return new SiteBaseUrlConfigurator(_assemblies, _controllerType, _resourceNameFilter, author);
        }
    }
}