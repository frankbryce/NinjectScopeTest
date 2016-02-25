using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scoper.Attribute;

namespace Scoper.Test.Autofac
{
    [TestClass]
    public class NoBindUutTest :
        Scoper.Autofac.AutoScopeTest<NoBindUutTest.TestScope>
    {
        public class TestScope : Scoper.Autofac.Scope
        {
            // [DoBind] is superfluous, because it describes the default
            // behavior of the Scope.Kernel object being bound with
            // the CloneableMock.Object object.
            [DoBind]
            public Mock<ICloneable> CloneableMock { get; set; }

            // [DoNotBind] specifies to Scoper.Ninject.AutoScopeTest NOT to bind the
            // mocked object to Scope.Kernel.  In this case, this is useful
            // in order to specify that because Ninject would not know which
            // ICloneable to use to resolve the dependency for our UUT
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

        // An example Unit Under Test, used solely for this example.
        // This class can really be anything.
        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestDependencyInjection
        {
            private readonly ICloneable _cloneable;

            // dependencies will be automatically resolved when Get<T>
            // is called on Scoper.Ninject.AutoScopeTest.  In this case, T would
            // be TestDependencyInjection
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
            // still we get our UUT in the same way as above
            var uut = Get<TestDependencyInjection>();

            // get our cloned object, setup in our Initialize() method in TestScope
            var cloned = uut.CloneWrapper();

            // The mocked object we have in scope is still useful!  It just
            // didn't interfere with the Scope.Kernel in resolving the depdencies for
            // our UUT
            Assert.AreEqual((object) Scope.CloneableClonedMock.Object, cloned);
        }
    }
}