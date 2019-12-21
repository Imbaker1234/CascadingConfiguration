using System.Data.SqlClient;

namespace CascadingConfiguration
{
    public abstract class Database
    {
        private string _connectionString;

        public string ConnectionString
        {
            get => null; //This object should not distribute sensitive information.
            set => _connectionString = value;
        }

        public abstract string QuerySingleValue(string sql);

        public abstract SqlDataReader QueryMultipleValues(string sql);
    }
}