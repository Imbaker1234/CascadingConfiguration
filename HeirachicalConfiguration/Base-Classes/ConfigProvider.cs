using System.Collections.Generic;
using HeirachicalConfiguration.Interfaces;

namespace HeirachicalConfiguration
{
    public abstract class ConfigProvider<T> : IConfigProvider<T> where T : IConfig, new()
    {
        public abstract T GetConfig();

        public List<IConfigSource<T>> Sources { get; set; }
        public bool AllowIncompleteConfiguration { get; set; }
        public bool AllowMultipleSources { get; set; }
    }
}