using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using DbTransmogrifier.Database;
using DbTransmogrifier.Dialects;
using DbTransmogrifier.Logging;

namespace DbTransmogrifier.Migrations
{
    public class MigrationConfigurer
    {
        private static readonly ILog Log = LoggerFactory.GetLoggerFor(typeof (MigrationConfigurer));

        public static Func<IDictionary<Type, object>, IMigrationBuilder> MigrationBuilderFactory;
        public static Func<IMigrationResolver> MigrationResolverFactory;
        public static Func<string> ProviderNameSource;
        public static Func<string> MasterConnectionStringSource;
        public static Func<string> TargetConnectionStringSource;

        public static MigrationConfigurer ConfigureWithDefaults()
        {
            MigrationBuilderFactory = dependencies => new DefaultMigrationBuilder(dependencies, MigrationResolverFactory());
            MigrationResolverFactory = () => new DefaultMigrationResolver();
            ProviderNameSource = () => ConfigurationManager.AppSettings["ProviderInvariantName"] ?? "System.Data.SqlClient";
            MasterConnectionStringSource = () => ConfigurationManager.ConnectionStrings["Master"].ConnectionString;
            TargetConnectionStringSource = () => ConfigurationManager.ConnectionStrings["Target"].ConnectionString;
            return new MigrationConfigurer();
        }

        public Transmogrifier BuildTransmogrifier()
        {
            var providerName = ProviderNameSource();
            var masterConnectionString = MasterConnectionStringSource();
            var targetConnectionString = TargetConnectionStringSource();
            var dialect = SqlDialectResolver.GetDialect(providerName, targetConnectionString);
            var targetDatabaseName = dialect.ExtractDatabaseName(targetConnectionString);
            var providerFactory = DbProviderFactories.GetFactory(providerName);

            Log.InfoFormat("Building migration configuration");
            Log.InfoFormat("Provider Name: {0}", providerName);
            Log.DebugFormat("Provider Factory: {0}", providerFactory.GetType().Name);
            Log.DebugFormat("Database Dialect: {0} ", dialect.GetType().Name);
            Log.InfoFormat("Database Name: {0}", targetDatabaseName);

            var configuration = new MigrationConfiguration
                       {
                           MigrationBuilderFactory = MigrationBuilderFactory,
                           ConnectionFactory = new ConnectionFactory(providerFactory, masterConnectionString, targetConnectionString),
                           Dialect = dialect,
                           DatabaseName = dialect.ExtractDatabaseName(targetConnectionString)
                       };

            return new Transmogrifier(configuration);
        }

        private class MigrationConfiguration : IMigrationConfiguration
        {
            public Func<IDictionary<Type, object>, IMigrationBuilder> MigrationBuilderFactory { get; set; }
            public IConnectionFactory ConnectionFactory { get; set; }
            public ISqlDialect Dialect { get; set; }
            public string DatabaseName { get; set; }
        }
    }
}