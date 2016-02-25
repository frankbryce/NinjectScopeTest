using Autofac;
using Autofac.Builder;

namespace Scoper.Autofac
{
    /// <summary>
    /// Scoper.Ninject.AutoScopeTest properly handles automocking the properties on
    /// the Scope object used by the Test cases.  Ninject instantiates
    /// the object in a Lazy fashion, so that if a particular test does not use
    /// the Scope object, then there is no overhead in having it a part of
    /// your derived class.
    /// </summary>
    public class Scope : Scoper.Scope
    {
        /// <summary>
        /// Autofac container which holds the result of the registrations
        /// </summary>
        public IContainer Container { get; set; }

        /// <summary>
        /// Provide the ability of the derived scope to override the default
        /// options that are used for the ContainerBuilder.Build Method.  Override
        /// this property in your test scope and provide the settings that
        /// you would like to use.  The default settings is
        /// 
        /// ContainerBuildOptions.None;
        /// </summary>
        public virtual ContainerBuildOptions Options => ContainerBuildOptions.None;
    }
}