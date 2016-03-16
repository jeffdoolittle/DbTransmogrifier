using System;
using System.Collections.Generic;

namespace SampleMigrations
{
    public interface IMigration
    {
        IEnumerable<string> Up();
        IEnumerable<string> Down();
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MigrationAttribute : Attribute
    {
        public MigrationAttribute(int version)
        {
            Version = version;
        }

        public int Version { get; private set; }
    }

    public abstract class Migration : IMigration
    {
        public abstract IEnumerable<string> Up();

        public virtual IEnumerable<string> Down()
        {
            yield break;
        }
    }
}