-- ============================================
-- TUMS: Drop All Tables and Recreate Database
-- ============================================

USE transportationDB;
GO

-- Drop all foreign key constraints first
DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) 
    + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) 
    + ' DROP CONSTRAINT ' + QUOTENAME(name) + ';' + CHAR(13)
FROM sys.foreign_keys;

EXEC sp_executesql @sql;
GO

PRINT 'Dropped all foreign key constraints';
GO

-- Drop all tables
DECLARE @sql2 NVARCHAR(MAX) = N'';

SELECT @sql2 += N'DROP TABLE ' + QUOTENAME(SCHEMA_NAME(schema_id)) + '.' + QUOTENAME(name) + ';' + CHAR(13)
FROM sys.tables
WHERE type = 'U';

EXEC sp_executesql @sql2;
GO

PRINT 'Dropped all tables';
GO
