using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Scoper.Test.ScopeTest
{
    public class AutoRegisterGenericMocksScope : Scoper.Ninject.Scope
    {
        public Mock<ITestDep<ICloneable>> GenericMock { get; set; }
    }

    public interface ITestDep<T>
    {
        object DoCompare(object obj);
    }

    public class TestUut<T>
    {
        private readonly IComparable<T> _depComparable;
        private readonly ITestDep<T> _dep;

        public TestUut(
            IComparable<T> depComparable,
            ITestDep<T> dep)
        {
            _depComparable = depComparable;
            _dep = dep;
        }

        public object DoCompare(object obj)
        {
            try
            {
                return _dep.DoCompare(obj);
            }
            catch (System.Exception)
            {
                throw new TestException();
            }
        }

        public class TestException : System.Exception
        {
        }
    }

    [TestClass]
    public class AutoRegisterGenericMocksTest : Scoper.Ninject.AutoScopeTest<AutoRegisterGenericMocksScope>
    {
        /// <summary>
        /// This test was created for the testing of https://github.com/frankbryce/Scoper/issues/1
        /// </summary>
        [TestMethod]
        public void MockShouldBeRegisteredWithDi()
        {
            Assert.AreEqual(Scope.GenericMock.Object,
                DiGet(typeof (ITestDep<>).MakeGenericType(typeof (ICloneable))));
        }

        /// <summary>
        /// This test was created for the testing of https://github.com/frankbryce/Scoper/issues/1
        /// </summary>
        [TestMethod, ExpectedException(typeof(TestUut<ICloneable>.TestException))]
        public void MockShouldBeInjectedWhenGettingUut()
        {
            Scope.GenericMock.Setup(x => x.DoCompare(It.IsAny<object>())).Throws(new System.Exception());
            var uut = Get<TestUut<ICloneable>>();
            uut.DoCompare(null);
        }
    }
}
