using System.Data.SqlClient;

namespace CascadingConfiguration
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
    /// <typeparam name="T"></typeparam>
    public class KvpDBConfigSource<T> : DBConfigSource<T> where T : IConfig, new()
    {
        public string ValueColumn { get; set; }

        /// <summary>
        /// Using a property first approach, each property seeks out a matching
        /// key column entry for its corresponding value column entry. If not
        /// found it skips the value.
        /// </summary>
        /// <returns></returns>
        public override void PopulateConfig(IConfigProvider<T> configProvider)
        {
            foreach (var property in configProvider.UnconfiguredProperties)
            {
                string key = property;

                string sql = $"SELECT {ValueColumn}" +
                             $"FROM {Table}" +
                             $"WHERE {IdColumn} = '{key}'";

                var value = Database.QuerySingleValue(sql);

                // If the query returns a usable value.
                if (value.Length > 0)
                {
                    configProvider.SetProperty(property, value);
                }
            }
        }

        public KvpDBConfigSource(int priority) : base(priority)
        {
            Priority = priority;
        }
    }
}