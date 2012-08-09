using System.Linq;

namespace BlogMonster.Domain.Queries
{
    public abstract class QuerySingle<T>
    {
        public abstract T Filter(IQueryable<T> items);
    }
}