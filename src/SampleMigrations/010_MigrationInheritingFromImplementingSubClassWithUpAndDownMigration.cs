using System.Collections.Generic;

namespace SampleMigrations
{
    [Migration(10)]
    public class MigrationInheritingFromImplementingSubClassWithUpAndDownMigration : Migration
    {
        public override IEnumerable<string> Up()
        {
            yield break;
        }

        public override IEnumerable<string> Down()
        {
            yield break;
        }
    }
}