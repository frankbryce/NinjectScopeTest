using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Scoper.Test
{
    [TestClass]
    public class GetWithoutAccessingScopeTest : AutoScopeTest
    {
        public class TestUut
        {
            public TestUut(IComparable comparable)
            {   
            }
        }

        [TestInitialize]
        public void SetupMocks()
        {

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
