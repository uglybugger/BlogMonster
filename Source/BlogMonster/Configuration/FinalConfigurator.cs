using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Routing;
using BlogMonster.Infrastructure;

namespace BlogMonster.Configuration
{
    public class FinalConfigurator
    {
        private readonly Assembly[] _blogPostAssemblies;
        private readonly Type _controllerType;
        private readonly Func<string, bool> _resourceNameFilter;
        private readonly RssFeedSettings _rssFeedSettings;
        private readonly string _url;
        private readonly RouteCollection _routeTable;
        private readonly List<IBlogPostLoader> _additionaBlogPostLoaders = new List<IBlogPostLoader>();

        internal FinalConfigurator(Assembly[] blogPostAssemblies,
                                   Type controllerType,
                                   Func<string, bool> resourceNameFilter,
                                   RssFeedSettings rssFeedSettings,
                                   string url,
                                   RouteCollection routeTable)
        {
            _blogPostAssemblies = blogPostAssemblies;
            _controllerType = controllerType;
            _resourceNameFilter = resourceNameFilter;
            _rssFeedSettings = rssFeedSettings;
            _url = url;
            _routeTable = routeTable;
        }

        public FinalConfigurator WithAdditionalBlogPostLoaders(params IBlogPostLoader[] addiBlogPostLoaders)
        {
            _additionaBlogPostLoaders.AddRange(addiBlogPostLoaders);
            return this;
        }

        public void Grr()
        {
            Settings.Configure(_blogPostAssemblies,
                               _controllerType,
                               _resourceNameFilter,
                               _rssFeedSettings,
                               _url,
                               _routeTable,
                               _additionaBlogPostLoaders.ToArray());
        }
    }
}