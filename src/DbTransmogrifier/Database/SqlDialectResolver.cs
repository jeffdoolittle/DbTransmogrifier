using DbTransmogrifier.Dialects;

namespace DbTransmogrifier.Database
{
    public class SqlDialectResolver
    {
        public static ISqlDialect GetDialect(string providerName, string connectionString)
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