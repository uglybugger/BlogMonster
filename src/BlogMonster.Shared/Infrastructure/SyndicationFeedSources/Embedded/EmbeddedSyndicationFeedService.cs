using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    public class EmbeddedSyndicationFeedService : StaticSyndicationFeedSource, IEmbeddedSyndicationFeedSource
    {
        private readonly Assembly[] _assemblies;
        private readonly Lazy<List<ResourceNameCacheEntry>> _resourceNamesAssemblyLookup;

        public EmbeddedSyndicationFeedService(Assembly[] assemblies, SyndicationFeed feed, SystemClock clock) : base(feed, clock)
        {
            _assemblies = assemblies;

            _resourceNamesAssemblyLookup = new Lazy<List<ResourceNameCacheEntry>>(ConstructResourceNameAssemblyLookup);
        }

        public Stream GetStreamForImageResourceName(string resourceName)
        {
            return _resourceNamesAssemblyLookup.Value
                                               .Where(cacheEntry => cacheEntry.ResourceName == resourceName)
                                               .Select(cacheEntry => cacheEntry.Assembly.GetManifestResourceStream(cacheEntry.ResourceName))
                                               .FirstOrDefault();
        }

        private List<ResourceNameCacheEntry> ConstructResourceNameAssemblyLookup()
        {
            var result = new List<ResourceNameCacheEntry>();

            foreach (var assembly in _assemblies)
            {
                var resourceNames = assembly.GetManifestResourceNames();
                foreach (var resourceName in resourceNames)
                {
                    var cacheEntry = new ResourceNameCacheEntry
                                     {
                                         ResourceName = resourceName,
                                         Assembly = assembly,
                                     };

                    result.Add(cacheEntry);
                }
            }

            return result;
        }

        private class ResourceNameCacheEntry
        {
            public string ResourceName { get; set; }
            public Assembly Assembly { get; set; }
        }
    }
}