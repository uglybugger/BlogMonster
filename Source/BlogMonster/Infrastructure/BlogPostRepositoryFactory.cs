using BlogMonster.Domain.Entities;
using BlogMonster.Domain.Repositories;

namespace BlogMonster.Infrastructure
{
    public class BlogPostRepositoryFactory
    {
        private readonly BlogPostLoader _blogPostLoader;
        private readonly IClock _clock;

        public BlogPostRepositoryFactory(BlogPostLoader blogPostLoader, IClock clock)
        {
            _blogPostLoader = blogPostLoader;
            _clock = clock;
        }

        public IRepository<BlogPost> Create()
        {
            var repository = new BlogPostMemoryRepository(_clock);
            var posts = _blogPostLoader.LoadPosts();
            foreach (var post in posts) repository.Add(post);
            return repository;
        }
    }
}