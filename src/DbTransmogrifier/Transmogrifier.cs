using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using DbTransmogrifier.Config;
using DbTransmogrifier.Database;
using DbTransmogrifier.Dialects;
using DbTransmogrifier.Logging;
using DbTransmogrifier.Migrations;

namespace DbTransmogrifier
{
    public class Transmogrifier
    {
        private readonly IConfigurator _configurator;
        private readonly IMigrationFactory _migrationFactory;
        private static readonly ILog Log = LoggerFactory.GetLoggerFor(typeof(Transmogrifier));
        private readonly string _providerName;
        private readonly ISqlDialect _dialect;
        private readonly string _targetDatabaseName;
        private readonly IConnectionFactory _connectionFactory;

        public Transmogrifier()
            : this(new DefaultConfigurator(), new DefaultMigrationFactory(), null, null)
        {
        }

        public Transmogrifier(IConfigurator configurator, IMigrationFactory migrationFactory, ISqlDialect sqlDialect, IConnectionFactory connectionFactory)
        {
            _configurator = configurator;
            _migrationFactory = migrationFactory;
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

        public void UpToLatest()
        {
            UpTo(int.MaxValue);
        }

        public void UpTo(int version)
        {
            using (var masterConnection = _connectionFactory.OpenMaster())
            {
                if (!DatabaseExists(masterConnection))
                {
                    Log.ErrorFormat("Database {0} does not exist.  You must initialize the database before applying migrations.", _targetDatabaseName);
                    return;
                }
            }

            using (var targetConnection = _connectionFactory.OpenTarget())
            using (var transaction = targetConnection.BeginTransaction())
            {
                var currentVersion = GetCurrentVersion(targetConnection, transaction);
                var migrations = _migrationFactory.GetMigrationsGreaterThan(currentVersion)
                    .Where(x => x.Version <= version);

                if (migrations.Count() == 0) Log.Info("No migrations to apply. Database is current.");

                foreach (var migration in migrations)
                {
                    foreach (var script in migration.Up) targetConnection.Execute(script, transaction);

                    var versionCommand = targetConnection.CreateCommand(_dialect.InsertSchemaVersion, migration.Version);
                    versionCommand.Transaction = transaction;
                    versionCommand.ExecuteNonQuery();

                    Log.InfoFormat("Applied up migration {0} - {1}", migration.Version, migration.Name);
                }

                transaction.Commit();
            }

            _dialect.ClearAllPools();
        }

        public void DownTo(int version)
        {
            using (var masterConnection = _connectionFactory.OpenMaster())
            {
                if (!DatabaseExists(masterConnection))
                {
                    Log.ErrorFormat("Database {0} does not exist.  You must initialize the database before applying migrations.", _targetDatabaseName);
                    return;
                }
            }

            using (var targetConnection = _connectionFactory.OpenTarget())
            using (var transaction = targetConnection.BeginTransaction())
            {
                var currentVersion = GetCurrentVersion(targetConnection, transaction);
                var migrations = _migrationFactory.GetMigrationsLessThanOrEqualTo(currentVersion)
                    .Where(x => x.Version > version);

                if (migrations.Count() == 0) Log.Info("No migrations to apply. Database is current.");

                foreach (var migration in migrations)
                {
                    var versionCommand = targetConnection.CreateCommand(_dialect.DeleteSchemaVersion, migration.Version);
                    versionCommand.Transaction = transaction;
                    versionCommand.ExecuteNonQuery();

                    foreach (var script in migration.Down) targetConnection.Execute(script, transaction);

                    Log.InfoFormat("Applied down migration {0} - {1}", migration.Version, migration.Name);
                }

                transaction.Commit();
            }

            _dialect.ClearAllPools();
        }

        public int CurrentVersion
        {
            get
            {
                try
                {
                    using (var targetConnection = _connectionFactory.OpenTarget())
                    {
                        return GetCurrentVersion(targetConnection);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return -1;
                }
            }
        }

        private int GetCurrentVersion(IDbConnection connection, IDbTransaction transaction = null)
        {
            var command = connection.CreateCommand(_dialect.CurrentVersion);
            if (transaction != null) command.Transaction = transaction;
            return (int)command.ExecuteScalar();
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