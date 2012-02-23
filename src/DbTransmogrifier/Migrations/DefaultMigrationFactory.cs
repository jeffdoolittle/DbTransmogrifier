using System;
using System.Collections.Generic;
using System.Linq;

namespace DbTransmogrifier.Migrations
{
    public class DefaultMigrationFactory : IMigrationFactory
    {
        private readonly IMigrationResolver _migrationResolver;

        public DefaultMigrationFactory() : this(new DefaultMigrationResolver())
        {            
        }

        public DefaultMigrationFactory(IMigrationResolver migrationResolver)
        {
            _migrationResolver = migrationResolver;
        }

        //public void RegisterCreationInterceptor(Action<dynamic> interceptor)
        //{
        //    throw new NotImplementedException();
        //}

        public IList<Migration> GetMigrationsGreaterThan(long version)
        {
            var migrationTypes = _migrationResolver.GetMigrationsGreaterThan(version);
            return Build(migrationTypes).ToList().AsReadOnly();
        }

        public IList<Migration> GetMigrationsLessThanOrEqualTo(long version)
        {
            var migrationTypes = _migrationResolver.GetMigrationsLessThanOrEqualTo(version);
            return Build(migrationTypes).ToList().AsReadOnly();
        }

        private IEnumerable<Migration> Build(IDictionary<long, Type> migrationTypes)
        {
            foreach (var kv in migrationTypes)
            {
                dynamic migration = Activator.CreateInstance(kv.Value);
                IEnumerable<string> up = migration.Up();
                IEnumerable<string> down = migration.Down();
                yield return new Migration(kv.Key, kv.Value.Name, up, down);
            }
        }
    }
}