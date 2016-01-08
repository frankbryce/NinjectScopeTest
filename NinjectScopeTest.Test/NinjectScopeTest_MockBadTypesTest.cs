using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NinjectScopeTest.Exception;

namespace NinjectScopeTest.Test
{
    [TestClass]
    public class NinjectScopeTest_MockBadTypesTest :
        NinjectScopeTest<NinjectScopeTest_MockBadTypesTest.TestScope>
    {
        public class TestScope : NinjectScope
        {
            public Mock GoodMock { get; set; }
            public Mock<string> BadMock { get; set; }

            public override void Initialize()
            {
            }
        }

        [TestMethod, ExpectedException(typeof (InvalidMockException))]
        public void NinjectScopeTest_ShouldThrowInvalidMockException()
        {
            // scope is lazy loaded, so need to do something with it for hte exception
            // to throw
            var invalidMock = Scope.BadMock;
        }

        [TestMethod]
        public void NinjectScopeTest_ShouldNotThrowExceptionIfWeDoNotAccessScope()
        {
            Assert.IsTrue(true);
        }
    }
}