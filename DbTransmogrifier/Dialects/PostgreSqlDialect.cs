using System;

namespace DbTransmogrifier.Dialects
{
    public class PostgreSqlDialect : ISqlDialect
    {
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
    }
}