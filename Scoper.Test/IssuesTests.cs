using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Scoper.Test.Ninject
{
    [TestClass]
    public class IssuesTests : Scoper.Ninject.AutoScopeTest<IssuesTestsScope>
    {
        [TestMethod]
        public void Issue_000002_GettingASealedClassShouldNotReturnNull()
        {
            var uut = Get<SealedClass>();
            Assert.IsNotNull(uut);
        }
    }

    public class IssuesTestsScope : Scoper.Ninject.Scope
    {
        public Mock<ICloneable> Clonable { get; set; }
    }

    public sealed class SealedClass
    {
        public SealedClass(ICloneable cloneable) { }
    }
}
