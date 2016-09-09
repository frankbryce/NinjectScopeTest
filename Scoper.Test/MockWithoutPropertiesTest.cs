using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test
{
    [TestClass]
    public class MockWithoutPropertiesTest : AutoScopeTest
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
            GetMock<ICloneable>().Setup(x => x.Clone()).Returns("I'm cloned!");
            Assert.AreEqual("I'm cloned!", Get<Uut>().DoClone());
        }
    }
}
