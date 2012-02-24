using System.Collections.Generic;

namespace SampleMigrations
{
    [Migration(2)]
    public class SecondMigration : IMigration
    {
        public IEnumerable<string> Up()
        {
            yield return "CREATE TABLE SecondMigration ( Id int NOT NULL PRIMARY KEY )";
            yield return "INSERT INTO SecondMigration Values (1)";
            yield return "INSERT INTO SecondMigration Values (2)";
            yield return "INSERT INTO SecondMigration Values (3)";
            yield return "INSERT INTO SecondMigration Values (4)";
            yield return "INSERT INTO SecondMigration Values (5)";
        }

        public IEnumerable<string> Down()
        {
            yield return "DROP TABLE [SecondMigration]";
        }
    }
}