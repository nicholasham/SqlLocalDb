using System;
using System.IO;
using System.Reflection;
using Microsoft.SqlServer.Dac;

namespace SqlLocalDb.Dac
{
    public static class DacExtensions
    {
        /// <summary>
        ///     Deploys a Dacpac package into a local database
        /// </summary>
        /// <param name="database"></param>
        /// <param name="packageFilePath"></param>
        /// <param name="deployOptions"></param>
        public static void DeployDacpac(this LocalDatabase database, string packageFilePath, DacDeployOptions deployOptions = null)
        {
            var dacServices = new DacServices(database.ConnectionString);

            using (var package = DacPackage.Load(packageFilePath))
            {
                dacServices.Deploy(package, database.DatabaseName, true, deployOptions);
            }
        }

        /// <summary>
        ///     Deploys a dacpac package into a local database. The package is loaded from an assembly resource
        /// </summary>
        /// <param name="database"></param>
        /// <param name="assembly"></param>
        /// <param name="embeddedPackageFileName"></param>
        public static void DeployDacpac(this LocalDatabase database, Assembly assembly, string embeddedPackageFileName)
        {
            var packageFilePath = Path.Combine(Path.GetTempPath(), string.Format("{0}.dacpac", Guid.NewGuid()));
            var packageFile = assembly.GetResource(embeddedPackageFileName).SaveToDisk(packageFilePath);

            try
            {
                database.DeployDacpac(packageFile.FullName);
            }
            finally
            {
                File.Delete(packageFile.FullName);
            }
        }
    }
}