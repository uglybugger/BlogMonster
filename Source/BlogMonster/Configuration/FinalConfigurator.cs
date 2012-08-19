using System;
using System.Reflection;

namespace BlogMonster.Configuration
{
    public class FinalConfigurator
    {
        private readonly Assembly[] _blogPostAssemblies;
        private readonly Type _controllerType;
        private readonly Func<string, bool> _resourceNameFilter;
        private readonly RssFeedSettings _rssFeedSettings;
        private readonly string _url;

        internal FinalConfigurator(Assembly[] blogPostAssemblies, Type controllerType, Func<string, bool> resourceNameFilter, RssFeedSettings rssFeedSettings, string url)
        {
            _blogPostAssemblies = blogPostAssemblies;
            _controllerType = controllerType;
            _resourceNameFilter = resourceNameFilter;
            _rssFeedSettings = rssFeedSettings;
            _url = url;
        }

        public void Grr()
        {
            Settings.Configure(_blogPostAssemblies, _controllerType, _resourceNameFilter, _rssFeedSettings, _url);
        }
    }
}