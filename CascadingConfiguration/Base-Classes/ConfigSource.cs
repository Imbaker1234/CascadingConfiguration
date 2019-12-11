using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using CascadingConfiguration.Interfaces;

namespace CascadingConfiguration
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
    }
}