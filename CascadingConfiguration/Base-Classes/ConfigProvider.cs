using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CascadingConfiguration
{
    public abstract class ConfigProvider<T> : IConfigProvider<T> where T : IConfig, new()
    {
        public T Config { get; set; }
        public List<IConfigSource<T>> Sources { get; set; }
        public bool AllowIncompleteConfiguration { get; set; }
        public bool AllowMultipleSources { get; set; }
        public bool Cascade { get; set; }
        public HashSet<string> ConfiguredProperties { get; set; }
        public HashSet<string> UnconfiguredProperties { get; set; }

        protected ConfigProvider(List<IConfigSource<T>> sources, bool allowIncompleteConfiguration = true,
            bool cascade = true)
        {
            Sources = sources;
            Sources.Sort(); //Sort sources based on greatest priority to least.
            AllowIncompleteConfiguration = allowIncompleteConfiguration;
            Cascade = cascade;
            ConfiguredProperties = new List<string>();
            UnconfiguredProperties = new List<string>();

            foreach (var property in Config.GetType().GetProperties())
            {
                UnconfiguredProperties.Add(property.Name);
            }

        }

        public void PopulateConfig()
        {
            Config = new T();
        }

        /// <summary>
        /// <para>
        /// Returns a full configuration from a single source
        /// </para>
        ///<para>
        /// If a source fails to fully populate the config then
        /// the config is cleared and the next one is tried,
        /// throwing an exception if all sources fail to provide
        /// a fully populated config.
        /// </para>
        /// </summary>
        private void SingleSourcePopulate()
        {
            foreach (IConfigSource<T> source in Sources)
            {
                source.PopulateConfig(this);

                if (UnconfiguredProperties.Any())
                {
                    //Since we are only allowing values from a single source
                    //we clear out the config if we don't retrieve a full one.
                    Config = new T();
                    //Add any configured values back to to unconfigured values.
                    UnconfiguredProperties.UnionWith(ConfiguredProperties);
                }
            }

            //If after we are done going through all of the sources we don't have
            //a fully populated config an exception is thrown.
            if (UnconfiguredProperties.Any())
            {
                throw new Exception("All sources failed to provide a fully populated config.");
            }
        }

        /// <summary>
        /// <para>
        /// Populates the configuration. Pulling as many values
        /// as possible from sources until fully populated.
        /// </para>
        /// <para>
        /// Unlike with CascadingPopulate values are not overwritten.
        /// Instead the execution halts as the configs values are all
        /// set.
        /// </para>
        /// <para>
        /// In the event that there is not a fully populated configuration
        /// and AllowIncompleteConfiguration is not 'True' an exception
        /// will be thrown.
        /// </para>
        /// </summary>
        private void MultiSourcePopulate()
        {
            foreach (var source in Sources)
            {
                //Iterating over each source which checks only for unset
                //properties held in this class.
                source.PopulateConfig(this);

                //While it won't overwrite any values this will prevent
                //any unnecessary iteration over additional sources once
                //we are fully configured.
                if (!UnconfiguredProperties.Any())
                {
                    break; 
                }
            }
        }

        public void CascadingPopulate()
        {
            //Starts the process from lowest priority and works our way
            //up. Allowing higher priority sources to overwrite properties
            //set by those of lower priority sources.
            Sources.Sort();
            Sources.Reverse();
            foreach (var source in Sources)
            {
                //Iterating over each source which checks only for unset
                //properties held in this class.
                source.PopulateConfig(this);

                UnconfiguredProperties.UnionWith(ConfiguredProperties);
                ConfiguredProperties.Clear();
            }
        }

        public void SetProperty(PropertyInfo property, string value)
        {
            //Get the type we need to cast to.
            Enum.TryParse(property.PropertyType.Name, true, out TypeCode enumValue); //Get the type based on typecode
            //Cast to that type
            var convertedValue = Convert.ChangeType(value, enumValue); //Convert the value to that of the typecode
            //Assign
            property.SetValue(this, convertedValue); // Set the converted value

            UnconfiguredProperties.Remove(property.Name); // Remove property as unset.
            ConfiguredProperties.Add(property.Name); // and mark it as set.
        }

        public void SetProperty(string property, string value)
        {
            SetProperty(Config.GetType().GetProperty(property), value);
        }
    }
}