using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace DbTransmogrifier
{
    class Program
    {
        static void Main(string[] args)
        {
            new Transmogrifier();
        }
    }

    public class Transmogrifier
    {
        private static readonly ILog Log = LoggerFactory.GetLoggerFor(typeof (Transmogrifier));

        public Transmogrifier()
        {
            var providerName = ConfigurationManager.AppSettings["ProviderInvariantName"] ?? "System.Data.SqlClient";
            var providerFactory = DbProviderFactories.GetFactory(providerName);

            Log.InfoFormat("Using {0} provider", providerFactory.GetType());

            var masterConnectionSettings = ConfigurationManager.ConnectionStrings["Master"];
            var targetConnectionSettings = ConfigurationManager.ConnectionStrings["Target"];

            using (var masterConnection = providerFactory.CreateConnection())
            {
                masterConnection.ConnectionString = masterConnectionSettings.ConnectionString;
                masterConnection.Open();

                var builder = providerFactory.CreateConnectionStringBuilder();
                builder.ConnectionString = targetConnectionSettings.ConnectionString;
                var databaseName = builder["Initial Catalog"] as string ?? "";
                EnsureTargetDatabase(masterConnection, databaseName);
            }

            var targetConnection = providerFactory.CreateConnection();
            targetConnection.ConnectionString = targetConnectionSettings.ConnectionString;
            targetConnection.Open();
        }

        private void EnsureTargetDatabase(IDbConnection connection, string databaseName)
        {
            bool exists;
            using(var command = connection.CreateCommand("SELECT count(*) FROM sys.databases WHERE [name] = @0", databaseName))
            {
                exists = (int)command.ExecuteScalar() > 0;
            }

            if (exists)
            {
                Log.InfoFormat("Database {0} already exists.", databaseName);
                return;
            }

            using (var createCommand = connection.CreateCommand(string.Format("CREATE DATABASE [{0}]", databaseName)))
            {
                createCommand.ExecuteNonQuery();
                Log.InfoFormat("Database {0} created.", databaseName);
            }
        }
    }

    public static class DbConnectionExtensions
    {
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, params object[] parameters)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            if (parameters != null && parameters.Length > 0)
                for (int p = 0; p < parameters.Length; p++)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@" + p;
                    parameter.Value = parameters[p];
                    command.Parameters.Add(parameter);
                }
            return command;
        }
    }

    public static class LoggerFactory
    {
        public static ILog GetLoggerFor(Type type)
        {
            return new DefaultLog();
        }
    }

    public interface ILog
    {
        void Info(string message);
        void InfoFormat(string message, params object[] args);
    }

    public class DefaultLog : ILog
    {
        public void Info(string message)
        {
            Console.WriteLine("[INFO] - " + message);
        }

        public void InfoFormat(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }
    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtensionAttribute : Attribute
    {
    }
}
