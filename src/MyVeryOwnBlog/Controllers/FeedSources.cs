using System;
using System.ServiceModel.Syndication;
using BlogMonster.Configuration;
using BlogMonster.Infrastructure.SyndicationFeedSources;
using BlogMonster.Infrastructure.SyndicationFeedSources.Embedded;
using MyVeryOwnBlog.Posts;

namespace MyVeryOwnBlog.Controllers
{
    public static class FeedSources
    {
        public static ISyndicationFeedSource GitHub { get; }

        public static IEmbeddedSyndicationFeedSource Personal { get; }

        static FeedSources()
        {
            GitHub = BlogMonsterBuilder.FromUrl(new Uri("https://github.com/uglybugger.atom"))
                .Grr();

            var personalBasePostUri = new Uri("http://localhost:29247/personal/post");
            var personalBaseImageUri = new Uri("http://localhost:29247/personal/image");

            Personal = BlogMonsterBuilder.FromEmbeddedResources(typeof (MyVeryOwnBlogPostsAssemblyMarker).Assembly)
                .WithResourceNameFilter(s => s.EndsWith(".markdown"))
                .WithRssSettings(new RssFeedSettings("http://www.example.com/",
                    "The Blog Monster Roars",
                    "A sample blog for the Blog Monster.",
                    new SyndicationPerson("monster@example.com",
                        "Blog Monster",
                        "http://www.example.com/"),
                    "http://www.example.com/content/me.jpg",
                    "en-AU",
                    "Copyright (C) Blog Monster. All rights reserved. Or I shall eat you.",
                    new Uri("http://www.example.com")))
                .WithBaseUris(personalBasePostUri, personalBaseImageUri)
                .Grr();
        }
    }
}