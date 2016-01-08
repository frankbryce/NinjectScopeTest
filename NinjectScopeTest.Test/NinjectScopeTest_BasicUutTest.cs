using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace NinjectScopeTest.Test
{
    [TestClass]
    public class NinjectScopeTest_BasicUutTest :
        NinjectScopeTest<NinjectScopeTest_BasicUutTest.TestScope>
    {
        public class TestScope : NinjectScope
        {
            public Mock<ICloneable> CloneableMock { get; set; }
            public object ClonedObject { get; set; }
            public Mock<IComparable> ComparableMock { get; set; }

            public override void Initialize()
            {
                ClonedObject = new object();
                CloneableMock.Setup(x => x.Clone())
                    .Returns(ClonedObject);
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestDependencyInjection
        {
            private readonly ICloneable _cloneable;

            public TestDependencyInjection(
                ICloneable cloneable,
                // ReSharper disable once UnusedParameter.Local
                IComparable unusedComparable)
            {
                _cloneable = cloneable;
            }

            public object CloneWrapper()
            {
                return _cloneable.Clone();
            }
        }

        [TestMethod]
        public void NinjectScopeTest_BasicDependencyTest()
        {
            var uut = Get<TestDependencyInjection>();
            var cloned = uut.CloneWrapper();
            Assert.AreEqual(Scope.ClonedObject, cloned);
        }
    }
}