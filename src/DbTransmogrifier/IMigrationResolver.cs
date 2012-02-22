using System.Collections.Generic;

namespace DbTransmogrifier
{
    public interface IMigrationResolver
    {
        IList<MigrationDescriptor> GetMigrationsGreaterThan(int version);
        IList<MigrationDescriptor> GetMigrationsLessThanOrEqualTo(int version);
    }
}