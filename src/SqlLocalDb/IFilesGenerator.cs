using System.Collections.Generic;
using System.IO;

namespace SqlLocalDb
{
    public interface IFilesGenerator
    {
        IEnumerable<FileInfo> Generate();
    }
}