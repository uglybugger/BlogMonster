using System;
using System.Linq;
using BlogMonster.Configuration;
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
            var syndicationFeedSource = BlogMonsterBuilder.FromUrl(new Uri("http://widget.stagram.com/rss/n/yolo"))
                                                          .Grr();

            syndicationFeedSource.Feed.Items.Count().ShouldBeGreaterThan(0);
        }
    }
}