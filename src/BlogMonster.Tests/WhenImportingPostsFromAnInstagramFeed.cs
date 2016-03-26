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
            var syndicationFeedSource = BlogMonsterBuilder.FromUrl(new Uri("http://instagram.heroku.com/users/730570669.atom"))
                                                          .Grr();

            syndicationFeedSource.Feed.Items.Count().ShouldBeGreaterThan(0);
        }
    }
}