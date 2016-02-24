using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scoper.Exception;

namespace Scoper.Test.Ninject
{
    [TestClass]
    public class BadBindUutTest :
        Scoper.Ninject.AutoScopeTest<BadBindUutTest.TestScope>
    {
        public class TestScope : Scoper.Ninject.Scope
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