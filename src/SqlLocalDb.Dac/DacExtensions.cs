using Microsoft.SqlServer.Dac;

namespace SqlLocalDb.Dac
{
    public static class DacExtensions
    {
        /// <summary>
        /// Deploys a Dac package into a local database
        /// </summary>
        /// <param name="database"></param>
        /// <param name="packagePath"></param>
        /// <param name="deployOptions"></param>
        public static void DeployDac(this LocalDatabase database, string packagePath, DacDeployOptions deployOptions = null)
        {
            var dacServices = new DacServices(database.ConnectionString);
            
            using (var package = DacPackage.Load(packagePath))
            {
                dacServices.Deploy(package, database.DatabaseName, true, deployOptions);
            }
        }
    }
}
