using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SqlLocalDb.IntegrationTests
{
    public class DatabaseSetupTest
    {
        [Fact]
        public void ShouldBeAbleToConstructADatabaseWithSeedData()
        {
            using (var database = new LocalDatabase())
            {
                using (var connection = database.GetConnection())
                {
                    connection.Open();
                    var seedScript =
                        AssemblyResource.FromAssembly(GetType().Assembly, "SampleDatabase_Create.sql").GetText();
                    connection.ExecuteScriptBlock(seedScript);

                }
            }
        }
    }
}
