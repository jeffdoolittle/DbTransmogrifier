using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DbTransmogrifier.Migrations
{
    public class DefaultMigrationResolver : IMigrationResolver
    {
        private readonly IDictionary<long, Type> _migrationTypes = new Dictionary<long, Type>();

        public DefaultMigrationResolver()
        {
            var appAssembly = Assembly.GetExecutingAssembly();
            var appName = appAssembly.GetName().Name;

            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.exe").Select(x=>new FileInfo(x))
                .Union(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").Select(x => new FileInfo(x)))
                .Where(x=>!x.Name.Contains(appName))
                .Select(x=> Assembly.LoadFrom(x.FullName))
                .ToList();

            var types = assemblies.SelectMany(x => x.GetTypes()).ToList();

            var migrationAttributeType = types.SingleOrDefault(x => x.Name == "MigrationAttribute");
            var migrationInterfaceType = types.SingleOrDefault(x => x.Name == "IMigration");

            var migrationTypes = types.Where(x => x.GetInterfaces().Contains(migrationInterfaceType));

            foreach (var type in migrationTypes)
            {
                dynamic attribute = type.GetCustomAttributes(migrationAttributeType, true).SingleOrDefault();
                int version = attribute.Version;

                _migrationTypes.Add(version, type);
            }
        }

        public IDictionary<long, Type> GetMigrationsGreaterThan(long version)
        {
            return _migrationTypes
                .Where(x => x.Key > version)
                .OrderBy(x => x.Key)
                .ToDictionary(x=>x.Key, x => x.Value);
        }

        public IDictionary<long, Type> GetMigrationsLessThanOrEqualTo(long version)
        {
            return _migrationTypes
                .Where(x => x.Key <= version)
                .OrderByDescending(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}