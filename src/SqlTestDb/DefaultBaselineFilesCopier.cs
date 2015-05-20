using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SqlTestDb
{
    public class DefaultBaselineFilesCopier : IBaselineFilesCopier
    {
        public IEnumerable<FileInfo> Copy(string directoryPath, string databaseName)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new ArgumentException("Invalid directory path", directoryPath);
            }

            var resourceNames = new[] {"TestDatabase.mdf", "TestDatabase_log.ldf"};

            return resourceNames.Select(x => SaveToDisk(x, directoryPath, databaseName));
       }

        private FileInfo SaveToDisk(string resourceName, string directoryPath, string databaseName)
        {
            var filePath = Path.Combine(directoryPath, resourceName.Replace("TestDatabase", databaseName));
            return AssemblyResource.FromAssembly(GetType().Assembly, resourceName).SaveToDisk(filePath);
        }
    }
}