using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NinjectScopeTest.Attribute;

namespace NinjectScopeTest.Test
{
    [TestClass]
    public class NinjectScopeTest_NoBindUutTest :
        NinjectScopeTest<NinjectScopeTest_NoBindUutTest.TestScope>
    {
        public class TestScope : NinjectScope
        {
            [DoBind]
            public Mock<ICloneable> CloneableMock { get; set; }

            [DoNotBind]
            public Mock<ICloneable> CloneableClonedMock { get; set; }

            public override void Initialize()
            {
                CloneableMock.Setup(x => x.Clone())
                    .Returns(CloneableClonedMock.Object);

                // it's the ciiiiiiircle of liiiiiiiife
                CloneableClonedMock.Setup(x => x.Clone())
                    .Returns(CloneableMock.Object);
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestDependencyInjection
        {
            private readonly ICloneable _cloneable;

            public TestDependencyInjection(ICloneable cloneable)
            {
                _cloneable = cloneable;
            }

            public ICloneable CloneWrapper()
            {
                return _cloneable.Clone() as ICloneable;
            }
        }

        [TestMethod]
        public void NinjectScopeTest_BasicDependencyTest()
        {
            var uut = Get<TestDependencyInjection>();
            var cloned = uut.CloneWrapper();
            Assert.AreEqual(Scope.CloneableClonedMock.Object, cloned);
        }
    }
}