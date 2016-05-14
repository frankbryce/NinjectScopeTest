using System;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Scoper.Exception;
using Moq;

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

        protected override void DiInitialize(T scope)
        {
            scope.Container = new ContainerBuilder().Build(scope.Options);
        }

        protected override void DiRegister(T scope, Type type, object obj)
        {
            if (obj != null)
            {
                var builder = new ContainerBuilder();
                builder.RegisterInstance(obj).As(type);
                builder.Update(scope.Container, scope.Options);
            }
        }

        protected override object DiGet(Type type)
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