using System;
using System.Collections.Generic;

namespace DbTransmogrifier.Migrations
{
    public interface IMigrationResolver
    {
        IDictionary<long, Type> GetMigrationsGreaterThan(long version);
        IDictionary<long, Type> GetMigrationsLessThanOrEqualTo(long version);
    }
}