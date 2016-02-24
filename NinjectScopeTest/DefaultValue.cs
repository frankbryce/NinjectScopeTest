using System;

namespace Scoper
{
    internal static class DefaultValue
    {
        /// <summary>
        /// Returns the default value for a given type dynamically.  Useful
        /// when the type is not known at compile time
        /// </summary>
        /// <param name="type">The type of object to return.</param>
        /// <returns>Default value of type type</returns>
        public static object Get(Type type)
        {
            // programmatically get default type, which is based on
            // https://stackoverflow.com/questions/325426/programmatic-equivalent-of-defaulttype
            object defaultValue = null;
            if (type.IsValueType)
            {
                defaultValue = Activator.CreateInstance(type);
            }
            return defaultValue;
        }
    }
}
