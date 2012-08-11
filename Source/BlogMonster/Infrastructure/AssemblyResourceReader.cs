using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BlogMonster.Configuration;

namespace BlogMonster.Infrastructure
{
    public class AssemblyResourceReader
    {
        private readonly ISettings _settings;

        public AssemblyResourceReader(ISettings settings)
        {
            _settings = settings;
        }

        public Stream GetManifestResourceStream(string id)
        {
            var assembly = AssemblyContainingResourceId(id);
            if (assembly == null) throw new InvalidOperationException("No resource with the given ID was found.");

            return assembly.GetManifestResourceStream(id);
        }

        public Assembly AssemblyContainingResourceId(string id)
        {
            var assembly = _settings.BlogPostAssemblies
                .Where(a => a.GetManifestResourceNames().Contains(id))
                .SingleOrDefault();

            return assembly;
        }
    }
}