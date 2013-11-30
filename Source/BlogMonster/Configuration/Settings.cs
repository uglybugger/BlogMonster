using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using BlogMonster.Infrastructure;

namespace BlogMonster.Configuration
{
    internal class Settings : ISettings
    {
        private static Assembly[] _blogPostAssemblies;
        private static Type _controllerType;
        private static Func<string, bool> _resourceNameFilter;
        private static RssFeedSettings _rssFeedSettings;
        private static string _url;
        private static IBlogPostLoader[] _additionalBlogPostLoaders;

        public Assembly[] BlogPostAssemblies
        {
            get { return _blogPostAssemblies; }
        }

        public Type ControllerType
        {
            get { return _controllerType; }
        }

        public Func<string, bool> ResourceNameFilter
        {
            get { return _resourceNameFilter; }
        }

        public string Url
        {
            get { return _url; }
        }

        public RssFeedSettings RssFeedSettings
        {
            get { return _rssFeedSettings; }
        }

        public IEnumerable<IBlogPostLoader> AdditionalBlogPostLoaders
        {
            get { return _additionalBlogPostLoaders; }
        }

        internal static void Configure(Assembly[] blogPostAssemblies,
                                       Type controllerType,
                                       Func<string, bool> resourceNameFilter,
                                       RssFeedSettings rssFeedSettings,
                                       string url,
                                       RouteCollection routeTable,
                                       IBlogPostLoader[] additionalBlogPostLoaders)
        {
            _resourceNameFilter = resourceNameFilter;
            _controllerType = controllerType;
            _blogPostAssemblies = blogPostAssemblies;
            _url = url;
            _rssFeedSettings = rssFeedSettings;
            _additionalBlogPostLoaders = additionalBlogPostLoaders;

            var controllerName = controllerType.Name.Replace("Controller", string.Empty);

            routeTable.MapRoute("blogPostById", "blog/{id}", new {controller = controllerName, action = "PostById"}
                );

            routeTable.MapRoute("blogPostByDate",
                "blog/{year}/{month}/{day}/{id}",
                new {controller = controllerName, action = "PostByDateAndId"}
                );
            routeTable.MapRoute("blog", "blog", new {controller = controllerName, action = "Index"}
                );
        }
    }
}