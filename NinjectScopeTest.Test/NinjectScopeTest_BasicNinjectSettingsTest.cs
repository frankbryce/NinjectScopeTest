using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using NinjectScopeTest.Attribute;
using Ninject.Infrastructure;
using NinjectScopeTest.Exception;

namespace NinjectScopeTest.Test
{
    [TestClass]
    public class NinjectScopeTest_BasicNinjectSettingsTest : NinjectScopeTest<NinjectScopeTest_BasicNinjectSettingsTest.TestScope>
    {
        public class TestScope : NinjectScope
        {
            // because we do no instantiate the mock, and we have a custom
            // NinjectSettings which doesn't allow null injection, then no
            // item will be bound for this scope
            [DoNotInstantiate]
            public Mock<ICloneable> WillNotBeBound { get; set; }

            public override INinjectSettings Settings => new NinjectSettings
            {
                // make this false so that not instantiating the object will
                // throw an exception if we try to get the mocked object from Ninject
                AllowNullInjection = false,
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
        public void NinjectScopeTest_ShouldNotBindANullValue()
        {
            try {
                var cloneable = Get<ICloneable>();
                Assert.Fail("The registration should fail because we don't allow null bindings");
            }
            catch(GetInstanceException e)
            {
                // good, exception should be thrown
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
}