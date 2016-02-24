using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Infrastructure;

namespace Scoper.Test.Ninject
{
    [TestClass]
    public class BasicNinjectSettingsTest : Scoper.Ninject.AutoScopeTest<BasicNinjectSettingsTest.TestScope>
    {
        public class TestScope : Scoper.Ninject.Scope
        {

            public override INinjectSettings Settings => new NinjectSettings
            {
                // singleton override behavior so all registered objects end
                // up being singleton
                DefaultScopeCallback = StandardScopeCallbacks.Singleton
            };

            public class TestComparable : IComparable
            {
                public int CompareTo(object obj)
                {
                    return 0;
                }
            }

            public override void Initialize()
            {
            }
        }

        [TestMethod]
        public void NinjectScopeTest_ShouldHaveOverriddenDefaultScopeToSingleton()
        {
            // make up some binding just to test that our Settings overrode the
            // default behavior of the base Scope.
            Scope.Kernel.Bind<IComparable>().To<TestScope.TestComparable>();

            // should return the same exact object
            var obj1 = Get<IComparable>();
            var obj2 = Get<IComparable>();
            Assert.AreSame(obj1, obj2);
        }
    }
    [TestClass]
    public class BasicNinjectSettingsAntiTest : Scoper.Ninject.AutoScopeTest<BasicNinjectSettingsAntiTest.TestScope>
    {
        public class TestScope : Scoper.Ninject.Scope
        {

            public override INinjectSettings Settings => new NinjectSettings
            {
                // singleton override behavior so all registered objects end
                // up being singleton
                DefaultScopeCallback = StandardScopeCallbacks.Transient
            };

            public class TestComparable : IComparable
            {
                public int CompareTo(object obj)
                {
                    return 0;
                }
            }

            public override void Initialize()
            {
            }
        }

        [TestMethod]
        public void NinjectScopeTest_ShouldHaveBehaviorOfTransientScope()
        {
            // make up some binding just to test that our Settings overrode the
            // default behavior of the base Scope.
            Scope.Kernel.Bind<IComparable>().To<TestScope.TestComparable>();

            // should return the same exact object
            var obj1 = Get<IComparable>();
            var obj2 = Get<IComparable>();
            Assert.AreNotSame(obj1, obj2);
        }
    }
}