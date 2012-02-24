using System.Collections.Generic;

namespace DbTransmogrifier.Migrations
{
    public interface IMigrationBuilder
    {
        IList<Migration> BuildMigrationsGreaterThan(long version);
        IList<Migration> BuildMigrationsLessThanOrEqualTo(long version);
    }
}