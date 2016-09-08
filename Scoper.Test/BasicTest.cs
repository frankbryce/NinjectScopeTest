using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scoper.Attribute;

namespace Scoper.Test.Ninject
{
    [TestClass]
    public class BasicTest : Scoper.Ninject.AutoScopeTest<BasicTest.TestScope>
    {
        public class TestScope : Scoper.Ninject.Scope
        {
            public Mock<ICloneable> CloneableMock { get; set; }
            [DoNotBind]
            public Mock<ICloneable> CloneableClonedMock { get; set; }
            public Mock ObjectMock { get; set; }
            public object NonMockObject { get; set; }
            public object InitializedNonMockObject { get; set; }

            public override void Initialize()
            {
                CloneableMock.Setup(x => x.Clone()).Returns(CloneableClonedMock.Object);

                CloneableClonedMock.Setup(x => x.Clone()).Returns(CloneableMock.Object);

                InitializedNonMockObject = new object();
            }
        }

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
            Assert.IsNotNull(Scope.CloneableMock);
            Assert.IsNotNull(Scope.CloneableMock.Object);
        }

        [TestMethod]
        public void NinjectScopeTest_ShouldCreateAValidObjectMock()
        {
            Assert.IsNotNull(Scope.ObjectMock);
            Assert.IsNotNull(Scope.ObjectMock.Object);
        }

        [TestMethod]
        public void NinjectScopeTest_ShouldInitializeTheTestScope()
        {
            var cloneable = Scope.CloneableMock.Object;
            var cloned = cloneable.Clone() as ICloneable;
            Assert.IsNotNull(cloned);

            // made the clone mock return the original mocked object on Clone()
            var cloneOfAClone = cloned.Clone();
            Assert.AreEqual(cloneable, cloneOfAClone);
        }

        [TestMethod]
        public void NinjectScopeTest_ShouldNotInitializeNonMockObjectsImplicitly()
        {
            Assert.IsNull(Scope.NonMockObject);
        }

        [TestMethod]
        public void NinjectScopeTest_ShouldInitializeNonMockObjectsExplicitly()
        {
            Assert.IsNotNull(Scope.InitializedNonMockObject);
        }

        [TestMethod]
        public void InjectedDependencyShouldMatchScopesMock()
        {
            var uut = Get<Uut>();
            uut.CallClone();
            Scope.CloneableMock.Verify(x => x.Clone(), Times.Once);
        }
    }
}