using System.Data;

namespace DbTransmogrifier.Database
{
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

        public static void Execute(this IDbConnection connection, string script, IDbTransaction transaction = null)
        {
            using (var command = connection.CreateCommand(script))
            {
                command.Transaction = transaction;
                command.ExecuteNonQuery();
            }
        }
    }
}