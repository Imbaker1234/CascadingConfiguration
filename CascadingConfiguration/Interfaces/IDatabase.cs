using System.Data.SqlClient;

namespace CascadingConfiguration
{
    public interface IDatabase
    {
        string ConnectionString { get; set; }

        string QuerySingleValue(string sql);

        SqlDataReader QueryMultipleValues(string sql);
    }
}