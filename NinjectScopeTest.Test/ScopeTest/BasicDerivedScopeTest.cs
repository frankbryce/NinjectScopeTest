using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test.ScopeTest
{
    [TestClass]
    public class BasicDerivedScopeTest : ScopeTest<BasicDerivedScopeTestScope>
    {
        [TestMethod]
        public void AssertNoRegistrationsCausesNoErrors()
        {
            Assert.AreEqual(default(string), Get<string>());
            Assert.AreEqual(default(IComparable), Get<IComparable>());
            Assert.AreEqual(default(int), Get<int>());
        }

        [TestMethod]
        public void AssertMockOnScopeIsCreated()
        {
            Assert.IsNotNull(Scope.comparableMock);
        }

        [TestMethod]
        public void AssertInitializeOnScopeIsCalled()
        {
            Assert.AreEqual(Scope.comparableResult, Scope.comparableMock.Object.CompareTo(null));
        }
    }
}