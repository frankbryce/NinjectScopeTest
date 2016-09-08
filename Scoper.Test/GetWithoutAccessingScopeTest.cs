using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Scoper.Test.Ninject
{
    [TestClass]
    public class GetWithoutAccessingScopeTest : Scoper.Ninject.AutoScopeTest<GetWithoutAccessingScopeTest.TestScope>
    {
        public class TestScope : Scoper.Ninject.Scope
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
