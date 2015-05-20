using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SqlTestDb
{
    public interface IBaselineFilesCopier
    {
        IEnumerable<FileInfo> Copy(string directoryPath, string databaseName);
    }
}