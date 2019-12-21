using CascadingConfiguration;

namespace CascadingConfiguration.Classes.ConfigSource.Database
{
    public abstract class DbConfigSource<T> : ConfigSource<T> where T : IConfig, new()
    { 
        public Database Database { get; set; }
        public string Table { get; set; }
        public string IdColumn { get; set; }

        protected DbConfigSource(string connectionString, string table, int priority) : base(priority)
        {
            Database = new Database(connectionString);
            Table = table;
        }
    }
}