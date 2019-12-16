using System.Data.SqlClient;
using CascadingConfiguration;

namespace CascadingConfiguration.Classes.ConfigSource.Database
{
    public class Database : IDatabase
    {
        public string ConnectionString { get; set; }

        public string QuerySingleValue(string sql)
        {
            string product;

            using (var cnn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cnn))
                {
                    product =  cmd.ExecuteScalar().ToString();
                }
            }

            return product;
        }

        public SqlDataReader QueryMultipleValues(string sql)
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cnn))
                {
                    return cmd.ExecuteReader();
                }
            }
        }
    }
}