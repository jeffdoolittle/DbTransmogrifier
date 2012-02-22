using System;
using System.Collections.Generic;

namespace SampleMigrations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MigrationAttribute : Attribute
    {
        public MigrationAttribute(int version)
        {
            Version = version;
        }

        public int Version { get; private set; }
    }

    public interface IMigration
    {
        IEnumerable<string> Up();
        IEnumerable<string> Down();
    }

    [Migration(1)]
    public class FirstMigration : IMigration
    {
        public IEnumerable<string> Up()
        {
            yield return "CREATE TABLE FirstMigration (	Id int NOT NULL PRIMARY KEY	)";
        }

        public IEnumerable<string> Down()
        {
            yield return "DROP TABLE [FirstMigration]";
        }
    }
}
