using System;
using System.Reflection;
using HeirachicalConfiguration.Interfaces;

namespace HeirachicalConfiguration
{
    public abstract class ConfigSource<T> : IConfigSource<T> where T : IConfig, new()
    {
        public T Config { get; set; }
        public IConfigProvider<T> Provider { get; set; }
        public int Priority { get; set; }
        public abstract T SourceConfig();

        protected ConfigSource()
        {

        }

        protected ConfigSource(T config, IConfigProvider<T> provider, int priority)
        {
            //Create a new config if null.
            Config = Config == null ? new T() : config;
            Provider = provider;
            Priority = priority;
        }

        public void SetPrimitive(PropertyInfo property, T product, string value)
        {
            if (property.PropertyType == typeof(bool))
            {
                property.SetValue(product, bool.Parse(value));
            }
            else if (property.PropertyType == typeof(int))
            {
                property.SetValue(product, int.Parse(value));
            }
            else if (property.PropertyType == typeof(double))
            {
                property.SetValue(product, double.Parse(value));
            }
            else if (property.PropertyType == typeof(char))
            {
                property.SetValue(product, char.Parse(value));
            }
            else if (property.PropertyType == typeof(float))
            {
                property.SetValue(product, float.Parse(value));
            }
            else if (property.PropertyType == typeof(long))
            {
                property.SetValue(product, long.Parse(value));
            }
            else if (property.PropertyType == typeof(short))
            {
                property.SetValue(product, short.Parse(value));
            }
            else if (property.PropertyType == typeof(string))
            {
                property.SetValue(product, value);
            }
            else
            {
                throw new Exception($"Invalid type '{property.PropertyType}'. This class will need to be extended to support this.");
            }
        }
    }
}