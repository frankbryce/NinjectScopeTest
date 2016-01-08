namespace NinjectScopeTest.Exception
{
    /// <summary>
    ///     This object is here to make it clear when an exception occurs
    ///     within the confines of the NinjectScopeTest package.
    /// </summary>
    public class InternalException : NinjectScopeExceptionBase
    {
        public InternalException(string message) : base(message)
        {
        }

        public InternalException(System.Exception innerException)
            : base(innerException)
        {
        }
    }
}