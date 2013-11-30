using System.Linq;
using Autofac;
using BlogMonster.Configuration;
using BlogMonster.Domain.Entities;
using BlogMonster.Infrastructure;
using BlogMonster.Tests.Scenarios;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using Shouldly;
using UniMock.Core;
using UniMock.Core.BaseTests;

namespace BlogMonster.Tests
{
    [TestFixture]
    public class WhenLoadingOneSimpleBlogPostWithOneImage : TestFor<EmbeddedResourceBlogPostLoader, SinglePostWithImageScenario>
    {
        private const string _someImageControllerPath = "/Some/Image";
        private BlogPost[] _result;

        public void RegisterMarkDownTransformationStuff(ContainerBuilder builder)
        {
            builder.RegisterType<MarkDownTransformer>()
                 .AsImplementedInterfaces()
                 .ForUniMock();

            builder.RegisterType<EmbeddedResourceImagePathMapper>()
                .AsImplementedInterfaces()
                .ForUniMock();
        }

        protected override EmbeddedResourceBlogPostLoader GivenSubject()
        {
            Stub<ISiteBaseUrlProvider>().ImageRelativeUrl.ReturnsForAnyArgs(_someImageControllerPath);
            return base.GivenSubject();
        }

        private string OnRemapImagePaths(CallInfo callInfo)
        {
            return (string) callInfo.Args()[0];
        }

        protected override void When()
        {
            _result = Subject.LoadPosts().ToArray();
        }

        [Test]
        public void ThereShouldBeOneBlogPost()
        {
            _result.Count().ShouldBe(1);
        }

        [Test]
        public void ThereShouldBeARelativeLinkToAnImageControllerAction()
        {
            _result.Single().Html.ShouldContain(_someImageControllerPath);
        }
    }
}