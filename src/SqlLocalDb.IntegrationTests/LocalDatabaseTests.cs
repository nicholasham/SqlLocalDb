using Should;
using System;
using System.IO;
using System.Linq;
using Xbehave;

namespace SqlLocalDb.IntegrationTests
{
    public class DatabaseTests : IDisposable
    {
        [Scenario]
        public void ShouldBeAbleToCreateNewDatabaseWithDefaults(LocalDatabase database)
        {
            "Given defaults".f(() =>
            {
            });

            "When creating  the database".f((c) =>
            {
                database = new LocalDatabase().Using(c);
            });

            "Then the database is assigned a GUID as its name".f(() =>
            {
                Guid value;
                Guid.TryParse(database.DatabaseName, out value).ShouldBeTrue();
            });

            "And the database should exist on the local db instance"._(() =>
            {
                database.Files.All(file => file.Exists).ShouldBeTrue();
                ServerInstance.LocalDb.DatabaseExists(database.DatabaseName).ShouldBeTrue();
            });
        }

        [Scenario]
        public void ShouldBeAbleToCreateANewDatabaseWithAProvidedName(LocalDatabase database, string databaseName)
        {
            "Given a database name".f(() =>
            {
                databaseName = Guid.NewGuid().ToString("N");
            });

            "When creating  the database".f((c) =>
            {
                database = new LocalDatabase(databaseName).Using(c);
            });

            "Then the database should be assigned the provided name".f(() =>
            {
                database.DatabaseName.ShouldEqual(databaseName);
            });

            "And the database should exist on the local db instance"._(() =>
            {
                database.Files.All(file => file.Exists).ShouldBeTrue();
                ServerInstance.LocalDb.DatabaseExists(database.DatabaseName).ShouldBeTrue();
            });
        }

        [Scenario]
        public void ShouldDropNewDatabaseWhenTheInstanceIsDisposed(LocalDatabase database, string databaseName)
        {
            "Given a new database ".f((c) =>
            {
                database = new LocalDatabase();
                databaseName = database.DatabaseName;
                ServerInstance.LocalDb.DatabaseExists(databaseName).ShouldBeTrue();
            });

            "When disposing the database".f(() =>
            {
                database.Dispose();
            });

            "Then the database should not exist on the local instance".f(() =>
            {
                ServerInstance.LocalDb.DatabaseExists(databaseName).ShouldBeFalse();
            });
        }

        [Scenario]
        public void ShouldNotDropExistingDatabasesWhenTheInstanceIsDisposed(LocalDatabase database1, LocalDatabase database2)
        {
            "Given an existing  database".f((c) =>
            {
                database1 = new LocalDatabase();
                ServerInstance.LocalDb.DatabaseExists(database1.DatabaseName).ShouldBeTrue();
            });

            "And then creating a instance of database with the same name as the existing database".f(() =>
            {
                database2 = new LocalDatabase(database1.DatabaseName);
            });

            "When disposing the new database database".f(() =>
            {
                database2.Dispose();
            });

            "Then the existing database should still exist on the local instance".f(() =>
            {
                ServerInstance.LocalDb.DatabaseExists(database1.DatabaseName).ShouldBeTrue();
            });
        }

        public void Dispose()
        {
            DirectoryInfo directory;

            using (var database = new LocalDatabase())
            {
                directory = database.Files.First().Directory;
            }

            foreach (var file in directory.GetFiles())
            {
                var databaseName = Path.GetFileNameWithoutExtension(file.FullName);
                Guid value;

                if (Guid.TryParse(databaseName, out value))
                {
                    var exists = ServerInstance.LocalDb.DatabaseExists(databaseName);

                    if (exists)
                    {
                        ServerInstance.LocalDb.DropDatabase(databaseName);
                    }

                    file.Delete();
                    file.Refresh();
                }
            }
        }
    }
}