namespace NinjectScopeTest.Exception
{
    public class InternalException : NinjectScopeExceptionBase
    {
        public InternalException(string message) : base(message)
        {
        }

        public InternalException(System.Exception innerException) : base(innerException)
        {
        }
    }
}