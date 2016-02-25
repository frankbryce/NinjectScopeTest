using System;
using System.Linq;
using Moq;
using Scoper.Exception;

namespace Scoper
{
    /// <summary>
    /// Default AutoScopeTest uses the default Scope for minimal setup for 
    /// the simplest tests.
    /// </summary>
    public abstract class AutoScopeTest : AutoScopeTest<Scope>
    {
    }

    /// <summary>
    /// This is a base type, use one of the derived DI AutoScopeTest objects.
    /// This class by itself will perform injection of default values for types,
    /// and will not return any meaningful values for your dependencies.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public abstract class AutoScopeTest<T> : ScopeTest<T> where T : Scope, new()
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected internal virtual bool _hasRegistration(Type type)
        {
            return false;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected internal virtual object DiContainerGet(Type type)
        {
            return DefaultValue.Get(type);
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
        protected override object Get(Type type)
        {
            try
            {
                // if the dependencies are all set to go, use default behavior
                return DiContainerGet(type);
            }
            catch (GetInstanceException)
            {
                // if activation fails in Ninject, let's bind all of the dependencies that could be used, then
                // call base again to use the same constructor overloading rules that are used by default in Ninject.
                var constructors = type.GetConstructors();
                var constructorToUse = constructors.OrderBy(x => x.GetParameters().Length).First();
                foreach (var param in constructorToUse.GetParameters())
                {
                    var paramType = param.ParameterType;

                    if (_hasRegistration(paramType)) continue;

                    var defaultValue = DefaultValue.Get(paramType);

                    var obj = paramType.IsValueType || (paramType.IsClass && paramType.GetConstructor(Type.EmptyTypes) == null) ?
                        defaultValue :
                        ((Mock)typeof(Mock<>)
                            .MakeGenericType(paramType)
                            .GetConstructor(Type.EmptyTypes)
                            .Invoke(new object[] { }))
                            .Object;

                    RegisterObject(paramType, obj);
                }
                return DiContainerGet(type);
            }
        }
    }
}
