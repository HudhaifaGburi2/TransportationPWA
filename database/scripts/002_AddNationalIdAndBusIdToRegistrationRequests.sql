-- ============================================
-- Migration: Add NationalId and BusId to RegistrationRequests
-- Purpose: Support National_ID persistence and bus assignment
-- ============================================

USE transportationDB;
GO

-- Add NationalId column for storing user's National ID
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[RegistrationRequests]') AND name = 'NationalId')
BEGIN
    ALTER TABLE [dbo].[RegistrationRequests]
    ADD [NationalId] NVARCHAR(20) NULL;
    PRINT 'Added NationalId column to RegistrationRequests';
END
GO

-- Add AssignedBusId column for bus assignment
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[RegistrationRequests]') AND name = 'AssignedBusId')
BEGIN
    ALTER TABLE [dbo].[RegistrationRequests]
    ADD [AssignedBusId] UNIQUEIDENTIFIER NULL;
    PRINT 'Added AssignedBusId column to RegistrationRequests';
END
GO

-- Add foreign key constraint for AssignedBusId
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_RegistrationRequests_Buses')
BEGIN
    ALTER TABLE [dbo].[RegistrationRequests]
    ADD CONSTRAINT [FK_RegistrationRequests_Buses] 
    FOREIGN KEY ([AssignedBusId]) REFERENCES [dbo].[Buses]([BusId]);
    PRINT 'Added FK_RegistrationRequests_Buses constraint';
END
GO

-- Create index on NationalId for faster lookups
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_RegistrationRequests_NationalId' AND object_id = OBJECT_ID('dbo.RegistrationRequests'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_RegistrationRequests_NationalId] 
    ON [dbo].[RegistrationRequests] ([NationalId]);
    PRINT 'Created IX_RegistrationRequests_NationalId index';
END
GO

-- Create index on AssignedBusId for faster lookups
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_RegistrationRequests_AssignedBusId' AND object_id = OBJECT_ID('dbo.RegistrationRequests'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_RegistrationRequests_AssignedBusId] 
    ON [dbo].[RegistrationRequests] ([AssignedBusId]);
    PRINT 'Created IX_RegistrationRequests_AssignedBusId index';
END
GO

PRINT 'Migration completed successfully';
GO
