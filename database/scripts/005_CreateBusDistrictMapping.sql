-- ============================================
-- TUMS Phase 1: Bus-District Mapping Table
-- Links buses to their assigned districts
-- ============================================

USE transportationDB;
GO

-- Create BusDistrictMappings table if not exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusDistrictMappings]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[BusDistrictMappings] (
        [MappingId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [BusId] UNIQUEIDENTIFIER NOT NULL,
        [DistrictId] UNIQUEIDENTIFIER NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] UNIQUEIDENTIFIER NULL,
        CONSTRAINT [PK_BusDistrictMappings] PRIMARY KEY CLUSTERED ([MappingId] ASC),
        CONSTRAINT [FK_BusDistrictMappings_Buses] FOREIGN KEY ([BusId]) REFERENCES [dbo].[Buses]([BusId]),
        CONSTRAINT [FK_BusDistrictMappings_Districts] FOREIGN KEY ([DistrictId]) REFERENCES [dbo].[Districts]([DistrictId]),
        CONSTRAINT [UQ_BusDistrictMappings_BusDistrict] UNIQUE ([BusId], [DistrictId])
    );

    CREATE NONCLUSTERED INDEX [IX_BusDistrictMappings_BusId] ON [dbo].[BusDistrictMappings] ([BusId]);
    CREATE NONCLUSTERED INDEX [IX_BusDistrictMappings_DistrictId] ON [dbo].[BusDistrictMappings] ([DistrictId]);
END
GO

PRINT 'BusDistrictMappings table created successfully!';
GO
