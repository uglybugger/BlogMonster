using System.Collections.Generic;
using System.Linq;
using BlogMonster.Domain.Queries;
using BlogMonster.Domain.Repositories;

namespace BlogMonster.Infrastructure
{
    public class MemoryRepository<T> : IRepository<T> where T : class
    {
        readonly List<T> _items = new List<T>();

        public IEnumerable<T> GetAll()
        {
            return _items.AsEnumerable();
        }

        public IQueryable<T> Query(Query<T> query)
        {
            return query.Filter(_items.AsQueryable());
        }

        public T Query(QuerySingle<T> query)
        {
            return query.Filter(_items.AsQueryable());

        }
    }
}