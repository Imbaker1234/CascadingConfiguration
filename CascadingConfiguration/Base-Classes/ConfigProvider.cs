using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CascadingConfiguration
{
    public abstract class ConfigProvider<T> : IConfigProvider<T> where T : IConfig, new()
    {
        public T Config { get; set; }
        public List<PropertyInfo> InitializedProperties;
        public List<IConfigSource<T>> Sources { get; set; }
        public bool AllowIncompleteConfiguration { get; set; }
        public bool AllowMultipleSources { get; set; }
        public bool Cascade { get; set; }

        protected ConfigProvider(List<IConfigSource<T>> sources, bool allowIncompleteConfiguration = true,
            bool cascade = true)
        {
            Sources = sources;
            AllowIncompleteConfiguration = allowIncompleteConfiguration;
            Cascade = cascade;
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
            Sources.Sort();

            var unsetProperties = typeof(T).GetProperties().ToHashSet();

            foreach (IConfigSource<T> source in Sources)
            {
                //Grab the config from the highest remaining priority source.
                unsetProperties = source.PopulateConfig(Config, null);

                //If you the configuration is fully populated return it.
                if (unsetProperties.Count is 0)
                    return;

                //Otherwise wipe it clean and try the next source.
                unsetProperties = typeof(T).GetProperties().ToHashSet();
                Config = new T();
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
            var allProperties = typeof(T).GetProperties().ToHashSet();

            foreach (var source in Sources)
            {
                source.PopulateConfig(Config, allProperties);

                if (allProperties.Count is 0) return;
            }

            if(!AllowIncompleteConfiguration) throw new Exception("Failed to fully populate configuration from all sources.");
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
                //Iterating over each source in reverse priority, lowest
                //to highest, providing null for unset properties so as
                //to retrieve any and all values from each source.
                //Allowing for the overwrite of values from lower priority
                //sources with values from higher priority sources.
                source.PopulateConfig(Config, null);

                if (!AllowIncompleteConfiguration) throw new Exception("Failed to fully populate configuration from all sources.");
            }
        }
    }
}