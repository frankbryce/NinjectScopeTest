using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test
{
    [TestClass]
    public class DefaultScopeTest : AutoScopeTest
    {
        private class TestUut
        {
        }

        [TestMethod]
        public void DefaultScopeWillStillProvideInstanceOfSimpleUut()
        {
            var uut = Get<TestUut>();
            Assert.IsNotNull(uut);
        }
    }
}
