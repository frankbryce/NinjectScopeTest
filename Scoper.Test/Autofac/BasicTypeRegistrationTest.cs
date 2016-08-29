using System;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test.Autofac
{
    [TestClass]
    public class BasicTypeRegistrationTest : Scoper.Autofac.AutoScopeTest<BasicRegisterScope>
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

    public class BasicRegisterScope : Scoper.Autofac.Scope
    {
        public override void Initialize()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new BasicClonable()).As(typeof(ICloneable));
            builder.Update(Container, Options);
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
