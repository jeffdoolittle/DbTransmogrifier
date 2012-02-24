using System.Collections.Generic;

namespace DbTransmogrifier.Migrations
{
    public class Migration
    {
        public Migration(long version, string name, IEnumerable<string> up, IEnumerable<string> down)
        {
            Version = version;
            Name = name;
            Up = up;
            Down = down;
        }

        public long Version { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<string> Up { get; private set; }
        public IEnumerable<string> Down { get; private set; }
    }
}