using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NinjectScopeTest.Attribute;

namespace NinjectScopeTest.Test
{
    [TestClass]
    public class NinjectScopeTest_NoInstantiateTest :
        NinjectScopeTest<NinjectScopeTest_NoInstantiateTest.TestScope>
    {
        public class TestScope : NinjectScope
        {
            [DoNotInstantiate]
            public Mock<ICloneable> CloneableMockNull { get; set; }

            [DoInstantiate]
            public Mock<ICloneable> CloneableMockNotNull { get; set; }

            public override void Initialize()
            {
            }
        }

        [TestMethod]
        public void NinjectScopeTest_BasicDoNotInstantiateTest()
        {
            Assert.IsNull(Scope.CloneableMockNull);
        }

        [TestMethod]
        public void NinjectScopeTest_BasicDoInstantiateTest()
        {
            Assert.IsNotNull(Scope.CloneableMockNotNull);
        }
    }
}