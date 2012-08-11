using System.Collections.Generic;
using System.Linq;
using BlogMonster.Domain.Queries;

namespace BlogMonster.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T item);
        IEnumerable<T> GetAll();
        IQueryable<T> Query(Query<T> query);
        T Query(QuerySingle<T> query);
    }
}