using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test.Ninject
{
    [TestClass]
    public class AutoScopeTest_AutoResolves : Scoper.Ninject.AutoScopeTest
    {
        // ReSharper disable ClassNeverInstantiated.Local
        public class TestUutSealedDep
        {
            public readonly string Dep1;

            public TestUutSealedDep(string dep1)
            {
                Dep1 = dep1;
            }
        }

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

        public class TestUutSealedAndInterfaceDeps
        {
            public readonly IComparable Dep1;
            public readonly int Dep4;
            public readonly ICloneable Dep2;
            public readonly string Dep3;

            public TestUutSealedAndInterfaceDeps(IComparable dep1, int dep4, ICloneable dep2, string dep3)
            {
                Dep1 = dep1;
                Dep2 = dep2;
                Dep3 = dep3;
                Dep4 = dep4;
            }
        }

        public class TestUutNonSealedDeps
        {
            public readonly TestUutSealedDep Dep1;
            public readonly TestUutNoDeps Dep2;

            public TestUutNonSealedDeps(TestUutSealedDep dep1, TestUutNoDeps dep2)
            {
                Dep1 = dep1;
                Dep2 = dep2;
            }
        }

        [TestMethod]
        public void AutoResolvesSealedDependenciesWithDefaultValue()
        {
            var uut = Get<TestUutSealedDep>();
            Assert.IsNotNull(uut);
            Assert.AreEqual(default(string), uut.Dep1);
        }

        [TestMethod]
        public void AutoResolvesInterfaceDependenciesWithDefaultValue()
        {
            var uut = Get<TestUutInterfaceDep>();
            Assert.IsNotNull(uut);
            Assert.IsNotNull(uut.Dep1);
        }

        [TestMethod]
        public void AutoResolvesNoDependencies()
        {
            var uut = Get<TestUutNoDeps>();
            Assert.IsNotNull(uut);
        }

        [TestMethod]
        public void AutoResolvesNonSealedDeps()
        {
            var uut = Get<TestUutNonSealedDeps>();
            Assert.IsNotNull(uut);
            Assert.IsNull(uut.Dep1);
            Assert.IsNotNull(uut.Dep2);
        }

        [TestMethod]
        public void AutoResolvesSealedAndInterfaceDeps()
        {
            var uut = Get<TestUutSealedAndInterfaceDeps>();
            Assert.IsNotNull(uut);
            Assert.IsNotNull(uut.Dep1);
            Assert.IsNotNull(uut.Dep2);
            Assert.AreEqual(default(string), uut.Dep3);
            Assert.AreEqual(default(int), uut.Dep4);
        }
    }
}
