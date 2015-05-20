MERGE INTO [dbo].Country AS TargetTable
    USING (VALUES
        ('GB', 'Great Britain'),
        ('US', 'United States')
    ) AS SourceTable (Code, Name)
    ON TargetTable.Code = SourceTable.Code
    WHEN MATCHED THEN
        UPDATE SET
            [Code] = SourceTable.Code,
            [Name] = SourceTable.Name
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (Code, Name)
        VALUES (Code, Name)
    WHEN NOT MATCHED BY SOURCE THEN
        DELETE;