using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Should;
using Xunit;

namespace SqlLocalDb.UnitTests
{
    public class LocalDatabaseTests : IDisposable
    {
        private LocalDatabase database;

        [Fact]
        public void Constructor_ShouldThrowWhenPassedMissingFiles()
        {
            var files = new[] {new FileInfo("Missing.Txt")};

            Assert.Throws<ArgumentException>(() => new LocalDatabase(files));

        }

        [Fact]
        public void Constructor_ShouldNotAttachOnInstantiation()
        {
            database = new LocalDatabase();
            database.IsAttached().ShouldBeFalse();
        }

        [Fact]
        public void GetConnection_ShouldAttach()
        {
            database = new LocalDatabase();
            database.GetConnection();
            database.IsAttached().ShouldBeTrue();

        }

        [Fact]
        public void Dispose_ShouldDetachAndDeleteFiles()
        {
            database = new LocalDatabase();
            database.GetConnection();

            database.Dispose();
            
            database.IsAttached().ShouldBeFalse();
            database.Files.ShouldNotExist();
        }

        public void Dispose()
        {
            if (database != null)
            {
                database.Dispose();
                database = null;    
            }
        }
    }
}