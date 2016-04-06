using System;

namespace Scoper.Test.Autofac
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Scoper.Autofac;

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
            var scopeObj = Scope.Cloneable;
            Assert.AreSame(scopeObj, Mock<ICloneable>());
        }
    }
}
