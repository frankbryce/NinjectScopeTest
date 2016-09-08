using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test.ScopeTest
{
    public class TestDisposeScope : Scope
    {
        public bool IsDisposed { get; private set; } = false;
        public override void Dispose()
        {
            IsDisposed = true;
            base.Dispose();
        }
    }

    [TestClass]
    public class DisposeAutoScopeTest : AutoScopeTest<TestDisposeScope>
    {
        [TestMethod]
        public void ScopeShouldBeDisposed()
        {
            Dispose();
            Assert.IsTrue(Scope.IsDisposed);
        }
    }

    [TestClass]
    public class DisposeScopeTest : ScopeTest<TestDisposeScope>
    {
        [TestMethod]
        public void ScopeShouldBeDisposed()
        {
            Dispose();
            Assert.IsTrue(Scope.IsDisposed);
        }
    }
}
