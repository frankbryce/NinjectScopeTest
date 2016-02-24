using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test.Ninject
{
    [TestClass]
    public class DefaultScopeTest : Scoper.Ninject.AutoScopeTest
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
