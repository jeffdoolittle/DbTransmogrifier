using Xunit;

namespace DbTransmogrifier.Specs.using_default_migration_type_source
{
    public class when_retrieving_max_available_version : DefaultMigrationTypeSourceFixture
    {
        [Fact]
        public void then_value_is_non_zero()
        {
            Assert.NotEqual(0, ClassUnderTest.GetMaxAvailableMigrationVersion());
        }
    }
}