using log4net;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using Scoper.Exception;

namespace Scoper
{
    /// <summary>
    /// This is a base type, use one of the derived DI AutoScopeTest objects.
    /// This class by itself will perform injection of default values for types,
    /// and will not return any meaningful values for your dependencies.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public abstract class AutoScopeTest : MoqMockingKernel
    {
        protected static readonly ILog Logger =
            LogManager.GetLogger(typeof(AutoScopeTest));

        protected AutoScopeTest() : base(new NinjectSettings { AllowNullInjection = true })
        {
        }

        /// <summary>
        /// This will create a Mock{U} object and register with the backing
        /// DI container to inject into classes which depend on type U
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        protected new Mock<U> GetMock<U>() where U : class
        {
            try
            {
                return base.GetMock<U>();
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
        /// all dependencies of all constructors of type and return IKernel.Get{U}().
        /// For each dependency which is not already registered with the Ninject
        /// kernel, Ninject will bind it with a Moq.Mock.Object reference if
        /// U is a non-sealed type.  If U is a class without a default constructor,
        /// or a value type, then we register a default value for that type.
        /// All registrations of previously unbound types are done as singleton
        /// constant values; using Get{U}() multiple times will inject the same
        /// dependencies each time for the same U
        /// </summary>
        /// <returns>An instance of an object of type U</returns>
        protected U Get<U>()
        {
            try
            {
                return (U) this.Get(typeof(U));
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
