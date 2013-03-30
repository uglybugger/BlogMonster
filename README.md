BlogMonster
===========

# Getting started

## Global.asax.cs

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
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
                                   .WithRouteTable(RouteTable.Routes)
                                   .Grr();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }

## Post.cshtml

	@model BlogMonster.Web.ViewModels.BlogPostViewModel

	@{
		ViewBag.Title = Model.Title;
		ViewBag.SubTitle = Model.PostDate;
		Layout = "~/Views/Shared/_Layout.cshtml";
	}

	<div class="row">
		<div class="span11">
			<div style="text-align: right;" class="content">
				<h1>
					<a href="@Model.Permalink">@Model.Title</a></h1>
				<h2>@Model.PostDate</h2>
			</div>
		</div>
	</div>

	<div class="row">
		<div class="span11">
			<div style="padding: 10px;">
				@Html.Raw(Model.Html)
			</div>
			<hr style="height: 1px; color: #cccccc;"/>
		</div>
	</div>

## Archive.chtml

	@using BlogMonster.Domain.Entities
	@using BlogMonster.Extensions
	@model BlogMonster.Web.ViewModels.ArchiveViewModel

	<div id="archiveWidget" class="widget">
		<h1>Archive</h1>
		@foreach (var year in Model.Posts)
		{
			<h2>@year.Key</h2>

			<div class="year">
				@foreach (var month in year.Value)
				{
					<h3>@month.Key</h3>
					<div class="month">
						@foreach (BlogPost post in month.Value)
						{
							string tooltip = "{0}/{1}".FormatWith(post.PostDate.Day, post.PostDate.Month);
							string linkText = post.Title.CoalesceIfWhiteSpace("Untitled Post");
							<a href="/blog/@post.Permalinks.First()" class="post" title="@tooltip">@linkText</a><br/>
						}
					</div>
				}
			</div>
		}
	</div>
