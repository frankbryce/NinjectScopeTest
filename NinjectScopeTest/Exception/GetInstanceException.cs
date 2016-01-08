namespace NinjectScopeTest.Exception
{
    /// <summary>
    ///     This Exception can be thrown when a Ninject IKernel
    ///     attempts to Activate an instance of an object when
    ///     the Kernel does not have enough information to
    ///     activate the object.  This can happen if you have more
    ///     than one mock for a given type which is not marked as
    ///     [DoNotBind], or if you do not have a Mocked property
    ///     on your scope which is necessary for ninject to
    ///     activate the object.
    /// </summary>
    public class GetInstanceException : NinjectScopeExceptionBase
    {
        public GetInstanceException(string message) : base(message)
        {
        }

        public GetInstanceException(System.Exception innerException)
            : base(innerException)
        {
        }
    }
}