using System.Reflection;
using System.Runtime.InteropServices;
using BlogMonster.Domain.Entities;
using BlogMonster.Tests.Scenarios;
using UniMock.Core.Attributes;
using UniMock.WithNSubstitute;

[assembly: AssemblyTitle("BlogMonster.Tests")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("4e3f3662-c066-49b5-9db9-dcede2ab3a24")]

// UniMock settings

[assembly: AssembliesToStub(typeof (BlogPost))]
[assembly: StubFactory(typeof (NSubstituteStubFactory))]
[assembly: ScenarioAssemblies(typeof (UnitTestBlogPostAssemblyScenario))]