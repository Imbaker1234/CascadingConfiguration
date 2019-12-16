using System.Collections.Generic;
using System.Reflection;

namespace CascadingConfiguration
{
    public interface IConfigSource<T> where T : IConfig

    {
        int Priority { get; set; }
        HashSet<PropertyInfo> PopulateConfig(T config, HashSet<PropertyInfo> unsetProperties);
    }
}