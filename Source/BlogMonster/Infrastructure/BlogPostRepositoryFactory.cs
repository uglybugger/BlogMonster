using System.Linq;
using BlogMonster.Domain.Entities;
using BlogMonster.Domain.Repositories;

namespace BlogMonster.Infrastructure
{
    public class BlogPostRepositoryFactory
    {
        private readonly BlogPostLoader _blogPostLoader;

        public BlogPostRepositoryFactory(BlogPostLoader blogPostLoader)
        {
            _blogPostLoader = blogPostLoader;
        }

        public IRepository<BlogPost> Create()
        {
            var repository = new MemoryRepository<BlogPost>();
            var posts = _blogPostLoader.LoadPosts();
            foreach (var post in posts) repository.Add(post);
            return repository;
        }
    }
}