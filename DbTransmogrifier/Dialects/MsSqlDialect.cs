namespace DbTransmogrifier.Dialects
{
    public class MsSqlDialect : ISqlDialect
    {
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
    }
}