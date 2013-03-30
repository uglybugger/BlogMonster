using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace BlogMonster.Configuration
{
    internal class Settings : ISettings
    {
        private static Assembly[] _blogPostAssemblies;
        private static Type _controllerType;
        private static Func<string, bool> _resourceNameFilter;
        private static RssFeedSettings _rssFeedSettings;
        private static string _url;

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

        internal static void Configure(Assembly[] blogPostAssemblies, Type controllerType, Func<string, bool> resourceNameFilter, RssFeedSettings rssFeedSettings, string url, RouteCollection routeTable)
        {
            _resourceNameFilter = resourceNameFilter;
            _controllerType = controllerType;
            _blogPostAssemblies = blogPostAssemblies;
            _url = url;
            _rssFeedSettings = rssFeedSettings;

            var controllerName = controllerType.Name.Replace("Controller", string.Empty);

            routeTable.MapRoute(
                         name: "blogPost",
                         url: "blog/{id}",
                         defaults: new { controller = controllerName, action = "Index", id = UrlParameter.Optional }
                         );

            routeTable.MapRoute(
                         name: "blogPostByDate",
                         url: "blog/{year}/{month}/{day}/{id}",
                         defaults: new { controller = controllerName, action = "Index" }
                         );
        }
    }
}