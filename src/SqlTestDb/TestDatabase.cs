using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace SqlTestDb
{

    // 1. Need to get the baseline database files from somewhere
    // 2. Need a way of generating the file name
    // 3. Need a way of setting an output location
    // 4. Need to be able to detach and delete the data files afterwards


    public interface ITestDatabase : IDisposable
    {
        IDbConnection GetConnection();
    }

    public interface IDatabaseNameGenerator
    {
        string Generate();
    }

    public class TestDatabase : Disposable
    {
        public const string ConnectionStringPattern = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0};Initial Catalog={1};Integrated Security=True";
        public FileInfo DataFile { get; private set; }
        public FileInfo LogFile { get; private set; }

        public TestDatabase(IDatabaseNameGenerator generator)
        {
            var sequence = generator.Generate();
            var directoryName = Path.GetDirectoryName(GetType().Assembly.Location);

            DataFile = new FileInfo(Path.Combine(directoryName, string.Format("{0}.mdf", sequence)));
            LogFile = new FileInfo(Path.Combine(directoryName, string.Format("{0}_log.ldf", sequence)));
        }

        private void CreateDataFiles()
        {
            AssemblyResource.FromAssembly(GetType().Assembly, "TestDatabase.mdf").SaveToDisk(DataFile.FullName);
            AssemblyResource.FromAssembly(GetType().Assembly, "TestDatabase_log.ldf").SaveToDisk(LogFile.FullName);

            DataFile.Refresh();
            LogFile.Refresh();
        }

        private string ConnectionString
        {
            get
            {
                return string.Format(ConnectionStringPattern, DataFile, DatabaseName);
                
            }
        }

        private bool Exists
        {
            get { return DataFile.Exists; }
        }


        public SqlConnection GetConnection()
        {
            if (!Exists)
            {
                CreateDataFiles();    
            }

            return new SqlConnection(ConnectionString);
        }

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

        private string DatabaseName
        {
            get { return DataFile.Name.Replace(".mdf", ""); }
        }

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
            DataFile.Delete();
            LogFile.Delete();
        }
    }
}