using System.Collections.Generic;
using System.Data;

namespace SampleMigrations
{
    [Migration(3)]
    public class MigrationWithConnectionInConstructor : IMigration
    {
        private readonly IDbConnection _connection;

        public MigrationWithConnectionInConstructor(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<string> Up() { yield break; }
        public IEnumerable<string> Down() { yield break; }
    }
}