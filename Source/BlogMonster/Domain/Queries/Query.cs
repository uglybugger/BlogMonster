using System.Linq;

namespace BlogMonster.Domain.Queries
{
    public abstract class Query<T>
    {
        public abstract IQueryable<T> Filter(IQueryable<T> items);
    }
}