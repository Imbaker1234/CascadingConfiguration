using System.Collections.Generic;
using CascadingConfiguration.Interfaces;

namespace CascadingConfiguration
{
    public abstract class ConfigProvider<T> : IConfigProvider<T> where T : IConfig, new()
    {
        public abstract T GetConfig();
        public List<IConfigSource<T>> Sources { get; set; }
        public bool AllowIncompleteConfiguration { get; set; }
        public bool AllowMultipleSources { get; set; }
        public bool Cascade { get; set; }

        protected ConfigProvider(List<IConfigSource<T>> sources, bool allowIncompleteConfiguration = true, bool cascade= true)
        {
            Sources = sources;
            AllowIncompleteConfiguration = allowIncompleteConfiguration;
            Cascade = cascade;
        }
    }
}