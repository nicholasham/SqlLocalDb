using System;
using System.Data;

namespace SqlLocalDb
{
    public static class ConnectionExtensions
    {
        public static int ExecuteSql(this IDbConnection connection, string sql, params object[] parameters)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql.MergeParameters(parameters);
                command.CommandType = CommandType.Text;
                return command.ExecuteNonQuery();
            }
        }

        public static T ExecuteScalar<T>(this IDbConnection connection, string sql, params object[] parameters)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql.MergeParameters(parameters);
                command.CommandType = CommandType.Text;
                var value = command.ExecuteScalar();
                return (T) value;
            }
        }

        public static void ExecuteScript(this IDbConnection connection, string scriptBlock)
        {
            string[] splitter = {"\r\nGO\r\n"};
            var commandTexts = scriptBlock.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

            foreach (var commandText in commandTexts)
            {
                connection.ExecuteSql(commandText);
            }
        }

        private static string MergeParameters(this string sql, params object[] parameters)
        {
            return parameters.Length == 0 ? sql : string.Format(sql, parameters);
        }
    }
}