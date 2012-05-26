using System.Data.Common;

namespace DbTransmogrifier.Dialects
{
    public class PostgreSqlDialect : ISqlDialect
    {
        public string ExtractDatabaseName(string connectionString)
        {
            var providerFactory = DbProviderFactories.GetFactory("Npgsql");
            var builder = providerFactory.CreateConnectionStringBuilder();
            builder.ConnectionString = connectionString;
            var databaseName = builder["Database"] as string ?? "";
            return databaseName;
        }

        public void ClearAllPools()
        {
        }

        public string CurrentVersion
        {
            get { return PostgreSqlStatements.CurrentVersion; }
        }

        public string DatabaseExists
        {
            get { return PostgreSqlStatements.DatabaseExists; }
        }

        public string CreateDatabase
        {
            get { return PostgreSqlStatements.CreateDatabase; }
        }

        public string DropDatabase
        {
            get { return PostgreSqlStatements.DropDatabase; }
        }

        public string SchemaVersionTableExists
        {
            get { return PostgreSqlStatements.SchemaVersionTableExists; }
        }

        public string CreateSchemaVersionTable
        {
            get { return PostgreSqlStatements.CreateSchemaVersionTable; }
        }

        public string DropSchemaVersionTable
        {
            get { return PostgreSqlStatements.DropSchemaVersionTable; }
        }

        public string InsertSchemaVersion
        {
            get { return PostgreSqlStatements.InsertSchemaVersion; }
        }

        public string DeleteSchemaVersion
        {
            get { return PostgreSqlStatements.DeleteSchemaVersion; }
        }

        public string TearDown
        {
            get { return PostgreSqlStatements.TearDown; }
        }
    }
}