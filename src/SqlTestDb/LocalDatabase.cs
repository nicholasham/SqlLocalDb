using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace SqlTestDb
{
    public class LocalDatabase : Disposable
    {
        public const string ConnectionStringPattern = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0};Initial Catalog={1};Integrated Security=True";

        public LocalDatabase(IEnumerable<FileInfo> files)
        {
            Files = files;
        }

        public IEnumerable<FileInfo> Files { get; private set; }

        private string ConnectionString
        {
            get
            {
                return string.Format(ConnectionStringPattern, Files.First(x=> Path.GetExtension(x.FullName).Equals("mdf", StringComparison.InvariantCultureIgnoreCase)), DatabaseName);

            }
        }

        private bool Exists
        {
            get { return Files.First().Exists; }
        }


        public SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }


        // TODO: Only detach if database exists
        private bool Detach()
        {
            var databaseName = DatabaseName;

            using (var connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True"))
            {
                connection.Open();
                connection.ExecuteSql(@"ALTER DATABASE [{0}] SET OFFLINE WITH ROLLBACK IMMEDIATE", databaseName);
                connection.ExecuteSql(@"exec sp_detach_db '{0}'", databaseName);
                connection.Close();
            }

            return true;
        }

        public string DatabaseName { get; private set; }
        
        protected override void Dispose(bool disposing)
        {
            if (Exists)
            {
                if (Detach())
                {
                    DeleteFiles();
                }
            }

            base.Dispose(disposing);
        }

        private void DeleteFiles()
        {
            foreach (var fileInfo in Files)
            {
                fileInfo.Delete();
            }
        }


        public static LocalDatabaseBuilder Builder { get { return new LocalDatabaseBuilder();} }
    }
}