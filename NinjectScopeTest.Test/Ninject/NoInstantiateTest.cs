using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scoper.Attribute;

namespace Scoper.Test.Ninject
{
    [TestClass]
    public class NoInstantiateTest :
        Scoper.Ninject.AutoScopeTest<NoInstantiateTest.TestScope>
    {
        public class TestScope : Scoper.Ninject.Scope
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