using System;
using System.IO;
using System.Linq;
using BlogMonster.Configuration;
using BlogMonster.Extensions;

namespace BlogMonster.Infrastructure
{
    public class AssemblyResourceReader : IAssemblyResourceReader
    {
        private readonly ISettings _settings;

        public AssemblyResourceReader(ISettings settings)
        {
            _settings = settings;
        }

        public Stream GetBestMatchingResourceStream(string id)
        {
            foreach (var assembly in _settings.BlogPostAssemblies)
            {
                var match = assembly.GetManifestResourceNames().SingleOrDefault(resourceName => resourceName.Contains(id));
                if (match != null) return assembly.GetManifestResourceStream(match);
            }

            throw new InvalidOperationException("Could not find a resource matching ID {0}".FormatWith(id));
        }
    }
}