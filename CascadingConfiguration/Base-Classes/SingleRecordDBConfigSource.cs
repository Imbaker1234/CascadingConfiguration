using System.Data.SqlClient;

namespace CascadingConfiguration
{
    public class SingleRecordDBConfigSource<T> : DBConfigSource<T> where T : IConfig, new()
    {
        //TODO Implement builder syntax for this
        //var dbConfig = new KvpDBConfigSource.ConnectedBy("").usingTable("Table").toGetValuesIn("ValueColumn").IdentifiedBy("IdColumn")
        public string ConnectionString { get; set; }
        public string Table { get; set; }
        public string IdColumn { get; set; }
        public string IdentifyingValue { get; set; }

        // TODO Extract the base/common elements of these DBConfigSources to an abstract class.
        /// <summary>
        /// Sets the properties for the IConfig held in this class. If the 
        /// </summary>
        /// <returns></returns>
        public override void PopulateConfig(IConfigProvider<T> configProvider)
        {
            using (SqlConnection cnn = new SqlConnection(ConnectionString))
            {
                string sql = $"SELECT TOP(1)" +
                                 $"FROM {Table}" +
                                 $"WHERE {IdColumn} = '{IdentifyingValue}'";

                    var command = new SqlCommand(sql, cnn);

                    using (var reader = command.ExecuteReader())
                    {
                        // TODO Build out dictionary from ordinals.
                        foreach (var property in configProvider.Config.GetType().GetProperties())
                        {
                            configProvider.SetProperty(property, reader.Get);
                        }

                        // If the query returns a usable value.
                        if (value.Length > 0)
                        {
                            configProvider.SetProperty(property, value);
                        }
                    }
                }
            }
        }

        public SingleRecordDBConfigSource(int priority) : base(priority)
        {
            Priority = priority;
        }
    }
}