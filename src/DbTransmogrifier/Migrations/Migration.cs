using System.Collections.Generic;
using System.Linq;

namespace DbTransmogrifier.Migrations
{
    public class Migration
    {
        public Migration(long version, string name, IEnumerable<string> up, IEnumerable<string> down)
        {
            Version = version;
            Name = name;
            Up = up.ToList().AsReadOnly();
            Down = down.ToList().AsReadOnly();
        }

        public long Version { get; private set; }
        public string Name { get; private set; }
        public IList<string> Up { get; private set; }
        public IList<string> Down { get; private set; }
    }
}