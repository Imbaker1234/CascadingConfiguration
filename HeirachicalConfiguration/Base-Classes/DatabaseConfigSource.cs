using System.Data.SqlClient;
using System.Dynamic;

namespace HeirachicalConfiguration
{
    public class DatabaseConfigSource<T> : ConfigSource<T> where T : IConfig, new()
    {
        public string ConnectionString { get; set; }

        public override T SourceConfig()
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                foreach (var property in Config.GetType().GetProperties())
                {
                    string value = null;

//                    string table;
//                    string valueColumn;
//                    string idColumn;
//                    string key = property.Name;
//
//                    var sql = $"SELECT {valueColumn}" +
//                              $"FROM {table}" +
//                              $"WHERE {idColumn} = '{key}'";

//                    var command = new SqlCommand(sql, cnn);

//                    var blah = command.ExecuteScalar();
                    SetPrimitive(property, Config, value);
                }

                return default(T); //TODO remove this and put proper return
            }
        }

//        private T QuerySingleValue(Property)
//        {
//        }
    }
}