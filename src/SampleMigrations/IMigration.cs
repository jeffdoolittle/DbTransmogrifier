using System.Collections.Generic;

namespace SampleMigrations
{
    public interface IMigration
    {
        IEnumerable<string> Up();
        IEnumerable<string> Down();
    }
}