using Ninject;

namespace NinjectScopeTest
{
    public abstract class NinjectScope
    {
        public IKernel Kernel { get; set; }

        public abstract void Initialize();
    }
}