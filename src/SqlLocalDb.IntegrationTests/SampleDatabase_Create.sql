
PRINT N'Creating [dbo].[Country]...';


GO
CREATE TABLE [dbo].[Country] (
    [Code] NCHAR (2)      NOT NULL,
    [Name] NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Code] ASC)
);


PRINT N'Update complete.';

