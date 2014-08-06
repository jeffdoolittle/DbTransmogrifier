using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace DbTransmogrifier.Dialects
{
    public class MsSqlDialect : ISqlDialect
    {
        public string ExtractDatabaseName(string connectionString)
        {
            var providerFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            var builder = providerFactory.CreateConnectionStringBuilder();
            builder.ConnectionString = connectionString;
            var databaseName = builder["Initial Catalog"] as string ?? "";
            return databaseName;
        }

        public void ClearAllPools()
        {
            SqlConnection.ClearAllPools();
        }

        public string DropAllConnections
        {
            get { return MsSqlStatements.DropAllConnections; }
        }

        public string CurrentVersion
        {
            get { return MsSqlStatements.CurrentVersion; }
        }

        public string DatabaseExists
        {
            get { return MsSqlStatements.DatabaseExists; }
        }

        public string CreateDatabase
        {
            get { return MsSqlStatements.CreateDatabase; }
        }

        public string DropDatabase
        {
            get { return MsSqlStatements.DropDatabase; }
        }

        public string SchemaVersionTableExists
        {
            get { return MsSqlStatements.SchemaVersionTableExists; }
        }

        public string CreateSchemaVersionTable
        {
            get { return MsSqlStatements.CreateSchemaVersionTable; }
        }

        public string DropSchemaVersionTable
        {
            get { return MsSqlStatements.DropSchemaVersionTable; }
        }

        public string InsertSchemaVersion
        {
            get { return MsSqlStatements.InsertSchemaVersion; }
        }

        public string DeleteSchemaVersion
        {
            get { return MsSqlStatements.DeleteSchemaVersion; }
        }

        public string TearDown
        {
            get { return MsSqlStatements.TearDown; }
        }
    }
}