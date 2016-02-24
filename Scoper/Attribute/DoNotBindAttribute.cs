namespace Scoper.Attribute
{
    /// <summary>
    ///     Put this attribute on one of your scope
    ///     properties if you do not want Scoper.Ninject.AutoScopeTest
    ///     to bind the mocked instance to the type of
    ///     the generic attribute on your mocked property.
    /// </summary>
    public class DoNotBindAttribute : System.Attribute
    {
    }
}