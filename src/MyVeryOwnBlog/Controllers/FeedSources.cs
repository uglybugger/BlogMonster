using System;
using System.Diagnostics;
using System.ServiceModel.Syndication;
using BlogMonster.Configuration;
using BlogMonster.Infrastructure.SyndicationFeedSources;
using BlogMonster.Infrastructure.SyndicationFeedSources.Embedded;
using MyVeryOwnBlog.Posts;

namespace MyVeryOwnBlog.Controllers
{
    public static class FeedSources
    {
        private static readonly ISyndicationFeedSource _gitHub;
        private static readonly IEmbeddedSyndicationFeedSource _personal;

        public static ISyndicationFeedSource GitHub
        {
            get { return _gitHub; }
        }

        public static IEmbeddedSyndicationFeedSource Personal
        {
            get { return _personal; }
        }

        static FeedSources()
        {
            _gitHub = BlogMonsterBuilder.FromUrl(new Uri("https://github.com/uglybugger.atom"))
                                        .Grr();

            var personalBasePostUri = Debugger.IsAttached ? new Uri("/personal/post", UriKind.Relative) : new Uri("http://www.example.com/personal/post");
            var personalBaseImageUri = Debugger.IsAttached ? new Uri("/personal/image", UriKind.Relative) : new Uri("http://www.example.com/personal/image");

            _personal = BlogMonsterBuilder.FromEmbeddedResources(typeof (MyVeryOwnBlogPostsAssemblyMarker).Assembly)
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