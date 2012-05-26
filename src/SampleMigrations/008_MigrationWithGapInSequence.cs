using System.Collections.Generic;

namespace SampleMigrations
{
    [Migration(8)]
    public class MigrationWithGapInSequence : IMigration
    {
        public IEnumerable<string> Up()
        {
            yield break;
        }

        public IEnumerable<string> Down()
        {
            yield break;
        }
    }
}