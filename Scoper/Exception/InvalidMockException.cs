namespace Scoper.Exception
{
    public class InvalidMockException : NinjectScopeExceptionBase
    {
        public InvalidMockException(string message) : base(message)
        {
        }

        public InvalidMockException(System.Exception innerException) : base(innerException)
        {
        }
    }
}