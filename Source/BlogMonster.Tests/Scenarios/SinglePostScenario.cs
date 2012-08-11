using System.Collections.Generic;
using System.Linq;
using BlogMonster.Infrastructure;
using UniMock.Core.BaseTests;

namespace BlogMonster.Tests.Scenarios
{
    public class SinglePostScenario : IScenario
    {
        private readonly UnitTestBlogPostAssemblyScenario _unitTestBlogPostAssemblyScenario;
        private readonly IBlogPostResourceNameFilter _blogPostResourceNameFilter;

        public SinglePostScenario(UnitTestBlogPostAssemblyScenario unitTestBlogPostAssemblyScenario)
        {
            _unitTestBlogPostAssemblyScenario = unitTestBlogPostAssemblyScenario;
            _blogPostResourceNameFilter = new SinglePostFilter();
        }

        private class SinglePostFilter : IBlogPostResourceNameFilter
        {
            public IEnumerable<string> Filter(IEnumerable<string> resourceNames)
            {
                return resourceNames.Where(rn => rn.Contains("SinglePost"));
            }
        }

        public IBlogPostResourceNameFilter BlogPostResourceNameFilter
        {
            get { return _blogPostResourceNameFilter; }
        }

        public UnitTestBlogPostAssemblyScenario UnitTestBlogPostAssemblyScenario
        {
            get { return _unitTestBlogPostAssemblyScenario; }
        }
    }
}