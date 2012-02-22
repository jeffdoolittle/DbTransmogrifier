namespace DbTransmogrifier.Dialects
{
    public interface ISqlDialect
    {
        string ExtractDatabaseName(string connectionString);
        void ClearAllPools();

        string DatabaseExists { get; }
        string CreateDatabase { get; }
        string DropDatabase { get; }

        string SchemaVersionTableExists { get; }
        string CreateSchemaVersionTable { get; }
        string DropSchemaVersionTable { get; }
    }
}