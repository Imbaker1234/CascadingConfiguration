using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CascadingConfiguration.Base_Classes
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
    }
}
