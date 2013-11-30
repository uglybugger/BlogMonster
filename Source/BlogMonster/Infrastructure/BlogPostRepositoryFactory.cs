using System.Collections.Generic;
using System.Linq;
using BlogMonster.Domain.Entities;
using BlogMonster.Domain.Repositories;

namespace BlogMonster.Infrastructure
{
    public class BlogPostRepositoryFactory
    {
        private readonly IEnumerable<IBlogPostLoader> _blogPostLoaders;
        private readonly IClock _clock;

        public BlogPostRepositoryFactory(IEnumerable<IBlogPostLoader> blogPostLoaders, IClock clock)
        {
            _blogPostLoaders = blogPostLoaders;
            _clock = clock;
        }

        public IRepository<BlogPost> Create()
        {
            var repository = new BlogPostMemoryRepository(_clock);
            var posts = _blogPostLoaders.SelectMany(bpl => bpl.LoadPosts())
                                        .OrderByDescending(bp => bp.PostDate)
                                        .ToArray();
            foreach (var post in posts) repository.Add(post);
            return repository;
        }
    }
}