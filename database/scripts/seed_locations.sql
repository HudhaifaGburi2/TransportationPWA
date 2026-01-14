-- Seed Locations Data for TUMS Phase 1
-- Parking areas A8-B11

BEGIN TRANSACTION;

-- Insert Parking Locations
IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Locations] WHERE LocationCode = 'A8')
BEGIN
    INSERT INTO [Transportation].[dbo].[Locations] (LocationId, LocationCode, LocationName, LocationType, IsActive)
    VALUES (NEWID(), 'A8', N'موقف A8', 'Parking', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Locations] WHERE LocationCode = 'A9')
BEGIN
    INSERT INTO [Transportation].[dbo].[Locations] (LocationId, LocationCode, LocationName, LocationType, IsActive)
    VALUES (NEWID(), 'A9', N'موقف A9', 'Parking', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Locations] WHERE LocationCode = 'A10')
BEGIN
    INSERT INTO [Transportation].[dbo].[Locations] (LocationId, LocationCode, LocationName, LocationType, IsActive)
    VALUES (NEWID(), 'A10', N'موقف A10', 'Parking', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Locations] WHERE LocationCode = 'A11')
BEGIN
    INSERT INTO [Transportation].[dbo].[Locations] (LocationId, LocationCode, LocationName, LocationType, IsActive)
    VALUES (NEWID(), 'A11', N'موقف A11', 'Parking', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Locations] WHERE LocationCode = 'B8')
BEGIN
    INSERT INTO [Transportation].[dbo].[Locations] (LocationId, LocationCode, LocationName, LocationType, IsActive)
    VALUES (NEWID(), 'B8', N'موقف B8', 'Parking', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Locations] WHERE LocationCode = 'B9')
BEGIN
    INSERT INTO [Transportation].[dbo].[Locations] (LocationId, LocationCode, LocationName, LocationType, IsActive)
    VALUES (NEWID(), 'B9', N'موقف B9', 'Parking', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Locations] WHERE LocationCode = 'B10')
BEGIN
    INSERT INTO [Transportation].[dbo].[Locations] (LocationId, LocationCode, LocationName, LocationType, IsActive)
    VALUES (NEWID(), 'B10', N'موقف B10', 'Parking', 1);
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Locations] WHERE LocationCode = 'B11')
BEGIN
    INSERT INTO [Transportation].[dbo].[Locations] (LocationId, LocationCode, LocationName, LocationType, IsActive)
    VALUES (NEWID(), 'B11', N'موقف B11', 'Parking', 1);
END;

COMMIT TRANSACTION;

PRINT 'Locations seeded successfully.';
GO
