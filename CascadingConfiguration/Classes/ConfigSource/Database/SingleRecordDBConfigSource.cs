using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CascadingConfiguration;
using CascadingConfiguration.Base_Classes;

namespace CascadingConfiguration.Classes.ConfigSource.Database
{
    public class SingleRecordDbConfigSource<T> : DbConfigSource<T> where T : IConfig, new()
    {
        public string IdentifyingValue { get; set; }

        /// <summary>
        /// Generates configuration values pulled from a single record where the column and property
        /// values are identical.
        /// </summary>
        /// <returns></returns>
        public override HashSet<PropertyInfo> PopulateConfig(IConfig config, HashSet<PropertyInfo> unsetProperties)
        {
            if (unsetProperties is null || unsetProperties.Count is 0)
                unsetProperties = config.GetType().GetProperties().ToHashSet();

            string sql = $"SELECT TOP(1)" +
                         $"FROM {Table}" +
                         $"WHERE {IdColumn} = '{IdentifyingValue}'";

            using (var reader = Database.QueryMultipleValues(sql))
            {
                foreach (var property in config.GetType().GetProperties())
                {
                    var value = reader[property.Name].ToString();

                    config.SetProperty(property, value);

                    unsetProperties.Remove(property);
                }
            }

            return unsetProperties;
        }


        public SingleRecordDbConfigSource(int priority) : base(priority)
        {
            Priority = priority;
        }
    }
}