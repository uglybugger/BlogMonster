using System;
using System.Reflection;
using System.ServiceModel.Syndication;

namespace BlogMonster.Configuration
{
    internal class Settings : ISettings
    {
        private static Assembly[] _blogPostAssemblies;
        private static Type _controllerType;
        private static Func<string, bool> _resourceNameFilter;
        private static SyndicationPerson _author;
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

        public SyndicationPerson Author
        {
            get { return _author; }
        }

        public string Url
        {
            get { return _url; }
        }

        internal static void Configure(Assembly[] blogPostAssemblies, Type controllerType, Func<string, bool> resourceNameFilter, SyndicationPerson author, string url)
        {
            _resourceNameFilter = resourceNameFilter;
            _controllerType = controllerType;
            _blogPostAssemblies = blogPostAssemblies;
            _author = author;
            _url = url;
        }
    }
}