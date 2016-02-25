using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test.Autofac
{
    [TestClass]
    public class DefaultScopeTest : Scoper.Autofac.AutoScopeTest
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
