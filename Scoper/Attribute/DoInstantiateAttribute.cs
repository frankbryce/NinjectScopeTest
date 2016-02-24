namespace Scoper.Attribute
{
    /// <summary>
    ///     This is the default behavior for properties
    ///     which are of type Mock<U> on your Scope class
    ///     in your unit test class.  This will be instantiated
    ///     by calling the equivalent of 
    ///     System.Activator.CreateInstance(typeof(Mock<U>))
    /// </summary>
    public class DoInstantiateAttribute : System.Attribute
    {
    }
}