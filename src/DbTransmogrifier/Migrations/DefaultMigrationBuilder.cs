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

        public Migration BuildUpMigration(long version)
        {
            return BuildMigration(version, Direction.Up);
        }

        public Migration BuildDownMigration(long version)
        {
            return BuildMigration(version, Direction.Down);
        }

        private Migration BuildMigration(long version, Direction direction)
        {
            var migrationType = _migrationTypeSource.GetMigrationType(version);
            if (migrationType == null) return null;
            return Build(version, migrationType, direction);
        }

        private Migration Build(long version, Type migrationType, Direction direction)
        {
            if (_dependencies.Count == 0) return SimpleBuild(version, migrationType, direction);
            else return BuildWithDependencies(version, migrationType, direction);
        }

        private Migration SimpleBuild(long version, Type migrationType, Direction direction)
        {
            dynamic migration = Activator.CreateInstance(migrationType);
            return direction == Direction.Up 
                ? new Migration(version, migrationType.Name, migration.Up()) 
                : new Migration(version, migrationType.Name, migration.Down());
        }

        private Migration BuildWithDependencies(long version, Type migrationType, Direction direction)
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

            return direction == Direction.Up
                ? new Migration(version, migrationType.Name, migration.Up())
                : new Migration(version, migrationType.Name, migration.Down());
        }

        private enum Direction
        {
            Up,
            Down
        }
    }
}