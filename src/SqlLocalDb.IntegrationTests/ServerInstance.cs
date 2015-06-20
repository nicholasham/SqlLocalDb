using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace SqlLocalDb.IntegrationTests
{
    public class ServerInstance
    {
        private readonly string connectionString;

        public ServerInstance(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public bool DatabaseExists(string databaseName)
        {
            using (var connection = GetOpenConnection())
            {
                return connection.DatabaseExists(databaseName);
            }
        }

        public void DropDatabase(string databaseName)
        {
            using (var connection = GetOpenConnection())
            {
                if (connection.DatabaseExists(databaseName))
                {
                    connection.DropDatabase(databaseName);
                }
            }
        }

        public static ServerInstance LocalDb = new ServerInstance(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True");
    }
}