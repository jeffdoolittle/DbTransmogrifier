using System;

namespace DbTransmogrifier.Dialects
{
    public class PostgreSqlDialect : ISqlDialect
    {
        public string ExtractDatabaseName(string connectionString)
        {
            throw new NotImplementedException();
        }

        public void ClearAllPools()
        {
            throw new NotImplementedException();
        }

        public string CurrentVersion
        {
            get { throw new NotImplementedException(); }
        }

        public string DatabaseExists
        {
            get { throw new NotImplementedException(); }
        }

        public string CreateDatabase
        {
            get { throw new NotImplementedException(); }
        }

        public string DropDatabase
        {
            get { throw new NotImplementedException(); }
        }

        public string SchemaVersionTableExists
        {
            get { throw new NotImplementedException(); }
        }

        public string CreateSchemaVersionTable
        {
            get { throw new NotImplementedException(); }
        }

        public string DropSchemaVersionTable
        {
            get { throw new NotImplementedException(); }
        }

        public string InsertSchemaVersion
        {
            get { throw new NotImplementedException(); }
        }

        public string DeleteSchemaVersion
        {
            get { throw new NotImplementedException(); }
        }
    }
}