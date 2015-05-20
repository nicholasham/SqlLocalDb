using System.Data;

namespace SqlTestDb
{
    public static class ConnectionExtensions
    {
        public static int ExecuteSql(this IDbConnection connection, string sql, params object[] parameters)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format(sql, parameters);
                command.CommandType = CommandType.Text;
                return  command.ExecuteNonQuery();
            }
        }
    }
}