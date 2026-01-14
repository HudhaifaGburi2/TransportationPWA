-- Seed Districts Data for TUMS Phase 1
-- Based on requirements: 11 districts

BEGIN TRANSACTION;

-- Clear existing data (if needed)
-- DELETE FROM [Transportation].[dbo].[Districts];

-- Insert Districts
IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Districts] WHERE DistrictNameAr = N'حي العزيزية')
BEGIN
    INSERT INTO [Transportation].[dbo].[Districts] (DistrictId, DistrictNameAr, DistrictNameEn, IsActive, CreatedAt)
    VALUES (NEWID(), N'حي العزيزية', 'Al Aziziyah', 1, GETUTCDATE());
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Districts] WHERE DistrictNameAr = N'حي الشوقية')
BEGIN
    INSERT INTO [Transportation].[dbo].[Districts] (DistrictId, DistrictNameAr, DistrictNameEn, IsActive, CreatedAt)
    VALUES (NEWID(), N'حي الشوقية', 'Al Shawqiyah', 1, GETUTCDATE());
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Districts] WHERE DistrictNameAr = N'حي الزاهر')
BEGIN
    INSERT INTO [Transportation].[dbo].[Districts] (DistrictId, DistrictNameAr, DistrictNameEn, IsActive, CreatedAt)
    VALUES (NEWID(), N'حي الزاهر', 'Al Zahir', 1, GETUTCDATE());
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Districts] WHERE DistrictNameAr = N'حي النسيم')
BEGIN
    INSERT INTO [Transportation].[dbo].[Districts] (DistrictId, DistrictNameAr, DistrictNameEn, IsActive, CreatedAt)
    VALUES (NEWID(), N'حي النسيم', 'Al Naseem', 1, GETUTCDATE());
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Districts] WHERE DistrictNameAr = N'حي الرصيفة')
BEGIN
    INSERT INTO [Transportation].[dbo].[Districts] (DistrictId, DistrictNameAr, DistrictNameEn, IsActive, CreatedAt)
    VALUES (NEWID(), N'حي الرصيفة', 'Al Rusayfah', 1, GETUTCDATE());
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Districts] WHERE DistrictNameAr = N'حي الكعكية')
BEGIN
    INSERT INTO [Transportation].[dbo].[Districts] (DistrictId, DistrictNameAr, DistrictNameEn, IsActive, CreatedAt)
    VALUES (NEWID(), N'حي الكعكية', 'Al Kakiyah', 1, GETUTCDATE());
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Districts] WHERE DistrictNameAr = N'حي العوالي')
BEGIN
    INSERT INTO [Transportation].[dbo].[Districts] (DistrictId, DistrictNameAr, DistrictNameEn, IsActive, CreatedAt)
    VALUES (NEWID(), N'حي العوالي', 'Al Awali', 1, GETUTCDATE());
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Districts] WHERE DistrictNameAr = N'حي الشرائع')
BEGIN
    INSERT INTO [Transportation].[dbo].[Districts] (DistrictId, DistrictNameAr, DistrictNameEn, IsActive, CreatedAt)
    VALUES (NEWID(), N'حي الشرائع', 'Al Sharai', 1, GETUTCDATE());
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Districts] WHERE DistrictNameAr = N'حي الحمراء')
BEGIN
    INSERT INTO [Transportation].[dbo].[Districts] (DistrictId, DistrictNameAr, DistrictNameEn, IsActive, CreatedAt)
    VALUES (NEWID(), N'حي الحمراء', 'Al Hamra', 1, GETUTCDATE());
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Districts] WHERE DistrictNameAr = N'حي التنعيم')
BEGIN
    INSERT INTO [Transportation].[dbo].[Districts] (DistrictId, DistrictNameAr, DistrictNameEn, IsActive, CreatedAt)
    VALUES (NEWID(), N'حي التنعيم', 'Al Taneem', 1, GETUTCDATE());
END;

IF NOT EXISTS (SELECT 1 FROM [Transportation].[dbo].[Districts] WHERE DistrictNameAr = N'حي الهجرة')
BEGIN
    INSERT INTO [Transportation].[dbo].[Districts] (DistrictId, DistrictNameAr, DistrictNameEn, IsActive, CreatedAt)
    VALUES (NEWID(), N'حي الهجرة', 'Al Hijrah', 1, GETUTCDATE());
END;

COMMIT TRANSACTION;

PRINT 'Districts seeded successfully.';
GO
