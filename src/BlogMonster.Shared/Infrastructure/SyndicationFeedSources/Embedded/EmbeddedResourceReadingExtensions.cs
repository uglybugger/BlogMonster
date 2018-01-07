using System.IO;
using System.Reflection;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    internal static class EmbeddedResourceReadingExtensions
    {
        public static string ReadResource(this Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) throw new MissingAssemblyResourceException();

                var text = new StreamReader(stream).ReadToEnd();
                return text;
            }
        }

        public static string TryReadResource(this Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return null;

                var text = new StreamReader(stream).ReadToEnd();
                return text;
            }
        }
    }
}