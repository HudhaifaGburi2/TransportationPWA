-- ============================================
-- TUMS Phase 1: Master Script
-- Run all Phase 1 database scripts in order
-- ============================================

PRINT '========================================';
PRINT 'TUMS Phase 1 Database Setup';
PRINT 'Starting at: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '========================================';
GO

-- Step 1: Create Schema
PRINT '';
PRINT '>>> Step 1: Creating Phase 1 Schema...';
:r 001_CreatePhase1Schema.sql
GO

-- Step 2: Seed Districts
PRINT '';
PRINT '>>> Step 2: Seeding Districts...';
:r 002_SeedDistricts.sql
GO

-- Step 3: Seed Locations
PRINT '';
PRINT '>>> Step 3: Seeding Locations...';
:r 003_SeedLocations.sql
GO

-- Step 4: Seed Buses
PRINT '';
PRINT '>>> Step 4: Seeding Buses...';
:r 004_SeedBuses.sql
GO

-- Step 5: Create Bus-District Mapping
PRINT '';
PRINT '>>> Step 5: Creating Bus-District Mapping...';
:r 005_CreateBusDistrictMapping.sql
GO

PRINT '';
PRINT '========================================';
PRINT 'TUMS Phase 1 Database Setup Complete!';
PRINT 'Finished at: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '========================================';
GO

-- Summary
SELECT 'Districts' AS TableName, COUNT(*) AS RecordCount FROM [dbo].[Districts]
UNION ALL
SELECT 'Locations', COUNT(*) FROM [dbo].[Locations]
UNION ALL
SELECT 'Buses', COUNT(*) FROM [dbo].[Buses]
UNION ALL
SELECT 'Routes', COUNT(*) FROM [dbo].[Routes];
GO
