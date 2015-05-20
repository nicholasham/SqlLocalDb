using Microsoft.SqlServer.Dac;
using SqlLocalDb.Dac;
using Xunit;

namespace SqlLocalDb.IntegrationTests
{
    public class DacTests
    {
        [Fact]
        public void ShouldBeAbleToDeployADacPackageIntoTheLocalDatabase()
        {
            var packagePath = @"C:\dev\GitHub\SqlLocalDb\src\SampleDatabase\bin\Debug\SampleDatabase.dacpac";

            var database = new LocalDatabase();
            database.DeployDac(packagePath);


        } 
    }
}