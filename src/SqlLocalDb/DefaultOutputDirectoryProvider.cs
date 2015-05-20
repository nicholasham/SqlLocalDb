using System.IO;

namespace SqlLocalDb
{
    public class DefaultOutputDirectoryProvider : IOutputDirectoryProvider
    {
        public string GetOutputDirectoryPath()
        {
            return Path.GetDirectoryName(GetType().Assembly.Location);
        }
    }
}