namespace CascadingConfiguration
{
    public interface IConfigSource<T> where T : IConfig

    {
        int Priority { get; set; }
        void PopulateConfig(IConfigProvider<T> configProvider);
    }
}