using System;
using System.Linq;
using BlogMonster.Configuration;
using BlogMonster.Controllers;
using BlogMonster.Domain.Entities;
using BlogMonster.Domain.Repositories;
using BlogMonster.Infrastructure;
using BlogMonster.Web.ViewModels;

namespace BlogMonster
{
    public static class ServiceLocator
    {
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
        private static readonly ISettings _settings;
        private static readonly BlogPostViewModelFactory _blogPostViewModelFactory;
        private static readonly Cached<IRepository<BlogPost>> _blogPostRepository;
        private static readonly SystemClock _clock;
        private static readonly EmbeddedResourceBlogPostLoader _blogPostLoader;
        private static readonly IEmbeddedResourceImagePathMapper _imagePathMapper;
        private static readonly ISiteBaseUrlProvider _siteBaseUrlProvider;
        private static readonly IMarkDownTransformer _markDownTransformer;
        private static readonly IBlogPostAssembliesProvider _blogPostAssembliesProvider;
        private static readonly IBlogPostResourceNameFilter _blogPostResourceNameFilter;
        private static readonly IAssemblyResourceReader _assemblyResourceReader;
        private static readonly Cached<IArchiveProvider> _archiveProvider;
        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable

        static ServiceLocator()
        {
            _settings = new Settings();
            _assemblyResourceReader = new AssemblyResourceReader(_settings);
            _siteBaseUrlProvider = new SiteBaseUrlProvider(_settings);
            _markDownTransformer = new MarkDownTransformer();
            _imagePathMapper = new EmbeddedResourceImagePathMapper(_siteBaseUrlProvider);
            _blogPostAssembliesProvider = new BlogPostAssembliesProvider(_settings);
            _blogPostResourceNameFilter = new BlogPostResourceNameFilter(_settings);
            _blogPostLoader = new EmbeddedResourceBlogPostLoader(_imagePathMapper,
                                                                 _markDownTransformer,
                                                                 _blogPostAssembliesProvider,
                                                                 _blogPostResourceNameFilter);
            _clock = new SystemClock();
            _blogPostViewModelFactory = new BlogPostViewModelFactory(_siteBaseUrlProvider);

            var blogPostLoaders = new[] {_blogPostLoader}.Union(_settings.AdditionalBlogPostLoaders).ToArray();

            _blogPostRepository = new Cached<IRepository<BlogPost>>(
                TimeSpan.FromMinutes(10),
                _clock,
                () => new BlogPostRepositoryFactory(blogPostLoaders, _clock).Create());

            _archiveProvider = new Cached<IArchiveProvider>(
                TimeSpan.FromMinutes(10),
                _clock,
                () => new ArchiveProvider(_blogPostRepository.Value));
        }

        public static ISettings Settings
        {
            get { return _settings; }
        }

        public static BlogPostViewModelFactory BlogPostViewModelFactory
        {
            get { return _blogPostViewModelFactory; }
        }

        public static IRepository<BlogPost> BlogPostRepository
        {
            get { return _blogPostRepository.Value; }
        }

        public static IAssemblyResourceReader AssemblyResourceReader
        {
            get { return _assemblyResourceReader; }
        }

        public static IArchiveProvider ArchiveProvider
        {
            get { return _archiveProvider.Value; }
        }

        public static ISiteBaseUrlProvider SiteBaseUrlProvider
        {
            get { return _siteBaseUrlProvider; }
        }
    }
}