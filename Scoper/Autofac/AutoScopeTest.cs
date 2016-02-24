using System;
using Ninject;
using Scoper.Exception;

namespace Scoper.Autofac
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
        protected internal override bool _hasRegistration(Type type)
        {
            return Scope.Kernel.TryGet(type) != null;
        }

        protected override void InitializeDI()
        {
            Scope.Kernel = new StandardKernel(Scope.Settings);
        }

        protected override void RegisterObject(Type type, object obj)
        {
            Scope.Kernel.Bind(type).ToConstant(obj);
        }

        protected internal override object DiContainerGet(Type type)
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