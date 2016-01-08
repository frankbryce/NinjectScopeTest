namespace NinjectScopeTest.Attribute
{
    /// <summary>
    ///     Put this attribute on one of your scope
    ///     properties if you do not want NinjectScopeTest
    ///     to create a mocked instance of this object
    ///     automatically.
    /// </summary>
    public class DoNotInstantiateAttribute : System.Attribute
    {
    }
}