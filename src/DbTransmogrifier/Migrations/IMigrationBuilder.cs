namespace DbTransmogrifier.Migrations
{
    public interface IMigrationBuilder
    {
        Migration BuildUpMigration(long version);
        Migration BuildDownMigration(long version);
    }
}