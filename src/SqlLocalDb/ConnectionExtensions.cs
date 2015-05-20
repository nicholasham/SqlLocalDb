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
                command.CommandText = string.Format(sql, parameters);
                command.CommandType = CommandType.Text;
                return  command.ExecuteNonQuery();
            }
        }

        public static T ExecuteScalar<T>(this IDbConnection connection, string sql, params object[] parameters)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format(sql, parameters);
                command.CommandType = CommandType.Text;
                var value = command.ExecuteScalar();
                return (T) value;
            }
        }

        public static void ExecuteScriptBlock(this IDbConnection connection, string scriptBlock)
        {
            string[] splitter = { "\r\nGO\r\n" };
            var commandTexts = scriptBlock.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

            foreach (var commandText in commandTexts)
            {
                connection.ExecuteSql(commandText);
            }
        }
    }
}