using CascadingConfiguration;
using SQLxt;

namespace CascadingConfiguration.Classes.ConfigSource.Database
{
    public abstract class DbConfigSource<T> : ConfigSource<T> where T : IConfig, new()
    { 
        public IDbOperator DbOperator { get; set; }
        public string Table { get; set; }
        public string IdColumn { get; set; }

        protected DbConfigSource(string connectionString, string table, int priority) : base(priority)
        {
            DbOperator = new DbOperator(connectionString);
            Table = table;
        }
    }
}