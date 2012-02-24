using DbTransmogrifier.Logging;
using DbTransmogrifier.Migrations;

namespace DbTransmogrifier
{
    class Program
    {
        static void Main()
        {
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
