using System.Collections.Generic;
using System.Reflection;
using BlogMonster.Configuration;
using BlogMonster.Infrastructure;

namespace BlogMonster.Controllers
{
    public class BlogPostAssembliesProvider : IBlogPostAssembliesProvider
    {
        private readonly ISettings _settings;

        public BlogPostAssembliesProvider(ISettings settings)
        {
            _settings = settings;
        }

        public IEnumerable<Assembly> Assemblies
        {
            get { return _settings.BlogPostAssemblies; }
        }
    }
}