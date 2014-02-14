using System.Linq;
using BlogMonster.Domain.Entities;
using BlogMonster.Infrastructure;
using BlogMonster.Tests.Scenarios;
using NUnit.Framework;
using Shouldly;
using UniMock.Core.BaseTests;

namespace BlogMonster.Tests
{
    [TestFixture]
    public class WhenLoadingOneSimpleBlogPost : TestFor<EmbeddedResourceBlogPostLoader, SinglePostScenario>
    {
        private BlogPost[] _result;

        protected override void When()
        {
            _result = Subject.LoadFeed().ToArray();
        }

        [Test]
        public void ThereShouldBeOneBlogPost()
        {
            _result.Count().ShouldBe(1);
        }
    }
}