using System;

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
}