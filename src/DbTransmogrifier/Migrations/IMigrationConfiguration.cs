using System;
using System.Collections.Generic;
using DbTransmogrifier.Database;
using DbTransmogrifier.Dialects;

namespace DbTransmogrifier.Migrations
{
    public interface IMigrationConfiguration
    {
        Func<IDictionary<Type, object>, IMigrationBuilder> MigrationBuilderFactory { get; }
        IConnectionFactory ConnectionFactory { get; }
        ISqlDialect Dialect { get; }
        string DatabaseName { get; }
        long MaxAvailableMigrationVersion { get; }
    }
}