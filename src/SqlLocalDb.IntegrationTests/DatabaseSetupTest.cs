using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlLocalDb.Dac;
using Xunit;

namespace SqlLocalDb.IntegrationTests
{
    public class DatabaseSetupTest
    {
        [Fact]
        public void ShouldBeAbleToDeployADacPackageIntoTheLocalDatabase()
        {
            var packagePath = @"SampleDatabase.dacpac";

            var database = new LocalDatabase();
            database.DeployDacpac(packagePath);


        } 
    }
}
