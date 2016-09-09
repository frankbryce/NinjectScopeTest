using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scoper.Exception;

namespace Scoper.Test
{
    [TestClass]
    public class MockBadTypesTest :  AutoScopeTest
    {
        [TestMethod, ExpectedException(typeof(GetInstanceException))]
        public void NinjectScopeTest_ShouldThrowInvalidMockException()
        {
            GetMock<string>();
        }
    }
}