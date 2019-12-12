using System;
using System.Collections.Generic;

namespace CascadingConfiguration
{
    public abstract class ConfigSource<T> : IComparable, IConfigSource<T> where T : IConfig, new()
    {
        public int Priority { get; set; }
        public abstract void PopulateConfig(IConfigProvider<T> configProvider);
        protected ConfigSource(int priority)
        {
            Priority = priority;
        }

        public int CompareTo(object obj)
        {
            if (obj is null)
                return 1;

            var that = obj as IConfigSource<T>;

            return this.Priority.CompareTo(that.Priority);
        }
    }
}