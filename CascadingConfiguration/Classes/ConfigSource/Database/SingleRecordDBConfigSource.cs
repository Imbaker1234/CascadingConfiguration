using System.Collections.Generic;
using System.Data.SqlClient;

namespace CascadingConfiguration
{
    public class SingleRecordDbConfigSource<T> : DBConfigSource<T> where T : IConfig, new()
    {
        public string IdentifyingValue { get; set; }

        /// <summary>
        /// Generates configuration values pulled from a single record where the column and property
        /// values are identical.
        /// </summary>
        /// <returns></returns>
        public override void PopulateConfig(IConfigProvider<T> configProvider)
        {
            string sql = $"SELECT TOP(1)" +
                         $"FROM {Table}" +
                         $"WHERE {IdColumn} = '{IdentifyingValue}'";

            using (var reader = Database.QueryMultipleValues(sql))
            {
                foreach (var property in configProvider.Config.GetType().GetProperties())
                {
                    var value = reader[property.Name].ToString();

                    if (value.Length > 0)
                    {
                        configProvider.SetProperty(property, value);
                    }
                }
            }
        }

        public SingleRecordDbConfigSource(int priority) : base(priority)
        {
            Priority = priority;
        }
    }
}