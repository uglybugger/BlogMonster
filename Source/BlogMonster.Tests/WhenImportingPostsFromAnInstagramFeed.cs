using System;
using System.Linq;
using BlogMonster.Infrastructure.BlogPostLoaders;
using NUnit.Framework;
using Shouldly;

namespace BlogMonster.Tests
{
    [TestFixture]
    public class WhenImportingPostsFromAnInstagramFeed
    {
        [Test]
        public void ThereShouldBeAtLeastOnePost()
        {
            var rssPostLoader = new RssBlogPostLoader(new Uri("http://widget.stagram.com/rss/n/yolo"));

            var posts = rssPostLoader.LoadPosts();

            posts.Count().ShouldBeGreaterThan(0);
        }
    }
}