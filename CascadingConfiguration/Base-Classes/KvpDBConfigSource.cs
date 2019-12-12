using System.Collections.Generic;
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
    public class KvpDBConfigSource<T> : ConfigSource<T> where T : IConfig, new()
    {
        //TODO Implement builder syntax for this
        //var dbConfig = new KvpDBConfigSource.ConnectedBy("").usingTable("Table").toGetValuesIn("ValueColumn").IdentifiedBy("IdColumn")
        public string ConnectionString { get; set; }
        public string ConfigTable { get; set; }
        public string ValueColumn { get; set; }
        public string IdColumn { get; set; }

        // TODO Extract the base/common elements of these DBConfigSources to an abstract class.
        /// <summary>
        /// Sets the properties for the IConfig held in this class. If the 
        /// </summary>
        /// <returns></returns>
        public override void PopulateConfig(IConfigProvider<T> configProvider)
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                foreach (var property in configProvider.UnconfiguredProperties)
                {
                    string key = property;

                    var sql = $"SELECT {ValueColumn}" +
                              $"FROM {ConfigTable}" +
                              $"WHERE {IdColumn} = '{key}'";

                    var command = new SqlCommand(sql, cnn);

                    var value = command.ExecuteScalar().ToString();
                    
                    // If the query returns a usable value.
                    if (value.Length > 0)
                    {
                        configProvider.SetProperty(property, value);
                    }
                }
            }
        }

        public KvpDBConfigSource(int priority) : base(priority)
        {
            Priority = priority;
        }
    }
}