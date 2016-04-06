using System;
using Ninject;
using Scoper.Exception;
using Moq;

namespace Scoper.Ninject
{
    /// <summary>
    /// Default Scoper.Ninject.AutoScopeTest uses the default Scoper.Ninject.Scope for minimal setup for 
    /// the simplest tests.
    /// </summary>
    public abstract class AutoScopeTest : AutoScopeTest<Scope>
    {
    }

    public abstract class AutoScopeTest<T> : Scoper.AutoScopeTest<T> where T : Scope, new()
    {
        protected override bool _hasRegistration(Type type)
        {
            return Scope.Kernel.TryGet(type) != null;
        }

        protected override void DiInitialize()
        {
            Scope.Kernel = new StandardKernel(Scope.Settings);
        }

        protected override void DiRegister(Type type, object obj)
        {
            Scope.Kernel.Bind(type).ToConstant(obj);
        }

        protected override object DiGet(Type type)
        {
            try
            {
                return Scope.Kernel.Get(type);
            }
            catch (ActivationException ex)
            {
                throw new GetInstanceException(ex);
            }
            catch (System.Exception ex)
            {
                throw new InternalException(ex);
            }
        }
    }
}