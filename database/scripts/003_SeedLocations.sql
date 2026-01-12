-- ============================================
-- TUMS Phase 1: Seed Locations (Parking Areas)
-- Mosque Basement Parking: A8-A11, B8-B11
-- ============================================

USE transportationDB;
GO

-- Insert Locations (Parking Areas)
MERGE INTO [dbo].[Locations] AS target
USING (VALUES
    -- Section A - Parking Areas
    (N'A8', N'موقف A8', N'Parking'),
    (N'A9', N'موقف A9', N'Parking'),
    (N'A10', N'موقف A10', N'Parking'),
    (N'A11', N'موقف A11', N'Parking'),
    -- Section B - Parking Areas
    (N'B8', N'موقف B8', N'Parking'),
    (N'B9', N'موقف B9', N'Parking'),
    (N'B10', N'موقف B10', N'Parking'),
    (N'B11', N'موقف B11', N'Parking')
) AS source ([LocationCode], [LocationName], [LocationType])
ON target.[LocationCode] = source.[LocationCode]
WHEN NOT MATCHED THEN
    INSERT ([LocationId], [LocationCode], [LocationName], [LocationType], [IsActive], [CreatedAt], [IsDeleted])
    VALUES (NEWID(), source.[LocationCode], source.[LocationName], source.[LocationType], 1, GETUTCDATE(), 0);

GO

-- Verify inserted locations
SELECT * FROM [dbo].[Locations];
GO

PRINT 'Locations seeded successfully!';
GO
