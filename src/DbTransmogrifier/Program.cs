using DbTransmogrifier.Logging;
using DbTransmogrifier.Migrations;
using System.Data.Common;

namespace DbTransmogrifier
{
    class Program
    {
        static void Main()
        {
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);

            LogConfigurer.Configure();

            var transmogrifier = MigrationConfigurer
                .ConfigureWithDefaults()
                .BuildTransmogrifier();

            new Processor(transmogrifier).Process();
        }
    }

    public enum ExitCodes
    {
        Success = 0,
        Error = 1,
        Fault = 2
    }
}
