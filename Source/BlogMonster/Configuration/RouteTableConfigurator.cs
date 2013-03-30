using System;
using System.Reflection;
using System.Web.Routing;

namespace BlogMonster.Configuration
{
    public class RouteTableConfigurator
    {
        private readonly Assembly[] _assemblies;
        private readonly Type _controllerType;
        private readonly Func<string, bool> _resourceNameFilter;
        private readonly RssFeedSettings _rssFeedSettings;
        private readonly string _siteBaseUrl;

        internal RouteTableConfigurator(Assembly[] assemblies, Type controllerType, Func<string, bool> resourceNameFilter, RssFeedSettings rssFeedSettings, string siteBaseUrl)
        {
            _assemblies = assemblies;
            _controllerType = controllerType;
            _resourceNameFilter = resourceNameFilter;
            _rssFeedSettings = rssFeedSettings;
            _siteBaseUrl = siteBaseUrl;
        }

        public FinalConfigurator WithRouteTable(RouteCollection routeTable)
        {
            return new FinalConfigurator(_assemblies, _controllerType, _resourceNameFilter, _rssFeedSettings, _siteBaseUrl, routeTable);
        }
    }
}