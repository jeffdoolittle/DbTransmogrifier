using System.Collections.Generic;
using System.Data;

namespace SampleMigrations
{
    [Migration(6)]
    public class MigrationWithConnectionAndTransactionProperties : IMigration
    {
        public IEnumerable<string> Up() { yield break; }
        public IEnumerable<string> Down() { yield break; }

        public IDbConnection Connection { protected get; set; }
        public IDbTransaction Transaction { protected get; set; }
    }
}