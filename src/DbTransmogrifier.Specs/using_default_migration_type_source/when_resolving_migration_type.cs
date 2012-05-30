using Xunit;

namespace DbTransmogrifier.Specs.using_default_migration_type_source
{
    public class when_resolving_migration_type_for_version_zero : DefaultMigrationTypeSourceFixture
    {
        [Fact]
        public void then_type_is_null()
        {
            Assert.Null(ClassUnderTest.GetMigrationType(0));
        }    
    }
}