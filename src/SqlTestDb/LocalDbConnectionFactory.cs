using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SqlTestDb
{
    public class LocalDatabaseConnectionFactory
    {
        public LocalDatabaseConnectionFactory()
        {
            DatabaseFilePath = GetRandomDatabaseFilePath("TestDatabase.mdf");
            ConnectionString = GetDatabaseConnectionString(DatabaseFilePath);

            using (var connection = (SqlConnection)Create())
            {
                connection.Open();
                RunDatabaseCreationScripts(connection);
            }
        }

        private string ConnectionString { get; set; }

        private string DatabaseFilePath { get; set; }

        public IDbConnection Create()
        {
            return new SqlConnection(ConnectionString);
        }

        private static string GetDatabaseConnectionString(string databaseFilePath)
        {
            return string.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0};Integrated Security=True", databaseFilePath);
        }

        private static Assembly ThisAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }

        private string GetRandomDatabaseFilePath(string templateDatabaseFileName)
        {
            var directoryName = Path.GetDirectoryName(ThisAssembly().Location);
            var randomFileName = templateDatabaseFileName.Replace(".mdf", string.Format("-{0}.mdf", Guid.NewGuid().ToString("N")));
            var filePath = Path.Combine(directoryName, randomFileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var writer = new FileStream(filePath, FileMode.CreateNew))
            {
                using (var databaseStream = GetResourceStream(templateDatabaseFileName))
                {
                    databaseStream.Seek(0, SeekOrigin.Begin);
                    databaseStream.CopyTo(writer);
                }
            }

            return filePath;
        }

        private Stream GetResourceStream(string databaseFileName)
        {
            var fullName = ThisAssembly().GetManifestResourceNames().First(x => x.EndsWith(databaseFileName, StringComparison.InvariantCultureIgnoreCase));

            return ThisAssembly().GetManifestResourceStream(fullName);
        }

        private string GetResourceText(string resourceName)
        {
            using (var reader = new StreamReader(GetResourceStream(resourceName)))
            {
                return reader.ReadToEnd();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private void RunDatabaseCreationScripts(SqlConnection connection)
        {
            var text = GetResourceText("TestDatabase.sql");

            string[] splitter = { "\r\nGO\r\n" };
            var commandTexts = text.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

            foreach (var commandText in commandTexts)
            {
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}