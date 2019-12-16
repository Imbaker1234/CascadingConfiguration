using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CascadingConfiguration.Base_Classes;

namespace CascadingConfiguration.Classes.ConfigSource.Database
{
    /// <summary>
    /// <para>
    /// Specifies a configuration held in a database with the following specifications.
    /// </para>
    ///
    /// 
    /// The values are all held in the same table, held in the same column,
    /// identified by matching property name in the same ID column specified
    /// in the WHERE clause.
    /// </para>
    /// </summary>
    /// <typeparam name="ConfigProvider<T>"></typeparam>
    public class KvpDBConfigSource<T> : DbConfigSource<T> where T : IConfig, new()
    {
        public string ValueColumn { get; set; }

        /// <summary>
        /// Using a property first approach, each property seeks out a matching
        /// key column entry for its corresponding value column entry. If not
        /// found it skips the value.
        /// </summary>
        /// <returns></returns>
        public override HashSet<PropertyInfo> PopulateConfig(IConfig config, HashSet<PropertyInfo> unsetProperties)
        {
            if (unsetProperties is null || unsetProperties.Count is 0)
                unsetProperties = config.GetType().GetProperties().ToHashSet();

            foreach (var property in unsetProperties)
            {
                string sql = $"SELECT {ValueColumn}" +
                             $"FROM {Table}" +
                             $"WHERE {IdColumn} = '{property}'";

                var value = Database.QuerySingleValue(sql);

                config.SetProperty(property, value);

                unsetProperties.Remove(property);
            }

            return unsetProperties;
    }

        public KvpDBConfigSource(int priority) : base(priority)
        {
        }
    }