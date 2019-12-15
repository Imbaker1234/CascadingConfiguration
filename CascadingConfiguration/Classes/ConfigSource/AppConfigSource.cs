using System.Configuration;

namespace CascadingConfiguration
{
    /// <summary>
    /// A relatively simple class that doesn't provide much functionality beyond what
    /// System.Configuration provides. The reason this class is in place is to allow
    /// it to act as a source for ConfigProvider and act in concert with the other
    /// configurations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AppConfigSource<T> : ConfigSource<T> where T : IConfig, new()
    {
        public AppConfigSource(int priority) : base(priority)
        {
        }

        public override void PopulateConfig(IConfigProvider<T> configProvider)
        {
            foreach (var property in typeof(Config).GetProperties())
            {
                configProvider.SetProperty(property, ConfigurationManager.AppSettings[property.Name]);
            }
        }
    }
}