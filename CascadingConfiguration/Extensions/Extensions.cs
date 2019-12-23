using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CascadingConfiguration.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// <para>
        /// Converts the source from a string to any of those specified under the Typecodes
        /// enumeration. (All primitive types, Datetime, etc.)
        /// </para>
        ///<para>
        /// Returns the name of the property being set. 
        /// </para>
        /// </summary>
        /// <param name="config"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetProperty(this IConfig config, string propertyName, string value)
        {
            var property = config.GetType().GetProperty(propertyName);

            return SetProperty(config, property, value);
        }

        /// <summary>
        /// <para>
        /// Converts the source from a string to any of those specified under the Typecodes
        /// enumeration. (All primitive types, Datetime, etc.)
        /// </para>
        /// <para>
        /// Null checking is performed vai checking against null or 0 length values within this
        /// method.
        /// </para>
        /// <para>
        /// Returns the name of the property being set. 
        /// </para>
        /// </summary>
        /// <param name="config"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetProperty(this IConfig config, PropertyInfo property, string value)
        {
            if (value is null || value is "") return null;

            //Get the type we need to cast to.
            Enum.TryParse(property.Name, true, out TypeCode enumValue); //Get the type based on typecode
            //Cast to that type
            var convertedValue = Convert.ChangeType(value, enumValue); //Convert the value to that of the typecode
            //Assign
            property.SetValue(config, convertedValue); // Set the converted value

            return property.Name;
        }

        /// <summary>
        /// Add multiple sources directory the list of sources for this ConfigProvider.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="sources"></param>
        public static void Add(this IConfigProvider<IConfig> provider, params IConfigSource<IConfig>[] sources)
        {
            foreach (var source in sources)
            {
                provider.Add(source);
            }
        }

        /// <summary>
        /// Add a single source to this ConfigProvider.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="source"></param>
        public static void Add(this IConfigProvider<IConfig> provider, IConfigSource<IConfig> source)
        {
            provider.Sources.Add(source);
        }

        /// <summary>
        /// Remove multiple sources from this ConfigProvider.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="sources"></param>
        public static void Remove(this IConfigProvider<IConfig> provider, IConfigSource<IConfig>[] sources)
        {
            foreach (var source in sources)
            {
                provider.Remove(source);
            }
        }

        /// <summary>
        /// Remove a single source from this ConfigProvider.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="source"></param>
        public static void Remove(this IConfigProvider<IConfig> provider, IConfigSource<IConfig> source)
        {
            provider.Sources.Remove(source);
        }
    }
}