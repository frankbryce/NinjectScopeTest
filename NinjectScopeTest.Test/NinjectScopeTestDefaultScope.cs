using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NinjectScopeTest.Test
{
    [TestClass]
    public class NinjectScopeTestDefaultScope : NinjectScopeTest
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
