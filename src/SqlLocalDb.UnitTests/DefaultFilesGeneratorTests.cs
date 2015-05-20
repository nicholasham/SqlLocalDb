using System;
using System.IO;
using System.Linq;
using Ploeh.AutoFixture;
using Should;
using Xunit;

namespace SqlLocalDb.UnitTests
{
    public class DefaultFilesGeneratorTests : AutoFixture
    {

        [Fact]
        public void Generate_ShouldOutputRenamedFilesToTheGivenDirectory()
        {
            var directoryPath = Path.GetDirectoryName(GetType().Assembly.Location);
            var databaseName = "Database1";
            var generator = new DefaultFilesGenerator(directoryPath, databaseName);

            var files = generator.Generate();

            files.Count().ShouldEqual(2);
            files.ElementAt(0).Exists.ShouldBeTrue();
            files.ElementAt(0).DirectoryName.ShouldEqual(directoryPath);
            files.ElementAt(0).Name.StartsWith(databaseName).ShouldBeTrue();
            files.ElementAt(1).Exists.ShouldBeTrue();
            files.ElementAt(1).DirectoryName.ShouldEqual(directoryPath);
            files.ElementAt(1).Name.StartsWith(databaseName).ShouldBeTrue();

        }
    }
}