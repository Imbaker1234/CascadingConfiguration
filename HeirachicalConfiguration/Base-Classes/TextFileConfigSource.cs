using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HeirachicalConfiguration.Interfaces;

namespace HeirachicalConfiguration
{
    public class TextFileConfigSource<T> : ConfigSource<T> where T : IConfig, new()
    {
        public string FilePath { get; set; }
        public char Delimiter { get; set; }
        public T Config { get; set; }
        public IConfigProvider<T> Provider { get; set; }
        public int Priority { get; set; }

        public TextFileConfigSource(string filePath, char delimiter, T config, IConfigProvider<T> provider, int priority)
        {
            Config = Config == null ? new T() : config;
            Provider = provider;
            Priority = priority;
            FilePath = filePath;
            Delimiter = delimiter;
        }

        public override T SourceConfig()
        {
            var textConfig = ConfigToDictionary(FilePath, Delimiter);

            foreach (var entry in textConfig)
            {
                var key = entry.Key;
                var value = entry.Value;

                try
                {
                    var property = typeof(T).GetProperty(key);

                    SetPrimitive(property, Config, value);
                }
                catch (NullReferenceException nre)
                {
                    var sb = new StringBuilder();

                    sb.Append(Environment.NewLine);

                    foreach (var property in typeof(T).GetProperties())
                    {
                        sb.Append($"{property}{Environment.NewLine}");
                    }
                    throw new NullReferenceException(
                        $"{typeof(IConfig)}.{key} is not a valid property in this configuration. Valid properties are: {sb}");
                }
            }

            return Config;
            }

        public Dictionary<string, string> ConfigToDictionary(string filePath, char delimiter = '=')
        {
            var product = new Dictionary<string, string>();

            foreach (var line in File.ReadAllLines(FilePath))
            {
                var key = line.Split(delimiter)[0];

                var value = line.Split(delimiter)[1];

                product.Add(key, value);
            }
            return product;
        }
    }
}