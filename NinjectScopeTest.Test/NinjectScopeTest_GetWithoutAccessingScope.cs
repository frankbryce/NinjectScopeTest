using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace NinjectScopeTest.Test
{
    [TestClass]
    public class NinjectScopeTest_GetWithoutAccessingScope : NinjectScopeTest<NinjectScopeTest_GetWithoutAccessingScope.TestScope>
    {
        public class TestScope : NinjectScope
        {
            public Mock<IComparable> ComparableDependency { get; set; }
        }

        public class TestUut
        {
            public TestUut(IComparable comparable)
            {   
            }
        }

        [TestMethod]
        public void TestUutGetWithDependenciesBeforeAccessingScope()
        {
            var uut = Get<TestUut>();
            Assert.IsNotNull(uut);
        }

        /// <summary>
        /// Feature added in version 0.3.2
        /// </summary>
        [TestMethod]
        public void TestUutGetWithoutInitializeMethod()
        {
            var uut = Get<TestUut>();
            Assert.IsNotNull(uut);
        }
    }
}
