using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scoper.Test
{
    // Derive your Unit test class from AutoScopeTest<T>
    // where T is the type of your Scope object
    [TestClass]
    public class BasicUutTest : AutoScopeTest
    {
        // An example Unit Under Test, used solely for this example.
        // This class can really be anything.
        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestDependencyInjection
        {
            private readonly ICloneable _cloneable;

            // dependencies will be automatically resolved when Get<T>
            // is called on AutoScopeTest.  In this case, T would
            // be TestDependencyInjection
            public TestDependencyInjection(
                ICloneable cloneable,
                // unused dependency, but here just for show
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
            // this is all you need in your test to get you
            // auto-resolved mocked dependencies for your
            // unit under test.
            var uut = Get<TestDependencyInjection>();

            var obj = new object();
            GetMock<ICloneable>().Setup(x => x.Clone()).Returns(obj);

            // perform action on your UUT
            var cloned = uut.CloneWrapper();

            // assert that results were as expected
            Assert.AreEqual(obj, cloned);
        }
    }
}