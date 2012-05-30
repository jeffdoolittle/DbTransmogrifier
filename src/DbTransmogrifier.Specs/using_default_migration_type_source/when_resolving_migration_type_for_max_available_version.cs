using Xunit;

namespace DbTransmogrifier.Specs.using_default_migration_type_source
{
    public class when_resolving_migration_type_for_max_available_version : DefaultMigrationTypeSourceFixture
    {
        [Fact]
        public void then_type_is_null()
        {
            var version = ClassUnderTest.GetMaxAvailableMigrationVersion();
            var type = ClassUnderTest.GetMigrationType(version);
            AssertTypeDecoratedWithMigrationAttributeAndVersionNumber(type, version);
        }
    }
}