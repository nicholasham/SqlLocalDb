SELECT physical_name 
FROM sys.master_files
WHERE database_id IN (SELECT database_id FROM sys.databases WHERE name = '{0}')