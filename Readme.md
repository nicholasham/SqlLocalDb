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

// Destroy the database instance and remove any data files by calling dispose
database.Dispose()
```
### Deploying a DAC package to your local database

First install the [NuGet package](https://www.nuget.org/packages/SqlLocalDb.Dac/):

```Install-Package SqlLocalDb.Dac```

Add the appropriate namespace:

```
using SqlLocalDb;
using SqlLocalDb.Dac;
```

Then create an instance and just deploy the DAC


```
var database = new LocalDatabase();

// deploy your DAC package into the local database instance
var packagePath = "SampleDatabase.dacpac";
database.DeployDac(packagePath);

using (var connection = database.GetConnection())
{
    connection.Open();

    // Use the connection...
}

// Destroy the database instance and remove any data files by calling dispose
database.Dispose()
```
