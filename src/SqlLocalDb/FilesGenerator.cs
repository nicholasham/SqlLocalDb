using System;
using System.Collections.Generic;
using System.IO;

namespace SqlLocalDb
{
    public abstract class FilesGenerator : IFilesGenerator
    {
        private readonly string databaseName;
        private readonly string directoryPath;

        protected FilesGenerator(string directoryPath, string databaseName)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new ArgumentException("The directory path is not valid", "directoryPath");
            }

            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException("databaseName");
            }

            this.directoryPath = directoryPath;
            this.databaseName = databaseName;
        }

        protected FilesGenerator(IDatabaseNameGenerator databaseNameGenerator, IOutputDirectoryProvider outputDirectoryProvider)
            : this(outputDirectoryProvider.GetOutputDirectoryPath(), databaseNameGenerator.Generate())
        {
            
        }


        public IEnumerable<FileInfo> Generate()
        {
            return Generate(directoryPath, databaseName);
        }

        protected abstract IEnumerable<FileInfo> Generate(string directoryPath, string databaseName);

    }
}