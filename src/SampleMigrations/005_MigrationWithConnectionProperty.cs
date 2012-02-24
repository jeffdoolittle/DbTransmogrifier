using System.Collections.Generic;
using System.Data;

namespace SampleMigrations
{
    [Migration(5)]
    public class MigrationWithConnectionProperty : IMigration
    {
        public IEnumerable<string> Up() { yield break; }
        public IEnumerable<string> Down() { yield break; }

        public IDbConnection Connection { protected get; set; }
    }
}