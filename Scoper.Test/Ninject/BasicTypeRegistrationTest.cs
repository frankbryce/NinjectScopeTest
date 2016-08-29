using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test.Ninject
{
    [TestClass]
    public class BasicTypeRegistrationTest : Scoper.Ninject.AutoScopeTest<BasicRegisterScope>
    {
        [TestMethod]
        public void CanGetDependencyWithNoConstructorArguments()
        {
            Assert.IsNotNull(Get<ICloneable>());
            Assert.AreEqual(Get<ICloneable>().GetType(), typeof(BasicClonable));
        }

        [TestMethod]
        public void CanGetDependencyWithNoScopeRegistration()
        {
            Assert.IsNotNull(Get<IComparable>());
        }
    }

    public class BasicRegisterScope : Scoper.Ninject.Scope
    {
        public override void Initialize()
        {
            Kernel.Bind<ICloneable>().To<BasicClonable>();
        }
    }

    public class BasicClonable : ICloneable
    {
        public object Clone()
        {
            return new BasicClonable();
        }
    }
}
