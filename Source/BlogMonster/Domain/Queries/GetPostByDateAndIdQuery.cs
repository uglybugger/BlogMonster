using System.Linq;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Domain.Queries
{
    public class GetPostByDateAndIdQuery : QuerySingle<BlogPost>
    {
        private readonly int _year;
        private readonly int _month;
        private readonly int _day;
        private readonly string _id;

        public GetPostByDateAndIdQuery(int year, int month, int day, string id)
        {
            _year = year;
            _month = month;
            _day = day;
            _id = id;
        }

        public override BlogPost Filter(IQueryable<BlogPost> items)
        {
            return items
                .Where(item => item.PostDate.Year == _year)
                .Where(item => item.PostDate.Month == _month)
                .Where(item => item.PostDate.Day == _day)
                .Where(item => item.Permalinks.Contains(_id))
                .FirstOrDefault();
        }
    }
}