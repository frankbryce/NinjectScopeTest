using System;

namespace Scoper.Test.Ninject
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Scoper.Ninject;

    public class MockWithPropertiesTestScope : Scope
    {
        public Mock<ICloneable> Cloneable { get; set; }
    }

    [TestClass]
    public class MockWithPropertiesTest : AutoScopeTest<MockWithPropertiesTestScope>
    {
        [TestMethod]
        public void MockOnScopeShouldBeTheSameAsOnAutoScopeTestMockMethod()
        {
            Assert.AreSame(Scope.Cloneable, Mock<ICloneable>());
        }
    }
}
