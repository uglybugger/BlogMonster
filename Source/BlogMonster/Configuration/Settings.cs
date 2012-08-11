using System;
using System.Reflection;

namespace BlogMonster.Configuration
{
    internal class Settings : ISettings
    {
        private static Assembly[] _blogPostAssemblies;
        private static Type _controllerType;
        private static Func<string, bool> _resourceNameFilter;

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

        internal static void Configure(Assembly[] blogPostAssemblies, Type controllerType, Func<string, bool> resourceNameFilter)
        {
            _resourceNameFilter = resourceNameFilter;
            _controllerType = controllerType;
            _blogPostAssemblies = blogPostAssemblies;
        }
    }
}