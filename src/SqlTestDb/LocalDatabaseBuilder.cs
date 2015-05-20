using System.IO;

namespace SqlTestDb
{
    public class LocalDatabaseBuilder
    {
        private string databaseName;
        private string directoryPath;
        private IBaselineFilesCopier baselineFilesCopier;

        public LocalDatabaseBuilder Named(string value)
        {
            databaseName = value;
            return this;
        }

        public LocalDatabaseBuilder FilesOutputtedTo(string value)
        {
            directoryPath = value;
            return this;
        }

        public LocalDatabaseBuilder BaselineFilesCopiedFrom(IBaselineFilesCopier copier)
        {
            baselineFilesCopier = copier;
            return this;
        }

        public LocalDatabaseBuilder()
        {
            databaseName = Path.GetRandomFileName();
            baselineFilesCopier = new DefaultBaselineFilesCopier();
            directoryPath = Path.GetDirectoryName(GetType().Assembly.Location);
        }

        public LocalDatabase Build()
        {
            var files = baselineFilesCopier.Copy(directoryPath, databaseName);
            return new LocalDatabase(files);
        }
    }
}