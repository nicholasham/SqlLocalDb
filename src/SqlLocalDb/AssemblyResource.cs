using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SqlLocalDb
{

    public static class AssemblyExtensions
    {
        public static AssemblyResource GetResource(this Assembly assembly, string named)
        {
            var resourceNames = assembly.GetManifestResourceNames().ToArray();
            var resourceName = resourceNames.FirstOrDefault(name => name.EndsWith(named, StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentException(string.Format("No assembly resource can be found that matches the name {0}.", named), "named");
            }

            return new AssemblyResource(assembly, resourceName);
        }
    }

    public class AssemblyResource 
    {
        
        internal AssemblyResource(Assembly assembly, string resourceName)
        {
            Assembly = assembly;
            ResourceName = resourceName;
        }

        protected Assembly Assembly { get; private set; }
        protected string ResourceName { get; private set; }

        public FileInfo SaveToDisk(string filePath)
        {
            if (File.Exists(filePath))
            {
                return new FileInfo(filePath);
            }

            using (var writer = new FileStream(filePath, FileMode.CreateNew))
            {
                using (var databaseStream = Assembly.GetManifestResourceStream(ResourceName))
                {
                    databaseStream.Seek(0, SeekOrigin.Begin);
                    databaseStream.CopyTo(writer);
                }
            }

            return new FileInfo(filePath);
        }

        public string GetText()
        {
            using (var reader = new StreamReader(Assembly.GetManifestResourceStream(ResourceName)))
            {
                return reader.ReadToEnd();
            }
        }

    }
}