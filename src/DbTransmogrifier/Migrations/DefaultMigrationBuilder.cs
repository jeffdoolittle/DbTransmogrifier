using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbTransmogrifier.Migrations
{
    public class DefaultMigrationBuilder : IMigrationBuilder
    {
        private readonly IDictionary<Type, object> _dependencies;
        private readonly IMigrationResolver _migrationResolver;

        public DefaultMigrationBuilder(IDictionary<Type, object> dependencies, IMigrationResolver migrationResolver)
        {
            _dependencies = dependencies;
            _migrationResolver = migrationResolver;
        }

        public IList<Migration> BuildMigrationsGreaterThan(long version)
        {
            var migrationTypes = _migrationResolver.GetMigrationsGreaterThan(version);
            var migrations = Build(migrationTypes).ToList().AsReadOnly();
            return migrations;
        }

        public IList<Migration> BuildMigrationsLessThanOrEqualTo(long version)
        {
            var migrationTypes = _migrationResolver.GetMigrationsLessThanOrEqualTo(version);
            var migrations = Build(migrationTypes).ToList().AsReadOnly();
            return migrations;
        }

        private IEnumerable<Migration> Build(IDictionary<long, Type> migrationTypes)
        {
            if (_dependencies.Count == 0) return SimpleBuild(migrationTypes);
            else return BuildWithDependencies(migrationTypes);
        }

        private IEnumerable<Migration> SimpleBuild(IDictionary<long, Type> migrationTypes)
        {
            foreach (var kv in migrationTypes)
            {
                dynamic migration = Activator.CreateInstance(kv.Value);
                IEnumerable<string> up = migration.Up();
                IEnumerable<string> down = migration.Down();
                yield return new Migration(kv.Key, kv.Value.Name, up, down);
            }
        }

        private IEnumerable<Migration> BuildWithDependencies(IDictionary<long, Type> migrationTypes)
        {
            foreach (var kv in migrationTypes)
            {
                var type = kv.Value;
                var constructor = type.GetConstructors()
                    .GroupBy(x => x)
                    .Select(x => new { Constructor = x.Key, ParameterCount = x.Sum(y => y.GetParameters().Count()) })
                    .OrderByDescending(x => x.ParameterCount)
                    .First().Constructor;

                var parameters = new List<object>();
                var parameterInfos = constructor.GetParameters();

                foreach (var parameterInfo in parameterInfos)
                {
                    parameters.Add(_dependencies[parameterInfo.ParameterType]);
                }

                dynamic migration = constructor.Invoke(parameters.ToArray());

                var properties = type.GetProperties()
                    .Where(x => x.PropertyType == typeof(IDbConnection) || x.PropertyType == typeof(IDbTransaction))                    
                    .Where(x=>x.CanWrite)
                    .ToList();

                foreach (var property in properties)
                {
                    property.SetValue(migration, _dependencies[property.PropertyType], new object[0]);
                }
                
                IEnumerable<string> up = migration.Up();
                IEnumerable<string> down = migration.Down();

                yield return new Migration(kv.Key, kv.Value.Name, up, down);
            }
        }
    }
}