namespace SqlLocalDb
{
    public static class LocalDatabaseExtensions
    {
        /// <summary>
        /// Executes a script with GO statements against the local database
        /// </summary>
        /// <param name="database">the local database instance</param>
        /// <param name="scriptBlock">the script to execute</param>
        public static void ExecuteScript(this LocalDatabase database, string scriptBlock)
        {
            using (var connection = database.GetConnection())
            {
                connection.ExecuteScript(scriptBlock);
            }
        } 
    }
}