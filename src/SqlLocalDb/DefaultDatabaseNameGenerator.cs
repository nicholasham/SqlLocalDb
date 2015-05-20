using System;

namespace SqlLocalDb
{
    public class DefaultDatabaseNameGenerator : IDatabaseNameGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}