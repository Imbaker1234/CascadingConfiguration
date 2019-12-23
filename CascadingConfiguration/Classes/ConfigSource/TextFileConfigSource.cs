using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Reflection;
using System.Text;
using CascadingConfiguration.Extensions;

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
                unsetProperties = new HashSet<PropertyInfo>(config.GetType().GetProperties());

            var sourceInfo = FileToDictionary(FilePath, Delimiter);

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
                catch (NullReferenceException)
                {
                    //If it doesn't match a property in our config we don't add it.
                }
            }

            return unsetProperties;
        }

        public override bool Initialize()
        {
            return File.Exists(FilePath);
        }

        /// <summary>
        /// Converts a string to a Dictionary by separating it into 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public Dictionary<string, string> FileToDictionary(string filePath, char delimiter = '=')
        {
            var product = new Dictionary<string, string>();

            foreach (var line in File.ReadAllLines(filePath))
            {
                var key = line.Split(delimiter)[0];

                var value = line.Split(delimiter)[1];

                product.Add(key, value);
            }
            return product;
        }
    }
}