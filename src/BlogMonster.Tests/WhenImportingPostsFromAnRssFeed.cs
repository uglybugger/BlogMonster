using System;
using System.Linq;
using BlogMonster.Configuration;
using NUnit.Framework;
using Shouldly;

namespace BlogMonster.Tests
{
    [TestFixture]
    public class WhenImportingPostsFromAnRssFeed
    {
        [Test]
        public void ThereShouldBeAtLeastOnePost()
        {
            var syndicationFeedSource = BlogMonsterBuilder.FromUrl(new Uri("https://www.youtube.com/feeds/videos.xml?channel_id=UCR0UEDo7YMDOHjZuGeLJBCA"))
                                                          .Grr();

            syndicationFeedSource.Feed.Items.Count().ShouldBeGreaterThan(0);
        }
    }
}