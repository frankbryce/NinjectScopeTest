using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test
{
    [TestClass]
    public class DisposeAutoScopeTest : AutoScopeTest
    {
        [TestInitialize]
        public void SetupMocks()
        {

        }

        [TestMethod]
        public void ScopeShouldBeDisposed()
        {
            Dispose();
            Assert.IsTrue(IsDisposed);
        }
    }
}
