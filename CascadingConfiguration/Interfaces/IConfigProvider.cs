using System.Collections.Generic;
using System.Reflection;

namespace CascadingConfiguration
{
    public interface IConfigProvider<T> where T : IConfig
    {
        T Config { get; set; }
        List<IConfigSource<T>> Sources { get; set; }
        bool AllowIncompleteConfiguration { get; set; }
    }
}