using System;
using System.Linq;
using System.ServiceModel.Syndication;
using BlogMonster.Configuration;
using BlogMonster.Extensions;
using BlogMonster.Infrastructure.SyndicationFeedSources.Embedded;
using NUnit.Framework;
using Shouldly;

namespace BlogMonster.Tests
{
    [TestFixture]
    public class WhenLoadingOneSimpleBlogPostWithOneImage : TestFor<IEmbeddedSyndicationFeedSource>
    {
        private const string _someImageControllerPath = "/Some/Image";

        protected override IEmbeddedSyndicationFeedSource GivenSubject()
        {
            return BlogMonsterBuilder.FromEmbeddedResources(GetType().Assembly)
                                     .WithResourceNameFilter(s => s.Contains(".SinglePostWithImage.") && s.EndsWith(".markdown"))
                                     .WithRssSettings(new RssFeedSettings("feedId",
                                                                          "title",
                                                                          "description",
                                                                          new SyndicationPerson(),
                                                                          "http://www.example.com/image.jpg",
                                                                          "language",
                                                                          "copyright",
                                                                          new Uri("http://www.example.com")))
                                     .WithBaseUris(new Uri("http://www.example.com"), new Uri("http://www.example.com/" + _someImageControllerPath + "/"))
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
        public void ThereShouldBeARelativeLinkToAnImageControllerAction()
        {
            Subject.Feed.Items.Single().Content.ToHtml().ShouldContain(_someImageControllerPath);
        }

        [Test]
        public void TheTitleShouldBeCorrect()
        {
            Subject.Feed.Items.Single().Title.ToHtml().ShouldContain("This post has an image");
        }

        [Test]
        public void TheSummaryShouldContainTheCorrectContent()
        {
            Subject.Feed.Items.Single().Summary.ToHtml().ShouldContain("This should be the summary.");
        }

        [Test]
        public void TheContentShouldNotBeIncludedInTheSummary()
        {
            Subject.Feed.Items.Single().Summary.ToHtml().ShouldNotContain("This should be in the content.");
        }

        [Test]
        public void TheContentShouldBeInTheContent()
        {
            Subject.Feed.Items.Single().Content.ToHtml().ShouldContain("This should be in the content.");
        }
    }
}