using System.Collections.Generic;

namespace SampleMigrations
{
    [Migration(9)]
    public class MigrationInheritingFromImplementingSubClassWithUpOnlyMigration : Migration
    {
        public override IEnumerable<string> Up()
        {
            yield break;
        }
    }
}