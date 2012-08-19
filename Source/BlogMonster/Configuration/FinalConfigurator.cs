using System;
using System.Reflection;
using System.ServiceModel.Syndication;

namespace BlogMonster.Configuration
{
    public class FinalConfigurator
    {
        private readonly Assembly[] _blogPostAssemblies;
        private readonly Type _controllerType;
        private readonly Func<string, bool> _resourceNameFilter;
        private readonly SyndicationPerson _author;
        private readonly string _url;

        internal FinalConfigurator(Assembly[] blogPostAssemblies, Type controllerType, Func<string, bool> resourceNameFilter, SyndicationPerson author, string url)
        {
            _blogPostAssemblies = blogPostAssemblies;
            _controllerType = controllerType;
            _resourceNameFilter = resourceNameFilter;
            _author = author;
            _url = url;
        }

        public void Grr()
        {
            Settings.Configure(_blogPostAssemblies, _controllerType, _resourceNameFilter, _author, _url);
        }
    }
}