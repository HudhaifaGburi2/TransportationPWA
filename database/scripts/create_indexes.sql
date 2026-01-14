-- Create Indexes for TUMS Phase 1
-- Performance optimization for common queries

-- Districts Indexes
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Districts_IsActive')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Districts_IsActive
    ON [Transportation].[dbo].[Districts] (IsActive)
    INCLUDE (DistrictNameAr, DistrictNameEn);
    PRINT 'Created index IX_Districts_IsActive';
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Districts_DistrictNameAr')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Districts_DistrictNameAr
    ON [Transportation].[dbo].[Districts] (DistrictNameAr);
    PRINT 'Created index IX_Districts_DistrictNameAr';
END;

-- Locations Indexes
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Locations_IsActive')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Locations_IsActive
    ON [Transportation].[dbo].[Locations] (IsActive)
    INCLUDE (LocationCode, LocationName, LocationType);
    PRINT 'Created index IX_Locations_IsActive';
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Locations_LocationCode')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX IX_Locations_LocationCode
    ON [Transportation].[dbo].[Locations] (LocationCode);
    PRINT 'Created index IX_Locations_LocationCode';
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Locations_LocationType')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Locations_LocationType
    ON [Transportation].[dbo].[Locations] (LocationType)
    WHERE LocationType IS NOT NULL;
    PRINT 'Created index IX_Locations_LocationType';
END;

-- AuditLogs Indexes
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_AuditLogs_UserId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_AuditLogs_UserId
    ON [Transportation].[dbo].[AuditLogs] (UserId)
    INCLUDE (Action, CreatedAt);
    PRINT 'Created index IX_AuditLogs_UserId';
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_AuditLogs_CreatedAt')
BEGIN
    CREATE NONCLUSTERED INDEX IX_AuditLogs_CreatedAt
    ON [Transportation].[dbo].[AuditLogs] (CreatedAt DESC)
    INCLUDE (UserId, Action, EntityType);
    PRINT 'Created index IX_AuditLogs_CreatedAt';
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_AuditLogs_EntityType_EntityId')
BEGIN
    CREATE NONCLUSTERED INDEX IX_AuditLogs_EntityType_EntityId
    ON [Transportation].[dbo].[AuditLogs] (EntityType, EntityId)
    INCLUDE (Action, CreatedAt);
    PRINT 'Created index IX_AuditLogs_EntityType_EntityId';
END;

-- RegistrationRequests Indexes (if table exists)
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'RegistrationRequests')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_RegistrationRequests_StudentUserId')
    BEGIN
        CREATE NONCLUSTERED INDEX IX_RegistrationRequests_StudentUserId
        ON [Transportation].[dbo].[RegistrationRequests] (StudentUserId);
        PRINT 'Created index IX_RegistrationRequests_StudentUserId';
    END;

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_RegistrationRequests_Status')
    BEGIN
        CREATE NONCLUSTERED INDEX IX_RegistrationRequests_Status
        ON [Transportation].[dbo].[RegistrationRequests] (Status)
        INCLUDE (StudentUserId, CreatedAt);
        PRINT 'Created index IX_RegistrationRequests_Status';
    END;
END;

PRINT 'All indexes created successfully.';
GO
