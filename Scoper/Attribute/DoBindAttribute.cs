namespace Scoper.Attribute
{
    /// <summary>
    ///     This is the default behavior for properties
    ///     which are of type Mock<U> on your Scope class
    ///     in your unit test class.  This will be bound
    ///     to the Ninject Kernel on Scope by calling
    ///     Ninject.Bind(typeof(U)).ToConstant(mockedObj);
    /// </summary>
    public class DoBindAttribute : System.Attribute
    {
    }
}