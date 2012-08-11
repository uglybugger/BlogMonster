using System.Collections.Generic;
using System.Linq;
using BlogMonster.Configuration;
using BlogMonster.Infrastructure;

namespace BlogMonster.Controllers
{
    public class BlogPostResourceNameFilter : IBlogPostResourceNameFilter
    {
        private readonly ISettings _settings;

        public BlogPostResourceNameFilter(ISettings settings)
        {
            _settings = settings;
        }

        public IEnumerable<string> Filter(IEnumerable<string> resourceNames)
        {
            return resourceNames.Where(_settings.ResourceNameFilter);
        }
    }
}