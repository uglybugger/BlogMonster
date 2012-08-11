using BlogMonster.Infrastructure;
using NSubstitute;
using UniMock.Core.BaseTests;

namespace BlogMonster.Tests.Scenarios
{
    public class UnitTestBlogPostAssemblyScenario : IScenario
    {
        private readonly IBlogPostAssembliesProvider _blogPostAssembliesProvider;

        public UnitTestBlogPostAssemblyScenario()
        {
            _blogPostAssembliesProvider = Substitute.For<IBlogPostAssembliesProvider>();
            BlogPostAssembliesProvider.Assemblies.Returns(new[] {GetType().Assembly});
        }

        public IBlogPostAssembliesProvider BlogPostAssembliesProvider
        {
            get { return _blogPostAssembliesProvider; }
        }
    }
}