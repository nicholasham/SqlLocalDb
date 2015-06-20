using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

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
                return (T)value;
            }
        }

        public static IDataReader ExecuteReader(this IDbConnection connection, string sql, params object[] parameters)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql.MergeParameters(parameters);
                command.CommandType = CommandType.Text;
                return command.ExecuteReader();
            }
        }

        public static void ExecuteScript(this IDbConnection connection, string scriptBlock)
        {
            string[] splitter = { "\r\nGO\r\n" };
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

        public static void CreateDatabase(this IDbConnection connection, string databaseName, string filesDirectoryPath = null)
        {
            if (!string.IsNullOrEmpty(filesDirectoryPath) && Directory.Exists(filesDirectoryPath))
            {
                connection.ExecuteSql(GetScript("CreateDatabaseWithFiles.sql"), databaseName, filesDirectoryPath);
            }
            else
            {
                connection.ExecuteSql(GetScript("CreateDatabaseNoFiles.sql"), databaseName);
            }
        }

        public static IEnumerable<FileInfo> GetDatabaseFiles(this IDbConnection connection, string databaseName)
        {
            var files = new List<FileInfo>();
            using (var reader = connection.ExecuteReader(GetScript("GetDatabaseFiles.sql"), databaseName))
            {
                while (reader.Read())
                {
                    files.Add(new FileInfo(reader.GetString(0)));
                }
            }
            return files;
        }

        public static IEnumerable<string> GetDatabaseNames(this IDbConnection connection)
        {
            var names = new List<string>();
            using (var reader = connection.ExecuteReader(GetScript("GetDatabaseNames.sql")))
            {
                while (reader.Read())
                {
                    names.Add(reader.GetString(0));
                }
            }
            return names;
        }

        public static void DropDatabase(this IDbConnection connection, string databaseName)
        {
            connection.ExecuteSql(GetScript("DropDatabase.sql"), databaseName);
        }

        public static bool DatabaseExists(this IDbConnection connection, string databaseName)
        {
            return connection.GetDatabaseNames().Any(s => s.Equals(databaseName, StringComparison.InvariantCultureIgnoreCase));
        }

        private static string GetScript(string scriptFileName)
        {
            return typeof(ConnectionExtensions).Assembly.GetResource(scriptFileName).GetText();
        }
    }
}