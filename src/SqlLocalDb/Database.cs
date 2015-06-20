using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace SqlLocalDb
{
    public abstract class Database : Disposable
    {
        private readonly string connectionStringPattern;
        private readonly bool databaseShouldBeDropped;

        protected Database(string serverInstanceName, string databaseName)
            : this(serverInstanceName, databaseName, null, null, null)
        {
        }

        public Database(string serverInstanceName, string databaseName, string filesDirectoryPath)
            : this(serverInstanceName, databaseName, null, null, filesDirectoryPath)
        {
        }

        public Database(string serverInstanceName, string databaseName, string userId, string password,
            string filesDirectoryPath)
        {
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(password))
            {
                connectionStringPattern = string.Format(
                    @"Data Source={0};Initial Catalog={1};Integrated Security=True", serverInstanceName, "{0}");
            }
            else
            {
                connectionStringPattern = string.Format(
                    @"Data Source={0};Initial Catalog={1};User Id={2};Password={3};", serverInstanceName, "{0}", userId,
                    password);
            }

            DatabaseName = databaseName;
            IEnumerable<FileInfo> files;
            databaseShouldBeDropped = CreateDatabase(databaseName, filesDirectoryPath, out files);
            Files = files;
        }

        public string DatabaseName { get; private set; }

        public IEnumerable<FileInfo> Files { get; private set; }

        public string ConnectionString
        {
            get { return string.Format(connectionStringPattern, DatabaseName); }
        }

        private bool CreateDatabase(string databaseName, string filesDirectoryPath, out IEnumerable<FileInfo> files)
        {
            var result = false;
            var connectionString = string.Format(connectionStringPattern, "Master");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                if (!connection.DatabaseExists(databaseName))
                {
                    connection.CreateDatabase(databaseName, filesDirectoryPath);
                    result = true;
                }
                files = connection.GetDatabaseFiles(databaseName);
                return result;
            }
        }

        private void DropDatabase()
        {
            var connectionString = string.Format(connectionStringPattern, "Master");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.DropDatabase(DatabaseName);
                connection.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (databaseShouldBeDropped)
                {
                    DropDatabase();
                }
            }

            base.Dispose(disposing);
        }
    }
}