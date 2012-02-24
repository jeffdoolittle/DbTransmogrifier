namespace DbTransmogrifier.Migrations
{
    public interface IMigrationBuilder
    {
        Migration BuildMigration(long version);
    }
}