-- ============================================
-- TUMS Phase 1: Seed Buses Data
-- Extracted from districts.json by Period
-- Period IDs: 1=Fajr, 2=Duha, 3=Asr, 4=Maghrib, 5=Isha
-- ============================================

USE transportationDB;
GO

-- ASR Period (Period ID = 3) Buses
INSERT INTO [dbo].[Buses] ([BusId], [BusNumber], [PeriodId], [Capacity], [IsActive], [CreatedAt], [IsDeleted])
SELECT NEWID(), BusNumber, 3, 30, 1, GETUTCDATE(), 0
FROM (VALUES
    (N'1'), (N'2'), (N'3'), (N'4'), (N'5'), (N'6'), (N'7'), (N'8'), (N'9'), (N'10'),
    (N'11'), (N'12'), (N'13'), (N'14'), (N'15'), (N'16'), (N'17'), (N'18'), (N'19'), (N'20'),
    (N'21'), (N'22'), (N'23'), (N'24'), (N'25'), (N'26'), (N'27'), (N'28'), (N'29'), (N'30'),
    (N'31'), (N'32'), (N'33'), (N'41'), (N'42'), (N'43'), (N'44'), (N'45'), (N'46'), (N'47'),
    (N'48'), (N'49'), (N'50'), (N'51'), (N'52'), (N'53'), (N'54'), (N'55'), (N'56'), (N'57'),
    (N'58'), (N'59'), (N'60'), (N'61'), (N'62'), (N'63'), (N'64'), (N'65'), (N'66'), (N'67'),
    (N'68'), (N'69'), (N'70'), (N'71'), (N'72'), (N'73'), (N'74'), (N'91'), (N'94'), (N'408'), (N'410')
) AS Buses(BusNumber)
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[Buses] b 
    WHERE b.BusNumber = Buses.BusNumber AND b.PeriodId = 3
);
GO

-- MAGHRIB Period (Period ID = 4) Buses
INSERT INTO [dbo].[Buses] ([BusId], [BusNumber], [PeriodId], [Capacity], [IsActive], [CreatedAt], [IsDeleted])
SELECT NEWID(), BusNumber, 4, 30, 1, GETUTCDATE(), 0
FROM (VALUES
    (N'301'), (N'302'), (N'303'), (N'304'), (N'305'), (N'306'), (N'307'), (N'308'), (N'309'), (N'310'),
    (N'311'), (N'312'), (N'313'), (N'401'), (N'402'), (N'403'), (N'404'), (N'405'), (N'406'), (N'407'), (N'409')
) AS Buses(BusNumber)
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[Buses] b 
    WHERE b.BusNumber = Buses.BusNumber AND b.PeriodId = 4
);
GO

-- FAJR_DUHA Period (Period ID = 1 for Fajr, 2 for Duha - using 1 as combined)
INSERT INTO [dbo].[Buses] ([BusId], [BusNumber], [PeriodId], [Capacity], [IsActive], [CreatedAt], [IsDeleted])
SELECT NEWID(), BusNumber, 1, 30, 1, GETUTCDATE(), 0
FROM (VALUES
    (N'101'), (N'102'), (N'103'), (N'104'), (N'105'), (N'106'),
    (N'201'), (N'202'), (N'203'), (N'204'), (N'205'), (N'206'), (N'207'), (N'208'), (N'209'), (N'210'),
    (N'211'), (N'212'), (N'213')
) AS Buses(BusNumber)
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[Buses] b 
    WHERE b.BusNumber = Buses.BusNumber AND b.PeriodId = 1
);
GO

-- Verify inserted buses
SELECT PeriodId, COUNT(*) AS BusCount 
FROM [dbo].[Buses] 
GROUP BY PeriodId
ORDER BY PeriodId;
GO

PRINT 'Buses seeded successfully!';
GO
