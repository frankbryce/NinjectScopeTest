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
    public class NinjectScope
    {
        public StandardKernel Kernel { get; set; }

        /// <summary>
        /// Provide the ability of the derived scope to override the default
        /// settings that are used for the StandardKernel.  Simply override
        /// this property in your test scope and provide the settings that
        /// you would like to use.  The default settings is
        /// 
        /// new NinjectSettings {
        ///     AllowNullInjection = true
        /// };
        /// </summary>
        public virtual INinjectSettings Settings => new NinjectSettings {AllowNullInjection = true};

        /// <summary>
        /// Overridable Initialize() method that is called the first time that
        /// the instance of this object is accessed.  It's like a TestInitialize()
        /// call but test framework agnostic, and lazy loaded so that not all tests
        /// need to execute this method if the test doesn't require the Test Scope.
        /// </summary>
        public virtual void Initialize()
        {
        }
    }
}