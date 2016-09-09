using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Scoper.Test
{
    [TestClass]
    public class IssuesTests : AutoScopeTest
    {
        [TestMethod]
        public void Issue_000002_GettingASealedClassShouldNotReturnNull()
        {
            var uut = Get<SealedClass>();
            Assert.IsNotNull(uut);
        }
    }

    public sealed class SealedClass
    {
        public SealedClass(ICloneable cloneable) { }
    }
}
