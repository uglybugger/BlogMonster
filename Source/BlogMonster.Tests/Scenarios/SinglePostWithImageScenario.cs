using System.Collections.Generic;
using System.Linq;
using BlogMonster.Infrastructure;
using UniMock.Core.BaseTests;

namespace BlogMonster.Tests.Scenarios
{
    public class SinglePostWithImageScenario : IScenario
    {
        private readonly UnitTestBlogPostAssemblyScenario _unitTestBlogPostAssemblyScenario;
        private readonly IBlogPostResourceNameFilter _blogPostResourceNameFilter;

        public SinglePostWithImageScenario(UnitTestBlogPostAssemblyScenario unitTestBlogPostAssemblyScenario)
        {
            _unitTestBlogPostAssemblyScenario = unitTestBlogPostAssemblyScenario;
            _blogPostResourceNameFilter = new SinglePostWithImageFilter();
        }

        private class SinglePostWithImageFilter : IBlogPostResourceNameFilter
        {
            public IEnumerable<string> Filter(IEnumerable<string> resourceNames)
            {
                return resourceNames.Where(rn => rn.Contains(".SinglePostWithImage."));
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