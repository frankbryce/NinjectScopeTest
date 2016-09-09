using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Scoper.Test
{
    [TestClass]
    public class BasicTest : AutoScopeTest
    {
        public class Uut
        {
            private readonly ICloneable _clonableDep;

            public Uut(ICloneable clonableDep)
            {
                _clonableDep = clonableDep;
            }

            public void CallClone()
            {
                _clonableDep.Clone();
            }
        }

        [TestMethod]
        public void NinjectScopeTest_ShouldCreateAValidMock()
        {
            Assert.IsNotNull(GetMock<ICloneable>());
            Assert.IsNotNull(Get<ICloneable>());
        }

        [TestMethod]
        public void NinjectScopeTest_ShouldInjectDependencies()
        {
            var uut = Get<Uut>();
            uut.CallClone();
            GetMock<ICloneable>().Verify(x => x.Clone(), Times.Once);
        }
    }
}