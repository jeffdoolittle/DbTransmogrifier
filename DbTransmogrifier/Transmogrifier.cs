using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DbTransmogrifier.Config;
using DbTransmogrifier.Dialects;
using DbTransmogrifier.Logging;

namespace DbTransmogrifier
{
    public class Transmogrifier
    {
        private readonly IConfigurator _configurator;
        private static readonly ILog Log = LoggerFactory.GetLoggerFor(typeof(Transmogrifier));
        private readonly string _providerName;
        private readonly ISqlDialect _dialect;
        private readonly string _targetDatabaseName;
        private readonly IConnectionFactory _connectionFactory;

        public Transmogrifier()
            : this(new DefaultConfigurator(), null, null)
        {
        }

        public Transmogrifier(IConfigurator configurator, ISqlDialect sqlDialect, IConnectionFactory connectionFactory)
        {
            _configurator = configurator;
            _providerName = configurator.ProviderName;
            Log.InfoFormat("Using {0} provider", _providerName);
            _dialect = sqlDialect ?? GetDialect(configurator.ProviderName, _configurator.TargetConnectionString);
            Log.InfoFormat("Using {0} dialect", _dialect.GetType().Name);
            _targetDatabaseName = _dialect.ExtractDatabaseName(_configurator.TargetConnectionString);
            Log.InfoFormat("Target Database: {0}", _targetDatabaseName);
            var providerFactory = DbProviderFactories.GetFactory(_providerName);
            _connectionFactory = connectionFactory ?? new ConnectionFactory(providerFactory, configurator.MasterConnectionString, configurator.TargetConnectionString);
        }

        public void Init()
        {
            using (var masterConnection = _connectionFactory.OpenMaster())
            {
                EnsureTargetDatabase(masterConnection);
            }

            using (var targetConnection = _connectionFactory.OpenTarget())
            {
                EnsureSchemaVersionTable(targetConnection);
            }

            _dialect.ClearAllPools();
        }

        public void TearDown()
        {
            using (var masterConnection = _connectionFactory.OpenMaster())
            {
                DropTargetDatabase(masterConnection);
            }

            _dialect.ClearAllPools();
        }

        private void EnsureSchemaVersionTable(IDbConnection targetConnection)
        {
            if (SchemaVersionTableExists(targetConnection))
            {
                Log.Info("Schema Version Table already exists.");
                return;
            } 

            using (var createCommand = targetConnection.CreateCommand(_dialect.CreateSchemaVersionTable))
            {
                createCommand.ExecuteNonQuery();
                Log.Info("Schema Version Table created.");
            }
        }

        private bool DatabaseExists(IDbConnection connection)
        {
            using (var command = connection.CreateCommand(_dialect.DatabaseExists, _targetDatabaseName))
            {
                return (bool)command.ExecuteScalar();
            }
        }

        private bool SchemaVersionTableExists(IDbConnection connection)
        {
            using (var command = connection.CreateCommand(_dialect.SchemaVersionTableExists))
            {
                return (bool)command.ExecuteScalar();
            }
        }

        private void EnsureTargetDatabase(IDbConnection connection)
        {
            if (DatabaseExists(connection))
            {
                Log.InfoFormat("Database {0} already exists.", _targetDatabaseName);
                return;
            }

            using (var createCommand = connection.CreateCommand(string.Format(_dialect.CreateDatabase, _targetDatabaseName)))
            {
                createCommand.ExecuteNonQuery();
                Log.InfoFormat("Database {0} created.", _targetDatabaseName);
            }
        }

        private void DropTargetDatabase(IDbConnection connection)
        {
            if (!DatabaseExists(connection))
            {
                Log.InfoFormat("Database {0} already dropped.", _targetDatabaseName);
                return;
            }

            using (var createCommand = connection.CreateCommand(string.Format(_dialect.DropDatabase, _targetDatabaseName)))
            {
                createCommand.ExecuteNonQuery();
                Log.InfoFormat("Database {0} dropped.", _targetDatabaseName);
            }
        }

        private static ISqlDialect GetDialect(string providerName, string connectionString)
        {
            //if (providerName.Contains("MYSQL"))
            //    return new MySqlDialect();

            //if (providerName.Contains("SQLITE"))
            //    return new SqliteDialect();

            //if (providerName.Contains("SQLSERVERCE") || connectionString.Contains(".SDF"))
            //    return new SqlCeDialect();

            if (providerName.Contains("POSTGRES") || providerName.Contains("NPGSQL"))
                return new PostgreSqlDialect();

            //if (providerName.Contains("FIREBIRD"))
            //    return new FirebirdSqlDialect();

            //if (providerName.Contains("OLEDB") && connectionString.Contains("MICROSOFT.JET"))
            //    return new AccessDialect();

            return new MsSqlDialect();
        }
    }
}