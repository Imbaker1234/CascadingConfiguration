using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CascadingConfiguration
{
    public abstract class ConfigSource<T> : IComparable, IConfigSource<IConfig> where T : IConfigProvider<IConfig>, new()
    {
        public int Priority { get; set; }

        public List<PropertyInfo> PopulateConfig(IConfig config)
        {
            throw new NotImplementedException();
        }

        public List<PropertyInfo> PopulateConfig(IConfig config, List<PropertyInfo> unsetProperties)
        {
            throw new NotImplementedException();
        }


        protected ConfigSource(int priority)
        {
            Priority = priority;
        }
        
        public int CompareTo(object obj)
        {
            if (obj is null)
                return 1;

            var that = obj as IConfigSource<IConfig>;

            return this.Priority.CompareTo(that.Priority);
        }
    }
}