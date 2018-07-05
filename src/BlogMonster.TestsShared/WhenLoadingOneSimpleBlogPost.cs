﻿using System;
using System.Linq;
using System.ServiceModel.Syndication;
using BlogMonster.Configuration;
using BlogMonster.Infrastructure.SyndicationFeedSources.Embedded;
using NUnit.Framework;
using Shouldly;

namespace BlogMonster.Tests
{
    [TestFixture]
    public class WhenLoadingOneSimpleBlogPost : TestFor<IEmbeddedSyndicationFeedSource>
    {
        protected override IEmbeddedSyndicationFeedSource Given()
        {
            return BlogMonsterBuilder.FromEmbeddedResources(GetType().Assembly)
                                     .WithResourceNameFilter(s => s.Contains(".SinglePost.") && s.EndsWith(".markdown"))
                                     .WithRssSettings(new RssFeedSettings("feedId",
                                                                          "title",
                                                                          "description",
                                                                          new SyndicationPerson(),
                                                                          "http://www.example.com/image.jpg",
                                                                          "language",
                                                                          "copyright",
                                                                          new Uri("http://www.example.com/")))
                                     .WithBaseUris(new Uri("http://www.example.com"), new Uri("http://www.example.com/image/"))
                                     .Grr();
        }

        protected override void When()
        {
        }

        [Test]
        public void ThereShouldBeOneBlogPost()
        {
            Subject.Feed.Items.Count().ShouldBe(1);
        }

        [Test]
        public void AllBlogPostsLinksShouldHaveAbsoluteUris()
        {
            foreach (var item in Subject.Feed.Items)
            {
                foreach (var link in item.Links)
                {
                    link.GetAbsoluteUri().IsAbsoluteUri.ShouldBe(true);
                }
            }
        }
    }
}