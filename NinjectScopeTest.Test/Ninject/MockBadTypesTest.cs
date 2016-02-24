using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scoper.Exception;

namespace Scoper.Test.Ninject
{
    [TestClass]
    public class MockBadTypesTest :
        Scoper.Ninject.AutoScopeTest<MockBadTypesTest.TestScope>
    {
        public class TestScope : Scoper.Ninject.Scope
        {
            public Mock GoodMock { get; set; }
            public Mock<string> BadMock { get; set; }

            public override void Initialize()
            {
            }
        }

        [TestMethod, ExpectedException(typeof (InternalException))]
        public void NinjectScopeTest_ShouldThrowInvalidMockException()
        {
            // scope is lazy loaded, so need to do something with it for hte exception
            // to throw
            var invalidMock = Scope.BadMock;
        }

        [TestMethod]
        public void NinjectScopeTest_ShouldNotThrowExceptionIfWeDoNotAccessScope
            ()
        {
            Assert.IsTrue(true);
        }
    }
}