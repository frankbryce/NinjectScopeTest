using System;

namespace Scoper
{
    /// <summary>
    /// Base Scope class with minimal functionality.  Useful if the only
    /// feature desired is for the initizilze method to be called once
    /// per test class.  By contrast [TestInitialize] methods are called
    /// once per test method.
    /// </summary>
    public class Scope : IDisposable
    {
        /// <summary>
        /// Overridable Initialize() method that is called the first time that
        /// the instance of this object is accessed.  It's like a TestInitialize()
        /// call but test framework agnostic, and lazy loaded so that not all tests
        /// need to execute this method if the test doesn't require the Test Scope.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}