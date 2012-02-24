using System.Collections.Generic;
using System.Data;

namespace SampleMigrations
{
    [Migration(4)]
    public class MigrationWithConnectionAndTransactionInConstructor : IMigration
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public MigrationWithConnectionAndTransactionInConstructor(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public IEnumerable<string> Up()
        {
            var scripts = new List<string>();
            using (var command = _connection.CreateCommand())
            {
                command.Transaction = _transaction;
                command.CommandText = "SELECT Id FROM SecondMigration";

                using (var reader = command.ExecuteReader())
                {
                    while(reader.Read()) scripts.Add(string.Format("INSERT INTO FirstMigration VALUES({0})", reader.GetInt32(0)));
                }
            }

            return scripts;
        }

        public IEnumerable<string> Down() { yield return "DELETE FROM FirstMigration"; }
    }
}