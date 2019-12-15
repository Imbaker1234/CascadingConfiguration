using System.Data.SqlClient;

namespace CascadingConfiguration
{
    public abstract class DBConfigSource<T> : ConfigSource<T> where T : IConfig, new()
    { 
        public IDatabase Database { get; set; }
        public string Table { get; set; }
        public string IdColumn { get; set; }

        /// <summary>
        /// Sets the properties for the IConfig held in this class. If the 
        /// </summary>
        /// <returns></returns>
        public abstract override void PopulateConfig(IConfigProvider<T> configProvider);

        protected DBConfigSource(int priority = 2) : base(priority)
        {
            Priority = priority;
        }
    }
}