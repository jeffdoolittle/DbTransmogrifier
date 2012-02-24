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

        public IEnumerable<string> Up() { yield break; }
        public IEnumerable<string> Down() { yield break; }
    }
}