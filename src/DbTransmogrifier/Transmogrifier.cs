using System;
using System.Collections.Generic;
using System.Data;
using DbTransmogrifier.Database;
using DbTransmogrifier.Dialects;
using DbTransmogrifier.Logging;
using DbTransmogrifier.Migrations;

namespace DbTransmogrifier
{
    public class Transmogrifier
    {
        private static readonly ILog Log = LoggerFactory.GetLoggerFor(typeof(Transmogrifier));
        private readonly Func<IDictionary<Type, object>, IMigrationBuilder> _migrationBuilderFactory;
        private readonly ISqlDialect _dialect;
        private readonly IConnectionFactory _connectionFactory;
        private readonly string _databaseName;

        public Transmogrifier(IMigrationConfiguration configuration)
        {
            _migrationBuilderFactory = configuration.MigrationBuilderFactory;
            _dialect = configuration.Dialect;
            _connectionFactory = configuration.ConnectionFactory;
            _databaseName = configuration.DatabaseName;
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
            UpTo(long.MaxValue);
        }

        public void UpTo(long version)
        {
            var currentVersion = CurrentVersion;
            if (currentVersion < 0) return;

            using (var targetConnection = _connectionFactory.OpenTarget())
            using (var transaction = targetConnection.BeginTransaction())
            {
                var dependencies = new Dictionary<Type, object>
                                       {
                                           {typeof (IDbConnection), targetConnection},
                                           {typeof (IDbTransaction), transaction}
                                       };

                var builder = _migrationBuilderFactory(dependencies);

                var appliedMigrations = false;

                for (long m = currentVersion + 1; m <= version; m++)
                {
                    var migration = builder.BuildMigration(m);
                    if (migration == null) break;

                    Log.DebugFormat("Applying up migration {0} - {1}", migration.Version, migration.Name);

                    foreach (var script in migration.Up)
                    {
                        targetConnection.Execute(script, transaction);
                        Log.DebugFormat("Executed script - {0}", script);
                    }

                    var versionCommand = targetConnection.CreateCommand(_dialect.InsertSchemaVersion, migration.Version);
                    versionCommand.Transaction = transaction;
                    versionCommand.ExecuteNonQuery();

                    Log.InfoFormat("Applied up migration {0} - {1}", migration.Version, migration.Name);

                    appliedMigrations = true;
                }

                if (!appliedMigrations) Log.InfoFormat("No migrations to apply. Database is current at version {0}.", currentVersion);
                else Log.InfoFormat("Database is now at version {0}.", GetCurrentVersion(targetConnection, transaction));

                transaction.Commit();
            }

            _dialect.ClearAllPools();
        }

        public void DownTo(long version)
        {
            var currentVersion = CurrentVersion;
            if (currentVersion < 0) return;

            using (var targetConnection = _connectionFactory.OpenTarget())
            using (var transaction = targetConnection.BeginTransaction())
            {
                var dependencies = new Dictionary<Type, object>
                                       {
                                           {typeof (IDbConnection), targetConnection},
                                           {typeof (IDbTransaction), transaction}
                                       };

                var builder = _migrationBuilderFactory(dependencies);

                var appliedMigrations = false;

                for (long m = currentVersion; m > version; m--)
                {
                    var migration = builder.BuildMigration(m);
                    if (migration == null) break;

                    Log.DebugFormat("Applying down migration {0} - {1}", migration.Version, migration.Name);

                    foreach (var script in migration.Down)
                    {
                        targetConnection.Execute(script, transaction);
                        Log.DebugFormat("Executed script - {0}", script);
                    }

                    var versionCommand = targetConnection.CreateCommand(_dialect.DeleteSchemaVersion, migration.Version);
                    versionCommand.Transaction = transaction;
                    versionCommand.ExecuteNonQuery();

                    Log.InfoFormat("Applied down migration {0} - {1}", migration.Version, migration.Name);

                    appliedMigrations = true;
                }

                if (!appliedMigrations) Log.InfoFormat("No migrations to apply. Database is current at version {0}.", currentVersion);
                else Log.InfoFormat("Database is now at version {0}.", GetCurrentVersion(targetConnection, transaction));

                transaction.Commit();
            }

            _dialect.ClearAllPools();
        }

        private bool CannotRunMigrations()
        {
            using (var masterConnection = _connectionFactory.OpenMaster())
            {
                if (!DatabaseExists(masterConnection))
                {
                    Log.ErrorFormat("Database {0} does not exist.  You must initialize the database.", _databaseName);
                    return true;
                }
            }
            return false;
        }

        public long CurrentVersion
        {
            get
            {
                if (CannotRunMigrations()) return -1;

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

        private long GetCurrentVersion(IDbConnection connection, IDbTransaction transaction = null)
        {
            var command = connection.CreateCommand(_dialect.CurrentVersion);
            if (transaction != null) command.Transaction = transaction;
            return (long)command.ExecuteScalar();
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
            using (var command = connection.CreateCommand(_dialect.DatabaseExists, _databaseName))
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
                Log.InfoFormat("Database {0} already exists.", _databaseName);
                return;
            }

            using (var createCommand = connection.CreateCommand(string.Format(_dialect.CreateDatabase, _databaseName)))
            {
                createCommand.ExecuteNonQuery();
                Log.InfoFormat("Database {0} created.", _databaseName);
            }
        }

        private void DropTargetDatabase(IDbConnection connection)
        {
            if (!DatabaseExists(connection))
            {
                Log.InfoFormat("Database {0} already dropped.", _databaseName);
                return;
            }

            using (var createCommand = connection.CreateCommand(string.Format(_dialect.DropDatabase, _databaseName)))
            {
                createCommand.ExecuteNonQuery();
                Log.InfoFormat("Database {0} dropped.", _databaseName);
            }
        }
    }
}