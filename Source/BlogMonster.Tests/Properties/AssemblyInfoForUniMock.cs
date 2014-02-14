using BlogMonster.Configuration;
using BlogMonster.Tests;
using UniMock.Core.Attributes;

/*
 * Hi, and welcome to UniMock!
 * 
 * There are a couple of things you'll need to do to get started. The first is to tell UniMock which assemblies
 * you want to automatically create stubs/mocks for. Generally, this will be all the assemblies in your project
 * except for your test projects. You specify which assemblies to stub/mock by using the AssembliesToStub
 * attribute below.
 *
 * I've included a typeof(WhenDoingBar) to get you started. That's a sample test that's been dropped into your
 * test project so that you can see how to structure your tests.
 */

[assembly: AssembliesToStub(typeof (BlogMonsterBuilder))]

/*
 * The second thing you'll need to do is to tell UniMock where it can find all your test scenarios. Don't worry if
 * you don't have any of these yet or don't know what they are - just tell it to look in all of your test
 * assemblies for now and we'll come to scenarios later.
 */

[assembly: ScenarioAssemblies(typeof (TestFor<>))]

/*
 * Finally, thank you for taking the time to try out UniMock. I hope you find it useful. Please drop me a line
 * with any feedback you have, either at andrewh@uglybugger.org or via @uglybugger on Twitter.
 * 
 * Have fun!
 * 
 * -andrewh
 * 
*/