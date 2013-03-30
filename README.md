BlogMonster
===========

# Getting started

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BlogMonsterConfigurator.Configure()
                .WithAssembliesContainingPosts(typeof (PostsAssemblyMarkerClass).Assembly)
                .WithControllerExtendingBlogMonsterController(typeof (BlogMonsterController))
                .WithResourceNameFilter(s => true)
                .WithRssSettings(new RssFeedSettings("http://www.codingforfunandprofit.com/",
                                                     "Coding for Fun and Profit",
                                                     "A blog of shared grief and despair",
                                                     new SyndicationPerson("andrewh@uglybugger.org", "Andrew Harcourt", "http://www.codingforfunandprofit.com/"),
                                                     "http://www.codingforfunandprofit.com/Content/Me.jpg",
                                                     "en-AU",
                                                     "Copyright (C) Andrew Harcourt. All rights reserved."))
                .WithSiteBaseUrl("http://www.codingforfunandprofit.com")
                .Grr();
        }
    }
