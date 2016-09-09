using System;
using System.Collections.Generic;
using System.Linq;

namespace Scoper.Exception
{
    /// <summary>
    ///     The base object from which all Exceptions within the
    ///     AutoScopeTest package derive.  This can be used to
    ///     catch any exception which may be thrown by this package.
    /// </summary>
    public abstract class NinjectScopeExceptionBase : System.Exception
    {
        protected NinjectScopeExceptionBase(string message) : base(message)
        {
        }

        protected NinjectScopeExceptionBase(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        protected NinjectScopeExceptionBase(System.Exception innerException)
            : base(innerException.Message, innerException)
        {
        }
    }
}