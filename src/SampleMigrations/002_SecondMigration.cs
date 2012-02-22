using System.Collections.Generic;

namespace SampleMigrations
{
    [Migration(2)]
    public class SecondMigration : IMigration
    {
        public IEnumerable<string> Up()
        {
            yield return "CREATE TABLE SecondMigration ( Id int NOT NULL PRIMARY KEY )";
        }

        public IEnumerable<string> Down()
        {
            yield return "DROP TABLE [SecondMigration]";
        }
    }
}