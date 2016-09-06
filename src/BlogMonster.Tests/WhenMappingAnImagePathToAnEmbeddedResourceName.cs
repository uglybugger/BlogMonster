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

        protected override EmbeddedResourceImagePathMapper Given()
        {
            _testAssembly = Assembly.GetExecutingAssembly();
            _manifestResourceNames = _testAssembly.GetManifestResourceNames();
            return new EmbeddedResourceImagePathMapper(new PathFactory(new Uri("http://www.example.com/feed/post"), new Uri("http://www.example.com/feed/image")));
        }

        protected override void When()
        {
            var postResourceName = _manifestResourceNames
                .Where(n => n.Contains("This post has an image"))
                .Where(n => n.EndsWith(".markdown"))
                .First();

            string baseResourcePath;
            DateTimeOffset postDate;
            postResourceName.ExtractBaseResourcePathAndPostDate(out baseResourcePath, out postDate);

            var markdown = _testAssembly.ReadResource(postResourceName);
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