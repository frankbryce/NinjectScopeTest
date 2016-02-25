using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test.Autofac
{
    [TestClass]
    public class AutoScopeTest_AutoResolves : Scoper.Autofac.AutoScopeTest
    {
        public class TestUutInterfaceDep
        {
            public readonly IComparable Dep1;

            public TestUutInterfaceDep(IComparable dep1)
            {
                Dep1 = dep1;
            }
        }

        public class TestUutNoDeps
        {
        }

        public class TestUutMultipleInterfaceDeps
        {
            public readonly IComparable Dep1;
            public readonly ICloneable Dep2;

            public TestUutMultipleInterfaceDeps(IComparable dep1, ICloneable dep2)
            {
                Dep1 = dep1;
                Dep2 = dep2;
            }
        }

        [TestMethod]
        public void AutoResolvesInterfaceDependenciesWithDefaultValue()
        {
            var uut = Get<TestUutInterfaceDep>();
            Assert.IsNotNull(uut);
            Assert.IsNotNull(uut.Dep1);
        }

        [TestMethod]
        public void AutoResolvesMultipleInterfaceDependenciesWithDefaultValue()
        {
            var uut = Get<TestUutMultipleInterfaceDeps>();
            Assert.IsNotNull(uut);
            Assert.IsNotNull(uut.Dep1);
            Assert.IsNotNull(uut.Dep2);
        }

        [TestMethod]
        public void AutoResolvesNoDependencies()
        {
            var uut = Get<TestUutNoDeps>();
            Assert.IsNotNull(uut);
        }
    }
}
