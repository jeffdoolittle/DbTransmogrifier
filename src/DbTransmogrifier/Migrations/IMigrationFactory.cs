using System;
using System.Collections.Generic;

namespace DbTransmogrifier.Migrations
{
    public interface IMigrationFactory
    {
        //void RegisterCreationInterceptor(Action<dynamic> interceptor);
        IList<Migration> GetMigrationsGreaterThan(long version);
        IList<Migration> GetMigrationsLessThanOrEqualTo(long version);
    }
}