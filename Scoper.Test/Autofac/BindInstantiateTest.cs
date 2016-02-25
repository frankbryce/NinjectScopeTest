using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scoper.Attribute;

namespace Scoper.Test.Autofac
{
    [TestClass]
    public class BindInstantiateTest :
        Scoper.Autofac.AutoScopeTest<BindInstantiateTest.TestScope>
    {
        public class TestScope : Scoper.Autofac.Scope
        {
            [DoNotInstantiate, DoNotBind]
            public Mock<ICloneable> NoInstNoBind { get; set; }

            [DoInstantiate, DoNotBind]
            public Mock<IComparable> InstNoBind { get; set; }

            [DoNotInstantiate, DoBind]
            public Mock<ICloneable> NoInstBind { get; set; }

            [DoInstantiate, DoBind]
            public Mock<IComparable> InstBind { get; set; }

            public override void Initialize()
            {
            }
        }

        private class TestDependencyInjection
        {
            private readonly ICloneable _clonable;
            private readonly IComparable _comparable;

            // ReSharper disable once MemberCanBePrivate.Local
            public TestDependencyInjection(
                ICloneable clonable,
                IComparable comparable = null)
            {
                _clonable = clonable;
                _comparable = comparable;
            }

            // ReSharper disable once MemberCanBePrivate.Local
            public TestDependencyInjection(
                IComparable comparable) : this(null, comparable)
            {
            }

            // ReSharper disable once UnusedMember.Local
            public TestDependencyInjection() : this(null)
            {
            }

            public ICloneable Cloneable => _clonable;
            public IComparable Comparable => _comparable;
        }

        [TestMethod]
        public void NinjectScopeTest_BasicNoInstantiateNoBindTest()
        {
            Assert.IsNull(Scope.NoInstNoBind);
            var uut = Get<TestDependencyInjection>();
        }

        [TestMethod]
        public void NinjectScopeTest_BasicInstantiateNoBindTest()
        {
            Assert.IsNotNull(Scope.InstNoBind);
            var uut = Get<TestDependencyInjection>();
            Assert.AreNotEqual((object) uut.Comparable, Scope.InstNoBind.Object);
        }

        [TestMethod]
        public void NinjectScopeTest_BasicNoInstantiateBindTest()
        {
            Assert.IsNull(Scope.NoInstBind);
            var uut = Get<TestDependencyInjection>();
            Assert.AreEqual((object) uut.Cloneable, null);
        }

        [TestMethod]
        public void NinjectScopeTest_BasicInstantiateBindTest()
        {
            Assert.IsNotNull(Scope.InstBind);
            var uut = Get<TestDependencyInjection>();
            Assert.AreEqual((object) uut.Comparable, Scope.InstBind.Object);
        }
    }
}