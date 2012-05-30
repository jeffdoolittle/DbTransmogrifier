using Xunit;

namespace DbTransmogrifier.Specs.using_default_migration_type_source
{
    public class when_resolving_migration_type_for_version_one : DefaultMigrationTypeSourceFixture
    {
        [Fact]
        public void then_type_is_not_null()
        {
            Assert.NotNull(ClassUnderTest.GetMigrationType(1));
        }

        [Fact]
        public void then_type_is_decorated_with_migration_attribute_containing_matching_version_number()
        {
            var type = ClassUnderTest.GetMigrationType(1);
            AssertTypeDecoratedWithMigrationAttributeAndVersionNumber(type, 1);
        }    
    }
}