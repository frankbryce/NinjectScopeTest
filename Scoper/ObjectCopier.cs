using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Scoper
{
    public static class ObjectCopier
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source) where T : new()
        {
            if (typeof(T).IsValueType)
            {
                return source;
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            if (!typeof(T).IsSerializable)
            {
                var newSource = new T();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    if (propertyInfo.CanRead && propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(newSource, Clone(propertyInfo.GetValue(source, null)));
                    }
                }
                return newSource;
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
