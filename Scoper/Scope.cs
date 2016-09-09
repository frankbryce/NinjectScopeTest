using System;
using Ninject;
using Ninject.MockingKernel.Moq;

namespace Scoper
{
    /// <summary>
    /// Base Scope class with minimal functionality.  Useful if the only
    /// feature desired is for the initizilze method to be called once
    /// per test class.  By contrast [TestInitialize] methods are called
    /// once per test method.
    /// </summary>
    public sealed class Scope : IDisposable
    {
        public Scope()
        {
            Kernel = new MoqMockingKernel(Settings);
        }

        public MoqMockingKernel Kernel { get; set; }

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
        public INinjectSettings Settings => new NinjectSettings
        {
            AllowNullInjection = true
        };

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Kernel?.Dispose();
        }
    }
}