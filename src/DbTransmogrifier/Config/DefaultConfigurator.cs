using System.Configuration;

namespace DbTransmogrifier.Config
{
    public class DefaultConfigurator : IConfigurator
    {
        public string ProviderName
        {
            get { return ConfigurationManager.AppSettings["ProviderInvariantName"] ?? "System.Data.SqlClient"; }
        }

        public string MasterConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Master"].ConnectionString; }
        }

        public string TargetConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Target"].ConnectionString; }
        }
    }
}