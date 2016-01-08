# Samples

This repo contains a NuGet package to help writing unit tests
with Moq against code which uses Ninject DI.

## Simple Example
```csharp
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace NinjectScopeTest.Test
{
    // An example Unit Under Test, used solely for this example.
    // This class can really be anything.
    internal class TestDependencyInjection
    {
        private readonly ICloneable _cloneable;

        // dependencies will be automatically resolved when Get<T>
        // is called on NinjectScopeTest.  In this case, T would
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

    // Derive your Unit test class from NinjectScopeTest<T>
    // where T is the type of your Scope object
    [TestClass]
    public class NinjectScopeTest_BasicUutTest :
        NinjectScopeTest<NinjectScopeTest_BasicUutTest.TestScope>
    {
        // create your own scope object, which is used to 
        // hold the data that your test class needs
        // to perform the test.  Any properties of type Mock<T>
        // where T is an interface or a non-sealed class
        // will be automatically instantiated and bound
        // to the Ninject Kernel on NinjectScope
        public class TestScope : NinjectScope
        {
            public Mock<ICloneable> CloneableMock { get; set; }
            public object ClonedObject { get; set; }
            public Mock<IComparable> ComparableMock { get; set; }

            // manditory Initialize method to do any
            // custom setup that you'd like to perform.
            public override void Initialize()
            {
                ClonedObject = new object();
                CloneableMock.Setup(x => x.Clone())
                    .Returns(ClonedObject);
            }
        }

        [TestMethod]
        public void NinjectScopeTest_BasicDependencyTest()
        {
            // this is all you need in your test to get you
            // auto-resolved mocked dependencies for your
            // unit under test.
            var uut = Get<TestDependencyInjection>();

            // perform action on your UUT
            var cloned = uut.CloneWrapper();

            // assert that results were as expected
            Assert.AreEqual(Scope.ClonedObject, cloned);
        }
    }
}
```

## DoNotBind Attribute

If you would not like to bind one of your mocked properties on your
Scope object to the Ninject Kernel, then you may specify this intention
with the [DoNotBind] Attribute.  This can be useful if you are creating a
mock solely for returning from another mocked call, or if you have
two mocked properties of the same type, and only one of them should
be used to resolve your UUT dependencies.

```csharp
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NinjectScopeTest.Attribute;

namespace NinjectScopeTest.Test
{
    // An example Unit Under Test, used solely for this example.
    // This class can really be anything.
    internal class TestDependencyInjection
    {
        private readonly ICloneable _cloneable;

        // dependencies will be automatically resolved when Get<T>
        // is called on NinjectScopeTest.  In this case, T would
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

    [TestClass]
    public class NinjectScopeTest_NoBindUutTest :
        NinjectScopeTest<NinjectScopeTest_NoBindUutTest.TestScope>
    {
        public class TestScope : NinjectScope
        {
            // [DoBind] is superfluous, because it describes the default
            // behavior of the Scope.Kernel object being bound with
            // the CloneableMock.Object object.
            [DoBind]
            public Mock<ICloneable> CloneableMock { get; set; }

            // [DoNotBind] specifies to NinjectScopeTest NOT to bind the
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
            Assert.AreEqual(Scope.CloneableClonedMock.Object, cloned);
        }
    }
}
```
## DoNotInstantiate Attribute

There is also an analagous attribute for you properties which you'd like to have
as type Mock<T>, but would not like NinjectScopeTest to instantiate it for you.

As a side note, *if you specify [DoNotInstantiate, DoBind] on an object, Ninject*
*will bind a null value to your mock of that type.*  If you would like to avoid
this behavior, then explicitely specify [DoNotInstantiate, DoNotBind] to your Mock<T>
property.

## Feedback

If you have any feedback about this package, please feel free to contact me
at john.carpenter1346@gmail.com
