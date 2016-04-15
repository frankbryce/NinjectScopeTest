using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test.Autofac
{
    [TestClass]
    public class IssuesTests : Scoper.Autofac.AutoScopeTest<EmptyScope>
    {
        [TestMethod]
        public void Issue_000002_GettingASealedClassShouldNotReturnNull()
        {
            var uut = Get<EmptySealedClass>();
            Assert.IsNotNull(uut);
        }
    }

    public class EmptyScope : Scoper.Autofac.Scope { }

    public sealed class EmptySealedClass { }
}
