# SqlLocalDb

## Overview

SqlLocalDb is a simple library that makes it easy to setup a local sql database for full stack integration testing.

## Documentation

### Basic Usage

First install the [NuGet package](https://www.nuget.org/packages/SqlLocalDb/):

```Install-Package SqlLocalDb```

Add the appropriate namespace:

```
using SqlLocalDb;
```

Then create an instance and just open a connection to the database:

```
var database = new LocalDatabase();

using (var connection = database.GetConnection())
{
    connection.Open();

    // Use the connection...
}

// Destroy the database instance and remove any data files
database.Dispose()
```
