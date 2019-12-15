using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CascadingConfiguration.Interfaces;

namespace CascadingConfiguration
{
    public class TextFileConfigSource<T> : ConfigSource<T> where T : IConfig, new()
    {
        public string FilePath { get; set; }
        public char Delimiter { get; set; }

        public TextFileConfigSource(string filePath, char delimiter, int priority)
        {
            Priority = priority;
            FilePath = filePath;
            Delimiter = delimiter;
        }

        public override void PopulateConfig(IConfigProvider<T> configProvider)
        {
            var sourceInfo = ConfigToDictionary(FilePath, Delimiter);

            foreach (var unsetProperty in configProvider.UnconfiguredProperties)
            {
                string key = unsetProperty;
                string value;
                if (sourceInfo[unsetProperty] == null)
                {
                    value = sourceInfo[unsetProperty];
                }
                else
                {
                    continue;
                }

                try
                {
                    configProvider.SetProperty(key, value);
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