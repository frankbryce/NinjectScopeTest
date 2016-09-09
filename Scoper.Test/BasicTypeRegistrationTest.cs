using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test
{
    [TestClass]
    public class BasicTypeRegistrationTest : AutoScopeTest
    {
        [TestInitialize]
        public void SetupMocks() => Bind<ICloneable>().To<BasicClonable>();

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

    public class BasicClonable : ICloneable
    {
        public object Clone()
        {
            return new BasicClonable();
        }
    }
}
