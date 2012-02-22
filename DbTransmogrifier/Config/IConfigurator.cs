namespace DbTransmogrifier.Config
{
    public interface IConfigurator
    {
        string ProviderName { get; }
        string MasterConnectionString { get; }
        string TargetConnectionString { get; }
    }
}