using System;
using System.Linq;
using NinjectScopeTest.Exception;
using Moq;
using Ninject;

namespace NinjectScopeTest
{
    /// <summary>
    /// Default NinjectAutoScopeTest wihtout specified test scope uses the default
    /// NinjectScope for minimal setup for the simplest tests and fast setup.
    /// </summary>
    public abstract class NinjectAutoScopeTest : NinjectAutoScopeTest<NinjectScope> { }

    /// <summary>
    /// An augmented ScopeTest platform for automatically attempting to resolve dependencies
    /// that you did not specify in your TestScope.  This is useful when you want a default
    /// Mock<T> behavior without special setup and you want to save the time and test-code
    /// space by explicitly adding Mocked properties in your TestScope.  One feature of
    /// NinjectAutoScopeTest which is not compatible with NinjectScopeTest is the Attribute
    /// DoNotInstantiate.  NinjectAutoScopeTest will override this behavior and instantiate the
    /// object as if the property were not in your TestScope.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NinjectAutoScopeTest<T> : NinjectScopeTest<T> where T : NinjectScope, new()
    {
        /// <summary>
        /// This will attempt to get an instance of type U via the kernel as it is
        /// setup manually.  On a failure, then NinjectAutoScopeTest will register
        /// all dependencies of all constructors of U and return IKernel.Get<U>().
        /// For each dependency which is not already registered with the Ninject
        /// kernel, Ninject will bind it with a Moq.Mock<U>.Object reference if
        /// U is a non-sealed type.  If U is sealed, then we register default(U).
        /// All registrations of previously unbound types are done as singleton
        /// constant values.  Using Get<U> multiple times will inject the same
        /// dependencies each time.
        /// </summary>
        /// <typeparam name="U">The type of object to construct with the Ninject kernel in the scope</typeparam>
        /// <returns>An instance of an object of type U</returns>
        protected override U Get<U>()
        {
            return (U) Get(typeof (U));
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
                // if the dependencies are all set to go, use default behavior!
                return base.Get(type);
            }
            catch (GetInstanceException)
            {
                // if activation fails in Ninject, let's bind all of the dependencies that could be used, then
                // call base again to use the same constructor overloading rules that are used by defauly in Ninject.
                var constructors = type.GetConstructors();
                var constructorToUse = constructors.OrderBy(x => x.GetParameters().Length).First();
                foreach (var param in constructorToUse.GetParameters())
                {
                    var paramType = param.ParameterType;

                    if (Scope.Kernel.TryGet(paramType) != null) continue;

                    // programmatically get default type, which is based on
                    // https://stackoverflow.com/questions/325426/programmatic-equivalent-of-defaulttype
                    object defaultValue = null;
                    if (paramType.IsValueType)
                    {
                        defaultValue = Activator.CreateInstance(paramType);
                    }

                    var obj = paramType.IsValueType || (paramType.IsClass && paramType.GetConstructor(Type.EmptyTypes)==null) ?
                        defaultValue :
                        ((Mock)typeof(Mock<>)
                            .MakeGenericType(paramType)
                            .GetConstructor(Type.EmptyTypes)
                            .Invoke(new object[] { }))
                            .Object;
                    Scope.Kernel.Bind(paramType).ToConstant(obj);
                }
                return base.Get(type);
            }
        }
    }
}
