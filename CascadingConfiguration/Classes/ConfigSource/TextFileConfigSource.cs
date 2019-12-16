using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CascadingConfiguration;
using CascadingConfiguration.Base_Classes;

namespace CascadingConfiguration.Classes.ConfigSource
{
    public class TextFileConfigSource<T> : ConfigSource<T> where T : IConfig, new()
    {
        public string FilePath { get; set; }
        public char Delimiter { get; set; }

        public TextFileConfigSource(string filePath, char delimiter, int priority) : base(priority)
        {
            FilePath = filePath;
            Delimiter = delimiter;
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

        /// <summary>
        /// <para>
        /// Sets the IConfig based on the key value pairs specified in the text file located at this sources
        /// filepath.
        /// </para>
        /// <para>
        /// Takes in properties unset by previous sources (or null/new HashSet if this is the first) and returns
        /// any properties that are left over.
        /// </para>
        /// </summary>
        /// <param name="config"></param>
        /// <param name="unsetProperties"></param>
        /// <returns></returns>
        public override HashSet<PropertyInfo> PopulateConfig(IConfig config, HashSet<PropertyInfo> unsetProperties)
        {
            if (unsetProperties is null || unsetProperties.Count is 0)
                unsetProperties = config.GetType().GetProperties().ToHashSet();

            var sourceInfo = ConfigToDictionary(FilePath, Delimiter);

            foreach (var unsetProperty in unsetProperties)
            {
                string key = unsetProperty.Name;
                string value;
                if (sourceInfo.ContainsKey(key))
                {
                    value = sourceInfo[key];
                }
                else
                {
                    continue;
                }

                try
                {
                    config.SetProperty(key, value);
                    unsetProperties.Remove(unsetProperty);
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

            return unsetProperties;
        }
    }
}