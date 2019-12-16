using CascadingConfiguration;

namespace CascadingConfiguration.Classes.ConfigSource.Database
{
    public abstract class DbConfigSource<T> : ConfigSource<T> where T : IConfig, new()
    { 
        public IDatabase Database { get; set; }
        public string Table { get; set; }
        public string IdColumn { get; set; }

        protected DbConfigSource(int priority) : base(priority)
        {
        }
    }
}