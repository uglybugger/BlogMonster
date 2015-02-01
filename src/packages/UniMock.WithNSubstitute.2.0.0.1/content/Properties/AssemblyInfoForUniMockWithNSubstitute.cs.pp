using UniMock.Core.Attributes;
using UniMock.WithNSubstitute;

/*
 * Hi!
 * 
 * The attribute below comes from the UniMock.WithNSubstitute package. This tells UniMock that you'd like to use
 * NSubstitute as your stubbing/mocking framework.
 * 
 * By all means, you can use your own if you like. I haven't written any other plugins yet but one for Moq would
 * be a nice start if you feel like submitting a pull request :)
 * 
 * Have fun!
 * 
 * -andrewh
 * 
 */

[assembly: StubFactory(typeof (NSubstituteStubFactory))]