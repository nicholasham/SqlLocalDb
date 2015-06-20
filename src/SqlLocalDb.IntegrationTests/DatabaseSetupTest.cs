using Microsoft.SqlServer.Dac;
using Should;
using SqlLocalDb.Dac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SqlLocalDb.IntegrationTests
{
    public class DacExtensionsTests
    {
        [Fact]
        public void ShouldBeAbleToDeployADacPackageIntoTheLocalDatabase()
        {
            var packagePath = @"SampleDatabase.dacpac";

            using (var database = new LocalDatabase())
            {
                database.DeployDacpac(packagePath, new DacDeployOptions());
            }
        }
    }
}