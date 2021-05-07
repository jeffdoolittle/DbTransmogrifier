using System;
using System.Configuration;

namespace DbTransmogrifier.Migrations
{
    public static class ConnectionStringSettingsCollectionExtensions
    {
        public static string GetConnectionStringOrEmpty(this ConnectionStringSettingsCollection collection, string name)
        {
            try
            {
                return collection[name].ConnectionString;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
