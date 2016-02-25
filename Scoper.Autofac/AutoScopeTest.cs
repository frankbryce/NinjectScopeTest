using System;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
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
        protected override bool _hasRegistration(Type type)
        {
            var obj = DefaultValue.Get(type);
            Scope.Container.TryResolve(type, out obj);
            return obj != DefaultValue.Get(type);
        }

        protected override void InitializeDI()
        {
            Scope.Container = new ContainerBuilder().Build(Scope.Options);
        }

        protected override void RegisterObject(Type type, object obj)
        {
            if (obj != null)
            {
                var builder = new ContainerBuilder();
                builder.RegisterInstance(obj).As(type);
                builder.Update(Scope.Container, Scope.Options);
            }
        }

        protected override object DiContainerGet(Type type)
        {
            try
            {
                if (!_hasRegistration(type))
                {
                    var builder = new ContainerBuilder();
                    builder.RegisterType(type).As(type);
                    builder.Update(Scope.Container, Scope.Options);
                }
                return Scope.Container.Resolve(type);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new GetInstanceException(ex);
            }
            catch (DependencyResolutionException ex)
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