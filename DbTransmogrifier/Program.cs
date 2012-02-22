using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using DbTransmogrifier.Dialects;

namespace DbTransmogrifier
{
    class Program
    {
        static void Main(string[] args)
        {
            new Transmogrifier();
        }
    }

    public class Transmogrifier
    {
        private static readonly ILog Log = LoggerFactory.GetLoggerFor(typeof (Transmogrifier));
        private readonly string _providerName;
        private readonly ISqlDialect _dialect;

        public Transmogrifier()
        {
            _providerName = ConfigurationManager.AppSettings["ProviderInvariantName"] ?? "System.Data.SqlClient";
            var providerFactory = DbProviderFactories.GetFactory(_providerName);

            Log.InfoFormat("Using {0} provider", providerFactory.GetType());

            _dialect = GetDialect();

            var masterConnectionSettings = ConfigurationManager.ConnectionStrings["Master"];
            var targetConnectionSettings = ConfigurationManager.ConnectionStrings["Target"];

            using (var masterConnection = providerFactory.CreateConnection())
            {
                masterConnection.ConnectionString = masterConnectionSettings.ConnectionString;
                masterConnection.Open();

                var builder = providerFactory.CreateConnectionStringBuilder();
                builder.ConnectionString = targetConnectionSettings.ConnectionString;
                var databaseName = builder["Initial Catalog"] as string ?? "";
                EnsureTargetDatabase(masterConnection, databaseName);
            }

            var targetConnection = providerFactory.CreateConnection();
            targetConnection.ConnectionString = targetConnectionSettings.ConnectionString;
            targetConnection.Open();
        }

        private void EnsureTargetDatabase(IDbConnection connection, string databaseName)
        {
            bool exists;
            using(var command = connection.CreateCommand(_dialect.DatabaseExists, databaseName))
            {
                exists = (bool)command.ExecuteScalar();
            }

            if (exists)
            {
                Log.InfoFormat("Database {0} already exists.", databaseName);
                return;
            }

            using (var createCommand = connection.CreateCommand(string.Format(_dialect.CreateDatabase, databaseName)))
            {
                createCommand.ExecuteNonQuery();
                Log.InfoFormat("Database {0} created.", databaseName);
            }
        }

        private ISqlDialect GetDialect()
        {
            if (_dialect != null)
                return _dialect;

            var providerName = _providerName.ToUpperInvariant();

            if (providerName.Contains("POSTGRES"))
                return new PostgreSqlDialect();

            return new MsSqlDialect();
        }
    }
}
