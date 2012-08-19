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
        private static readonly ISettings _settings;
        private static readonly BlogPostViewModelFactory _blogPostViewModelFactory;
        private static readonly IRepository<BlogPost> _blogPostRepository;
        private static readonly BlogPostLoader _blogPostLoader;
        private static readonly IEmbeddedResourceImagePathMapper _imagePathMapper;
        private static readonly ISiteBaseUrlProvider _siteBaseUrlProvider;
        private static readonly IMarkDownTransformer _markDownTransformer;
        private static readonly IBlogPostAssembliesProvider _blogPostAssembliesProvider;
        private static readonly IBlogPostResourceNameFilter _blogPostResourceNameFilter;
        private static readonly IAssemblyResourceReader _assemblyResourceReader;
        private static readonly IArchiveProvider _archiveProvider;

        static ServiceLocator()
        {
            _settings = new Settings();
            _assemblyResourceReader = new AssemblyResourceReader(_settings);
            _siteBaseUrlProvider = new SiteBaseUrlProvider(_settings);
            _markDownTransformer = new MarkDownTransformer();
            _imagePathMapper = new EmbeddedResourceImagePathMapper(_siteBaseUrlProvider);
            _blogPostAssembliesProvider = new BlogPostAssembliesProvider(_settings);
            _blogPostResourceNameFilter = new BlogPostResourceNameFilter(_settings);
            _blogPostLoader = new BlogPostLoader(_imagePathMapper, _markDownTransformer, _blogPostAssembliesProvider, _blogPostResourceNameFilter);
            _blogPostRepository = new BlogPostRepositoryFactory(_blogPostLoader).Create();
            _archiveProvider = new ArchiveProvider(_blogPostRepository);
            _blogPostViewModelFactory = new BlogPostViewModelFactory(_siteBaseUrlProvider);
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
            get { return _blogPostRepository; }
        }

        public static IAssemblyResourceReader AssemblyResourceReader
        {
            get { return _assemblyResourceReader; }
        }

        public static IArchiveProvider ArchiveProvider
        {
            get { return _archiveProvider; }
        }
    }
}