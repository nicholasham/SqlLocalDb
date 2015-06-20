using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SqlLocalDb
{
    public class LocalDatabase : Database
    {
        public LocalDatabase()
            : this(Guid.NewGuid().ToString("N"))
        {
        }

        public LocalDatabase(string databaseName)
            : this(databaseName, null)
        {
        }

        public LocalDatabase(string databaseName, string filesDirectoryPath)
            : base(@"(LocalDB)\MSSQLLocalDB", databaseName, filesDirectoryPath)
        {
        }
    }
}