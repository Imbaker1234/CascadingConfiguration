using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using CascadingConfiguration.Extensions;

namespace CascadingConfiguration.Classes.ConfigSource
{
    /// <summary>
    /// A relatively simple class that doesn't provide much functionality beyond what
    /// System.Configuration provides. The reason this class is in place is to allow
    /// it to act as a source for ConfigProvider and act in concert with the other
    /// configurations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AppConfigSource<T> : ConfigSource<T> where T : IConfig, new()
    {
        public override bool Initialize()
        {
            if (!File.Exists(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)) return false;

            try
            {
                Priority = int.Parse(ConfigurationManager.AppSettings["CCSPriority"]);
            }
            catch (Exception)
            {
                //Ignored
            }

            return true;
        }

        /// <summary>
        /// <para>
        /// Sets the properties in the IConfig object. The properties in the app config file
        /// should match the names of the properties in the IConfig object.
        /// </para>
        /// <para>
        /// Takes in properties unset by previous sources (or null/new HashSet if this is the first) and returns
        /// any properties that are left over.
        /// </para>
        /// </summary>
        /// <param name="config"></param>
        /// <param name="unsetProperties"></param>
        /// <returns></returns>
        public override HashSet<PropertyInfo> PopulateConfig(IConfig config,
            HashSet<PropertyInfo> unsetProperties)
        {
            if (unsetProperties is null || unsetProperties.Count is 0)
                unsetProperties = new HashSet<PropertyInfo>(config.GetType().GetProperties());

            foreach (var property in unsetProperties)
            {
                config.SetProperty(property, ConfigurationManager.AppSettings[property.Name]);

                unsetProperties.Remove(property);
            }

            return unsetProperties;
        }

        public AppConfigSource(int priority) : base(priority)
        {
        }
    }
}