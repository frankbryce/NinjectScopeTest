namespace NinjectScopeTest.Exception
{
    public abstract class NinjectScopeExceptionBase : System.Exception
    {
        protected NinjectScopeExceptionBase(string message) : base(message)
        {
        }

        protected NinjectScopeExceptionBase(System.Exception innerException)
            : base(innerException.Message, innerException)
        {
        }
    }
}