using System;

namespace DbTransmogrifier.Migrations
{
    public interface IMigrationTypeSource
    {
        Type GetMigrationType(long version);
        long GetMaxAvailableMigrationVersion();
    }
}