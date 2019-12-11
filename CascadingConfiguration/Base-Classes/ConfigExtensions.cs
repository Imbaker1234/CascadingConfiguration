using System;
using System.Reflection;

namespace CascadingConfiguration
{
    public static class ConfigExtensions
    {
        public static void SetProperty(this IConfig classToPopulate, PropertyInfo property, string value)
        {
            //Get the type we need to cast to.
            Enum.TryParse(property.PropertyType.Name, true, out TypeCode enumValue);
            //Cast to that type
            var convertedValue = Convert.ChangeType(value, enumValue);
            //Assign
            property.SetValue(classToPopulate, convertedValue);
        }
    }
}