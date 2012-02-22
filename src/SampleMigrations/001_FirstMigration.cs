using System.Collections.Generic;

namespace SampleMigrations
{
    [Migration(1)]
    public class FirstMigration : IMigration
    {
        public IEnumerable<string> Up()
        {
            yield return "CREATE TABLE FirstMigration ( Id int NOT NULL PRIMARY KEY )";
        }

        public IEnumerable<string> Down()
        {
            yield return "DROP TABLE [FirstMigration]";
        }
    }
}
