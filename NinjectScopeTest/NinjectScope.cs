using Ninject;

namespace NinjectScopeTest
{
    /// <summary>
    ///     This is the base class which is needed to be derived from in order for
    ///     NinjectScopeTest to properly handle automocking the properties on
    ///     the derived Scope object used by the Test cases.  Ninject instantiates
    ///     the object in a Lazy fashion, so that if a particular test does not use
    ///     the Scope object, then there is no overhead in having it a part of
    ///     your derived class.
    /// </summary>
    public abstract class NinjectScope
    {
        public IKernel Kernel { get; set; }

        public abstract void Initialize();
    }
}