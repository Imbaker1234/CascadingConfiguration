using System.Collections.Generic;
using System.Reflection;

namespace CascadingConfiguration
{
    public interface IConfigProvider<T> where T : IConfig
    {
        T Config { get; set; }
        List<IConfigSource<T>> Sources { get; set; }
        bool AllowIncompleteConfiguration { get; set; }
        bool AllowMultipleSources { get; set; }
        bool Cascade { get; set; }
        HashSet<string> ConfiguredProperties { get; set; }
        HashSet<string> UnconfiguredProperties { get; set; }

        void PopulateConfig();

        void SetProperty(PropertyInfo property, string value);
        void SetProperty(string property, string value);
    }
}