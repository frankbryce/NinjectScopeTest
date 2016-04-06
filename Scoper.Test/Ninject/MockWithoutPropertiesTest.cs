using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scoper.Test.Ninject
{
    [TestClass]
    public class MockWithoutPropertiesTest : Scoper.Ninject.AutoScopeTest
    {
        public class Uut
        {
            private readonly ICloneable _dep;
            public Uut(ICloneable dep) { _dep = dep; }
            public object DoClone() => _dep.Clone();
        }

        [TestMethod]
        public void SettingUpAMockShouldWorkWithDi()
        {
            Mock<ICloneable>().Setup(x => x.Clone()).Returns("I'm cloned!");
            Assert.AreEqual("I'm cloned!", Get<Uut>().DoClone());
        }
    }
}
