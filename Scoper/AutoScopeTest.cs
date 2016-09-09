using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Moq;
using Ninject;
using Ninject.Activation;
using Ninject.Activation.Blocks;
using Ninject.Components;
using Ninject.MockingKernel.Moq;
using Ninject.Modules;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Syntax;
using Scoper.Exception;

namespace Scoper
{
    /// <summary>
    /// This is a base type, use one of the derived DI AutoScopeTest objects.
    /// This class by itself will perform injection of default values for types,
    /// and will not return any meaningful values for your dependencies.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public abstract class AutoScopeTest : IKernel
    {
        protected static readonly ILog Logger =
            LogManager.GetLogger(typeof(AutoScopeTest));
        private Scope _scope;

        private Scope Scope
        {
            get
            {
                if (_scope == null)
                {
                    try
                    {
                        _scope = new Scope();
                    }
                    catch (System.Exception ex)
                    {
                        throw new InternalException(ex);
                    }
                }
                return _scope;
            }
        }

        public void Dispose()
        {
            Scope?.Dispose();
        }

        /// <summary>
        /// This will create a Mock{U} object and register with the backing
        /// DI container to inject into classes which depend on type U
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        protected Mock<U> GetMock<U>() where U : class
        {
            try
            {
                return Scope.Kernel.GetMock<U>();
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

        /// <summary>
        /// This will attempt to get an instance of type type via the kernel as it is
        /// setup manually.  On a failure, then NinjectAutoScopeTest will register
        /// all dependencies of all constructors of type and return IKernel.Get<U>().
        /// For each dependency which is not already registered with the Ninject
        /// kernel, Ninject will bind it with a Moq.Mock.Object reference if
        /// U is a non-sealed type.  If type is a class without a default constructor,
        /// or a value type, then we register a default value for that type.
        /// All registrations of previously unbound types are done as singleton
        /// constant values; using Get(type) multiple times will inject the same
        /// dependencies each time.
        /// </summary>
        /// <returns>An instance of an object of type type</returns>
        protected object Get(Type type)
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

        /// <summary>
        /// This will attempt to get an instance of type U via the kernel as it is
        /// setup manually.  On a failure, then NinjectAutoScopeTest will register
        /// all dependencies of all constructors of type and return IKernel.Get<U>().
        /// For each dependency which is not already registered with the Ninject
        /// kernel, Ninject will bind it with a Moq.Mock.Object reference if
        /// U is a non-sealed type.  If U is a class without a default constructor,
        /// or a value type, then we register a default value for that type.
        /// All registrations of previously unbound types are done as singleton
        /// constant values; using Get{U}() multiple times will inject the same
        /// dependencies each time for the same U
        /// </summary>
        /// <returns>An instance of an object of type U</returns>
        protected U Get<U>() => (U) Get(typeof (U));

        public IBindingToSyntax<T1> Bind<T1>() => Scope?.Kernel?.Bind<T1>();
        public IBindingToSyntax<T1, T2> Bind<T1, T2>() => Scope?.Kernel?.Bind<T1, T2>();
        public IBindingToSyntax<T1, T2, T3> Bind<T1, T2, T3>() => Scope?.Kernel?.Bind<T1, T2, T3>();
        public IBindingToSyntax<T1, T2, T3, T4> Bind<T1, T2, T3, T4>() => Scope?.Kernel?.Bind<T1, T2, T3, T4>();
        public IBindingToSyntax<object> Bind(params Type[] services) => Scope?.Kernel?.Bind(services);
        public void Unbind<T1>() => Scope?.Kernel?.Unbind<T1>();
        public void Unbind(Type service) => Scope?.Kernel?.Unbind(service);
        public IBindingToSyntax<T1> Rebind<T1>() => Scope?.Kernel?.Rebind<T1>();
        public IBindingToSyntax<T1, T2> Rebind<T1, T2>() => Scope?.Kernel?.Rebind<T1, T2>();
        public IBindingToSyntax<T1, T2, T3> Rebind<T1, T2, T3>() => Scope?.Kernel?.Rebind<T1, T2, T3>();
        public IBindingToSyntax<T1, T2, T3, T4> Rebind<T1, T2, T3, T4>() => Scope?.Kernel?.Rebind<T1, T2, T3, T4>();
        public IBindingToSyntax<object> Rebind(params Type[] services) => Scope?.Kernel?.Rebind(services);
        public void AddBinding(IBinding binding) => Scope?.Kernel?.AddBinding(binding);
        public void RemoveBinding(IBinding binding) => Scope?.Kernel?.RemoveBinding(binding);
        public bool CanResolve(IRequest request) => Scope?.Kernel?.CanResolve(request) ?? false;
        public bool CanResolve(IRequest request, bool ignoreImplicitBindings)
            => Scope?.Kernel?.CanResolve(request, ignoreImplicitBindings) ?? false;
        public IEnumerable<object> Resolve(IRequest request) => Scope?.Kernel?.Resolve(request);
        public bool Release(object instance) => Scope?.Kernel?.Release(instance) ?? false;
        public IRequest CreateRequest(Type service, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters,
            bool isOptional, bool isUnique)
            => Scope?.Kernel?.CreateRequest(service, constraint, parameters, isOptional, isUnique);
        public object GetService(Type serviceType) => (Scope?.Kernel as IKernel)?.GetService(serviceType);
        public bool IsDisposed => Scope?.Kernel?.IsDisposed ?? true;
        public IEnumerable<INinjectModule> GetModules() => Scope?.Kernel?.GetModules();
        public bool HasModule(string name) => Scope?.Kernel?.HasModule(name) ?? false;
        public void Unload(string name) => Scope?.Kernel?.Unload(name);
        public void Inject(object instance, params IParameter[] parameters)
            => Scope?.Kernel?.Inject(instance, parameters);
        public IEnumerable<IBinding> GetBindings(Type service) => Scope?.Kernel?.GetBindings(service);
        public IActivationBlock BeginBlock() => Scope?.Kernel?.BeginBlock();
        public INinjectSettings Settings => Scope?.Kernel?.Settings;
        public IComponentContainer Components => Scope?.Kernel?.Components;
        public void Load(IEnumerable<INinjectModule> assemblies) => Scope?.Kernel?.Load(assemblies);
        public void Load(IEnumerable<string> filePatterns) => Scope?.Kernel?.Load(filePatterns);
        public void Load(IEnumerable<Assembly> m) => Scope?.Kernel?.Load(m);
    }
}
