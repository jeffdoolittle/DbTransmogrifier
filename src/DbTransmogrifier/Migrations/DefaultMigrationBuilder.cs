using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbTransmogrifier.Migrations
{
    public class DefaultMigrationBuilder : IMigrationBuilder
    {
        private readonly IDictionary<Type, object> _dependencies;
        private readonly IMigrationTypeSource _migrationTypeSource;

        public DefaultMigrationBuilder(IDictionary<Type, object> dependencies, IMigrationTypeSource migrationTypeSource)
        {
            _dependencies = dependencies;
            _migrationTypeSource = migrationTypeSource;
        }

        public Migration BuildMigration(long version)
        {
            var migrationType = _migrationTypeSource.GetMigrationType(version);
            if (migrationType == null) return null;
            return Build(version, migrationType);
        }

        private Migration Build(long version, Type migrationType)
        {
            if (_dependencies.Count == 0) return SimpleBuild(version, migrationType);
            else return BuildWithDependencies(version, migrationType);
        }

        private Migration SimpleBuild(long version, Type migrationType)
        {
            dynamic migration = Activator.CreateInstance(migrationType);
            IEnumerable<string> up = migration.Up();
            IEnumerable<string> down = migration.Down();
            return new Migration(version, migrationType.Name, up, down);
        }

        private Migration BuildWithDependencies(long version, Type migrationType)
        {
            var type = migrationType;
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
                .Where(x => x.CanWrite)
                .ToList();

            foreach (var property in properties)
            {
                property.SetValue(migration, _dependencies[property.PropertyType], new object[0]);
            }

            IEnumerable<string> up = migration.Up();
            IEnumerable<string> down = migration.Down();

            return new Migration(version, type.Name, up, down);
        }
    }
}