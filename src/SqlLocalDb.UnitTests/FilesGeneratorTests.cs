using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using Ploeh.AutoFixture;
using Should;
using Xunit;

namespace SqlLocalDb.UnitTests
{
    public class TestFilesGenerator : FilesGenerator
    {
       
        public TestFilesGenerator(string directoryPath, string databaseName) : base(directoryPath, databaseName)
        {
        }

        public TestFilesGenerator(IDatabaseNameGenerator databaseNameGenerator, IOutputDirectoryProvider outputDirectoryProvider) : base(databaseNameGenerator, outputDirectoryProvider)
        {
        }

        protected override IEnumerable<FileInfo> Generate(string directoryPath, string databaseName)
        {
            return new[]{new FileInfo(Path.Combine(directoryPath, string.Format("{0}.txt", databaseName)))};
        }
    }

    public class FilesGeneratorTests : AutoFixture
    {
       
       

        [Fact]
        public void Constructor_ShouldThrowWhenTheOutputDirectoryDoesNotExist()
        {
            Assert.Throws<ArgumentException>(() => new TestFilesGenerator("Z:\ahdga1w1w", "GoodName"));
        }

        [Fact]
        public void Constructor_ShouldThrowWhenTheDatabaseNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new TestFilesGenerator(@"c:\", null));
        }

        [Fact]
        public void Constructor_ShouldThrowWhenTheDatabaseNameIsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new TestFilesGenerator(@"c:\", string.Empty));
        }

        [Fact]
        public void Constructor_ShouldInitialiseCorrectlyUsingExtensionAbstractions()
        {
            var databaseNameGenerator = Fixture.Freeze<IDatabaseNameGenerator>();
            var outputDirectoryProvider = Fixture.Freeze<IOutputDirectoryProvider>();

            databaseNameGenerator.Generate().Returns("GoodName");
            outputDirectoryProvider.GetOutputDirectoryPath().Returns(@"C:\");

            var filesGenerator = new TestFilesGenerator(databaseNameGenerator, outputDirectoryProvider);
            filesGenerator.Generate().First().FullName.ShouldEqual(@"C:\GoodName.txt");

            
        }
        
    }
}