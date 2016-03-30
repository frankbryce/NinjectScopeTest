using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Scoper.Test.ScopeTest
{
    [TestClass]
    public class BasicScopeTest : Scoper.ScopeTest
    {
        [TestMethod]
        public void AssertNoRegistrationsCausesNoErrors()
        {
            Assert.AreEqual(default(string), Get<string>());
            Assert.AreEqual(default(IComparable), Get<IComparable>());
            Assert.AreEqual(default(int), Get<int>());
        }
    }

    public class BasicDerivedScopeTestScope : Scope
    {
        public Mock<IComparable> comparableMock { get; set; }
        public int comparableResult { get { return 123; } }

        public override void Initialize()
        {
            comparableMock.Setup(x => x.CompareTo(It.IsAny<object>())).Returns(comparableResult);
        }
    }
}
