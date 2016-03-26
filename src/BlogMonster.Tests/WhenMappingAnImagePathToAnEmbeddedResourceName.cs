using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BlogMonster.Infrastructure.SyndicationFeedSources.Embedded;
using NUnit.Framework;
using Shouldly;

namespace BlogMonster.Tests
{
    [TestFixture]
    public class WhenMappingAnImagePathToAnEmbeddedResourceName : TestFor<EmbeddedResourceImagePathMapper>
    {
        private Assembly _testAssembly;
        private string[] _manifestResourceNames;

        private Uri[] _remappedImageUris;

        protected override EmbeddedResourceImagePathMapper GivenSubject()
        {
            _testAssembly = Assembly.GetExecutingAssembly();
            _manifestResourceNames = _testAssembly.GetManifestResourceNames();
            return new EmbeddedResourceImagePathMapper(new PathFactory(new Uri("http://www.example.com/feed/post"), new Uri("http://www.example.com/feed/image")));
        }

        protected override void When()
        {
            var resourceName = _manifestResourceNames
                .Where(n => n.Contains("image"))
                .Where(n => n.EndsWith(".markdown"))
                .First();

            string baseResourcePath;
            DateTimeOffset postDate;
            EmbeddedResourceBlogPostLoader.ExtractBaseResourcePathAndPostDate(resourceName, out baseResourcePath, out postDate);

            string markdown;
            using (var stream = _testAssembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    markdown = Encoding.UTF8.GetString(ms.GetBuffer());
                }
            }

            Subject.ReMapImagePaths(markdown, baseResourcePath, out _remappedImageUris);
        }

        [Test]
        public void TheResultShouldMatchAnEmbeddedResource()
        {
            foreach (var uri in _remappedImageUris)
            {
                var resourcePath = uri.LocalPath.Replace("/feed/image/", string.Empty).TrimEnd('/');
                _manifestResourceNames.ShouldContain(resourcePath);
            }
        }
    }
}