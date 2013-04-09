using System;
using System.Linq;
using DbTransmogrifier.Migrations;
using Xunit;

namespace DbTransmogrifier.Specs.using_default_migration_type_source
{
    public abstract class DefaultMigrationTypeSourceFixture
    {
        protected DefaultMigrationTypeSource ClassUnderTest;

        protected DefaultMigrationTypeSourceFixture()
        {
            ClassUnderTest = new DefaultMigrationTypeSource(x => x.FullName.Contains("SampleMigrations"));
        }

        protected void AssertTypeDecoratedWithMigrationAttributeAndVersionNumber(Type type, long expectedVersion)
        {
            dynamic attribute = type.GetCustomAttributes(true).FirstOrDefault();
            Assert.NotNull(attribute);
            var version = attribute.Version;
            Assert.Equal(expectedVersion, version);
        }
    }
}
