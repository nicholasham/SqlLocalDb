using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SqlLocalDb
{
    public class DefaultFilesGenerator : FilesGenerator
    {
        public DefaultFilesGenerator(string directoryPath, string databaseName) : base(directoryPath, databaseName)
        {
        }

        public DefaultFilesGenerator(IDatabaseNameGenerator databaseNameGenerator, IOutputDirectoryProvider outputDirectoryProvider) : base(databaseNameGenerator, outputDirectoryProvider)
        {
        }

        protected override IEnumerable<FileInfo> Generate(string directoryPath, string databaseName)
        {
         
            var resourceNames = new[] { "TestDatabase.mdf", "TestDatabase_log.ldf" };

            return resourceNames.Select(x => SaveToDisk(x, directoryPath, databaseName)).ToArray();
        }

        private FileInfo SaveToDisk(string resourceName, string directoryPath, string databaseName)
        {
            var filePath = Path.Combine(directoryPath, resourceName.Replace("TestDatabase", databaseName));
            return AssemblyResource.FromAssembly(GetType().Assembly, resourceName).SaveToDisk(filePath);
        }

        
    }
}