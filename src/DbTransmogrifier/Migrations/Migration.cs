using System.Collections.Generic;

namespace DbTransmogrifier.Migrations
{
    public class Migration
    {
        public Migration(long version, string name, IEnumerable<string> scripts)
        {
            Version = version;
            Name = name;
            Scripts = scripts;
        }

        public long Version { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<string> Scripts { get; private set; }
    }
}