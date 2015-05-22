using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace SqlLocalDb
{
    public class LocalDatabase : Disposable
    {
        public const string ConnectionStringPattern =
            @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True";

        private const string DataFileExtension = ".MDF";

        public LocalDatabase(IEnumerable<FileInfo> files)
        {
            if (files.Any(file => !file.Exists))
            {
                throw new ArgumentException("All files should exist", "files");
            }

            Files = files;
            DatabaseName =
                Path.GetFileNameWithoutExtension(
                    Files.First(
                        file => file.Extension.Equals(DataFileExtension, StringComparison.InvariantCultureIgnoreCase))
                        .Name);
        }

        public LocalDatabase(IFilesGenerator filesGenerator) : this(filesGenerator.Generate())
        {
        }

        public LocalDatabase()
            : this(new DefaultFilesGenerator(new DefaultDatabaseNameGenerator(), new DefaultOutputDirectoryProvider()))
        {
        }

        public IEnumerable<FileInfo> Files { get; private set; }

        public string ConnectionString
        {
            get { return string.Format(ConnectionStringPattern, DatabaseName); }
        }

        public string DatabaseName { get; private set; }

        private FileInfo GetDataFile()
        {
            return
                Files.First(
                    x =>
                        Path.GetExtension(x.FullName)
                            .Equals(DataFileExtension, StringComparison.InvariantCultureIgnoreCase));
        }

        public SqlConnection GetConnection()
        {
            if (!IsAttached())
            {
                Attach();
            }

            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public bool IsAttached()
        {
            var connectionString = string.Format(ConnectionStringPattern, "Master");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return
                    Convert.ToInt32(connection.ExecuteScalar<object>("SELECT ISNULL(DB_ID('{0}'), 0 ) AS Value",
                        DatabaseName)) > 0;
            }
        }

        private void Attach()
        {
            var attachPart = string.Format(";AttachDbFilename={0}",GetDataFile().FullName);
            var attachConnectionString = ConnectionString + attachPart;

            using (var connection = new SqlConnection(attachConnectionString))
            {
                connection.Open();
                connection.Close();
            }
        }

        private void Detach()
        {

            var connectionString = string.Format(ConnectionStringPattern, "Master");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ExecuteSql(@"ALTER DATABASE [{0}] SET OFFLINE WITH ROLLBACK IMMEDIATE", DatabaseName);
                connection.ExecuteSql(@"exec sp_detach_db '{0}'", DatabaseName);
                connection.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Files != null)
            {
                if (IsAttached())
                {
                    Detach();
                }

                DeleteFiles();
            }

            base.Dispose(disposing);
        }

        private void DeleteFiles()
        {
            foreach (var file in Files.Where(file => file.Exists))
            {
                file.Delete();
                file.Refresh();
            }
        }
    }
}