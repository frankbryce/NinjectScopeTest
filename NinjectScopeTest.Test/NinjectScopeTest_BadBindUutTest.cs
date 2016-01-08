using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NinjectScopeTest.Exception;

namespace NinjectScopeTest.Test
{
    [TestClass]
    public class NinjectScopeTest_BadBindUutTest :
        NinjectScopeTest<NinjectScopeTest_BadBindUutTest.TestScope>
    {
        public class TestScope : NinjectScope
        {
            public Mock<ICloneable> CloneableMock { get; set; }
            public Mock<ICloneable> CloneableMock2 { get; set; }

            public override void Initialize()
            {
                CloneableMock.Setup(x => x.Clone())
                    .Returns(CloneableMock2);
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestDependencyInjection
        {
            public TestDependencyInjection(
                // ReSharper disable once UnusedParameter.Local
                ICloneable cloneable)
            {
            }
        }

        [TestMethod, ExpectedException(typeof (GetInstanceException))]
        public void NinjectScopeTest_BasicDependencyTest()
        {
            var uut = Get<TestDependencyInjection>();
        }
    }
}