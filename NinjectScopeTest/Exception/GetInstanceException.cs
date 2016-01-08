namespace NinjectScopeTest.Exception
{
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