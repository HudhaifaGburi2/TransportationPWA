-- ============================================
-- TUMS Phase 1: Foundation Schema (Fixed Order)
-- Transportation Attendance System
-- Database: TransportationDB
-- ============================================

USE master;
GO

-- Create database if not exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'transportationDB')
BEGIN
    CREATE DATABASE transportationDB;
END
GO

USE transportationDB;
GO

-- ============================================
-- Drop existing tables in reverse order (for clean recreation)
-- ============================================
IF OBJECT_ID('dbo.AttendanceRecords', 'U') IS NOT NULL DROP TABLE dbo.AttendanceRecords;
IF OBJECT_ID('dbo.AttendanceSessions', 'U') IS NOT NULL DROP TABLE dbo.AttendanceSessions;
IF OBJECT_ID('dbo.StudentBusAssignments', 'U') IS NOT NULL DROP TABLE dbo.StudentBusAssignments;
IF OBJECT_ID('dbo.RegistrationRequests', 'U') IS NOT NULL DROP TABLE dbo.RegistrationRequests;
IF OBJECT_ID('dbo.Students', 'U') IS NOT NULL DROP TABLE dbo.Students;
IF OBJECT_ID('dbo.BusDistrictMappings', 'U') IS NOT NULL DROP TABLE dbo.BusDistrictMappings;
IF OBJECT_ID('dbo.Buses', 'U') IS NOT NULL DROP TABLE dbo.Buses;
IF OBJECT_ID('dbo.Routes', 'U') IS NOT NULL DROP TABLE dbo.Routes;
IF OBJECT_ID('dbo.Locations', 'U') IS NOT NULL DROP TABLE dbo.Locations;
IF OBJECT_ID('dbo.Districts', 'U') IS NOT NULL DROP TABLE dbo.Districts;
IF OBJECT_ID('dbo.AuditLogs', 'U') IS NOT NULL DROP TABLE dbo.AuditLogs;
GO

PRINT 'Dropped existing tables...';
GO

-- ============================================
-- Table 1: Districts (No dependencies)
-- ============================================
CREATE TABLE [dbo].[Districts] (
    [DistrictId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [DistrictNameAr] NVARCHAR(200) NOT NULL,
    [DistrictNameEn] NVARCHAR(200) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] UNIQUEIDENTIFIER NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Districts] PRIMARY KEY CLUSTERED ([DistrictId] ASC)
);
CREATE NONCLUSTERED INDEX [IX_Districts_DistrictNameAr] ON [dbo].[Districts] ([DistrictNameAr]);
CREATE NONCLUSTERED INDEX [IX_Districts_IsActive] ON [dbo].[Districts] ([IsActive]);
CREATE NONCLUSTERED INDEX [IX_Districts_IsDeleted] ON [dbo].[Districts] ([IsDeleted]);
GO
PRINT 'Created Districts table';
GO

-- ============================================
-- Table 2: Locations (No dependencies)
-- ============================================
CREATE TABLE [dbo].[Locations] (
    [LocationId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [LocationCode] NVARCHAR(20) NOT NULL,
    [LocationName] NVARCHAR(100) NOT NULL,
    [LocationType] NVARCHAR(50) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] UNIQUEIDENTIFIER NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED ([LocationId] ASC),
    CONSTRAINT [UQ_Locations_LocationCode] UNIQUE ([LocationCode])
);
CREATE NONCLUSTERED INDEX [IX_Locations_IsActive] ON [dbo].[Locations] ([IsActive]);
CREATE NONCLUSTERED INDEX [IX_Locations_IsDeleted] ON [dbo].[Locations] ([IsDeleted]);
GO
PRINT 'Created Locations table';
GO

-- ============================================
-- Table 3: Routes (No dependencies)
-- ============================================
CREATE TABLE [dbo].[Routes] (
    [RouteId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [RouteName] NVARCHAR(200) NOT NULL,
    [RouteDescription] NVARCHAR(MAX) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] UNIQUEIDENTIFIER NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Routes] PRIMARY KEY CLUSTERED ([RouteId] ASC)
);
CREATE NONCLUSTERED INDEX [IX_Routes_IsActive] ON [dbo].[Routes] ([IsActive]);
CREATE NONCLUSTERED INDEX [IX_Routes_IsDeleted] ON [dbo].[Routes] ([IsDeleted]);
GO
PRINT 'Created Routes table';
GO

-- ============================================
-- Table 4: AuditLogs (No dependencies)
-- ============================================
CREATE TABLE [dbo].[AuditLogs] (
    [AuditId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [UserId] UNIQUEIDENTIFIER NULL,
    [Action] NVARCHAR(100) NOT NULL,
    [EntityType] NVARCHAR(100) NULL,
    [EntityId] UNIQUEIDENTIFIER NULL,
    [OldValues] NVARCHAR(MAX) NULL,
    [NewValues] NVARCHAR(MAX) NULL,
    [IpAddress] NVARCHAR(50) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([AuditId] ASC)
);
CREATE NONCLUSTERED INDEX [IX_AuditLogs_UserId] ON [dbo].[AuditLogs] ([UserId]);
CREATE NONCLUSTERED INDEX [IX_AuditLogs_EntityType] ON [dbo].[AuditLogs] ([EntityType]);
CREATE NONCLUSTERED INDEX [IX_AuditLogs_CreatedAt] ON [dbo].[AuditLogs] ([CreatedAt]);
GO
PRINT 'Created AuditLogs table';
GO

-- ============================================
-- Table 5: Buses (Depends on Routes)
-- ============================================
CREATE TABLE [dbo].[Buses] (
    [BusId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [BusNumber] NVARCHAR(20) NOT NULL,
    [PeriodId] INT NOT NULL,
    [RouteId] UNIQUEIDENTIFIER NULL,
    [DriverName] NVARCHAR(200) NULL,
    [DriverPhoneNumber] NVARCHAR(20) NULL,
    [Capacity] INT NOT NULL DEFAULT 30,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [IsMerged] BIT NOT NULL DEFAULT 0,
    [MergedWithBusId] UNIQUEIDENTIFIER NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] UNIQUEIDENTIFIER NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Buses] PRIMARY KEY CLUSTERED ([BusId] ASC),
    CONSTRAINT [FK_Buses_Routes] FOREIGN KEY ([RouteId]) REFERENCES [dbo].[Routes]([RouteId])
);
ALTER TABLE [dbo].[Buses] ADD CONSTRAINT [FK_Buses_MergedWithBus] FOREIGN KEY ([MergedWithBusId]) REFERENCES [dbo].[Buses]([BusId]);
CREATE NONCLUSTERED INDEX [IX_Buses_PeriodId] ON [dbo].[Buses] ([PeriodId]);
CREATE NONCLUSTERED INDEX [IX_Buses_BusNumber] ON [dbo].[Buses] ([BusNumber]);
CREATE NONCLUSTERED INDEX [IX_Buses_IsActive] ON [dbo].[Buses] ([IsActive]);
CREATE NONCLUSTERED INDEX [IX_Buses_IsDeleted] ON [dbo].[Buses] ([IsDeleted]);
GO
PRINT 'Created Buses table';
GO

-- ============================================
-- Table 6: BusDistrictMappings (Depends on Buses, Districts)
-- ============================================
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
GO
PRINT 'Created BusDistrictMappings table';
GO

-- ============================================
-- Table 7: Students (Depends on Districts)
-- ============================================
CREATE TABLE [dbo].[Students] (
    [StudentId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [StudentUserId] INT NOT NULL,
    [ExternalStudentId] NVARCHAR(50) NOT NULL,
    [FullNameAr] NVARCHAR(200) NOT NULL,
    [HalaqaTypeCode] NVARCHAR(50) NULL,
    [HalaqaSectionId] UNIQUEIDENTIFIER NULL,
    [HalaqaGender] NVARCHAR(20) NULL,
    [PeriodId] INT NULL,
    [AgeGroupId] INT NULL,
    [HalaqaLocationId] INT NULL,
    [TeacherName] NVARCHAR(200) NULL,
    [DistrictId] UNIQUEIDENTIFIER NOT NULL,
    [HomeAddress] NVARCHAR(500) NULL,
    [Latitude] DECIMAL(10, 8) NULL,
    [Longitude] DECIMAL(11, 8) NULL,
    [PhoneNumber] NVARCHAR(20) NULL,
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Active',
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] UNIQUEIDENTIFIER NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([StudentId] ASC),
    CONSTRAINT [FK_Students_Districts] FOREIGN KEY ([DistrictId]) REFERENCES [dbo].[Districts]([DistrictId]),
    CONSTRAINT [UQ_Students_StudentUserId] UNIQUE ([StudentUserId]),
    CONSTRAINT [UQ_Students_ExternalStudentId] UNIQUE ([ExternalStudentId])
);
CREATE NONCLUSTERED INDEX [IX_Students_DistrictId] ON [dbo].[Students] ([DistrictId]);
CREATE NONCLUSTERED INDEX [IX_Students_Status] ON [dbo].[Students] ([Status]);
CREATE NONCLUSTERED INDEX [IX_Students_IsDeleted] ON [dbo].[Students] ([IsDeleted]);
CREATE NONCLUSTERED INDEX [IX_Students_PeriodId] ON [dbo].[Students] ([PeriodId]);
GO
PRINT 'Created Students table';
GO

-- ============================================
-- Table 8: StudentBusAssignments (Depends on Students, Buses)
-- ============================================
CREATE TABLE [dbo].[StudentBusAssignments] (
    [AssignmentId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [StudentId] UNIQUEIDENTIFIER NOT NULL,
    [BusId] UNIQUEIDENTIFIER NOT NULL,
    [TransportType] NVARCHAR(50) NOT NULL DEFAULT 'Both',
    [ArrivalBusId] UNIQUEIDENTIFIER NULL,
    [ReturnBusId] UNIQUEIDENTIFIER NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [AssignedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [AssignedBy] UNIQUEIDENTIFIER NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] UNIQUEIDENTIFIER NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_StudentBusAssignments] PRIMARY KEY CLUSTERED ([AssignmentId] ASC),
    CONSTRAINT [FK_StudentBusAssignments_Students] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students]([StudentId]),
    CONSTRAINT [FK_StudentBusAssignments_Buses] FOREIGN KEY ([BusId]) REFERENCES [dbo].[Buses]([BusId]),
    CONSTRAINT [FK_StudentBusAssignments_ArrivalBus] FOREIGN KEY ([ArrivalBusId]) REFERENCES [dbo].[Buses]([BusId]),
    CONSTRAINT [FK_StudentBusAssignments_ReturnBus] FOREIGN KEY ([ReturnBusId]) REFERENCES [dbo].[Buses]([BusId])
);
CREATE NONCLUSTERED INDEX [IX_StudentBusAssignments_StudentId] ON [dbo].[StudentBusAssignments] ([StudentId]);
CREATE NONCLUSTERED INDEX [IX_StudentBusAssignments_BusId] ON [dbo].[StudentBusAssignments] ([BusId]);
CREATE NONCLUSTERED INDEX [IX_StudentBusAssignments_IsActive] ON [dbo].[StudentBusAssignments] ([IsActive]);
CREATE NONCLUSTERED INDEX [IX_StudentBusAssignments_IsDeleted] ON [dbo].[StudentBusAssignments] ([IsDeleted]);
GO
PRINT 'Created StudentBusAssignments table';
GO

-- ============================================
-- Table 9: RegistrationRequests (Depends on Districts)
-- ============================================
CREATE TABLE [dbo].[RegistrationRequests] (
    [RequestId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [StudentUserId] INT NOT NULL,
    [ExternalStudentId] NVARCHAR(50) NOT NULL,
    [StudentName] NVARCHAR(200) NOT NULL,
    [HalaqaTypeCode] NVARCHAR(50) NULL,
    [HalaqaSectionId] UNIQUEIDENTIFIER NULL,
    [HalaqaGender] NVARCHAR(20) NULL,
    [PeriodId] INT NULL,
    [AgeGroupId] INT NULL,
    [HalaqaLocationId] INT NULL,
    [TeacherName] NVARCHAR(200) NULL,
    [DistrictId] UNIQUEIDENTIFIER NOT NULL,
    [HomeAddress] NVARCHAR(500) NULL,
    [Latitude] DECIMAL(10, 8) NOT NULL,
    [Longitude] DECIMAL(11, 8) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    [RequestedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ReviewedAt] DATETIME2 NULL,
    [ReviewedBy] UNIQUEIDENTIFIER NULL,
    [ReviewNotes] NVARCHAR(1000) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] UNIQUEIDENTIFIER NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_RegistrationRequests] PRIMARY KEY CLUSTERED ([RequestId] ASC),
    CONSTRAINT [FK_RegistrationRequests_Districts] FOREIGN KEY ([DistrictId]) REFERENCES [dbo].[Districts]([DistrictId])
);
CREATE NONCLUSTERED INDEX [IX_RegistrationRequests_StudentUserId] ON [dbo].[RegistrationRequests] ([StudentUserId]);
CREATE NONCLUSTERED INDEX [IX_RegistrationRequests_Status] ON [dbo].[RegistrationRequests] ([Status]);
CREATE NONCLUSTERED INDEX [IX_RegistrationRequests_DistrictId] ON [dbo].[RegistrationRequests] ([DistrictId]);
CREATE NONCLUSTERED INDEX [IX_RegistrationRequests_IsDeleted] ON [dbo].[RegistrationRequests] ([IsDeleted]);
GO
PRINT 'Created RegistrationRequests table';
GO

-- ============================================
-- Table 10: AttendanceSessions (Depends on Buses, Locations)
-- ============================================
CREATE TABLE [dbo].[AttendanceSessions] (
    [SessionId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [SupervisorId] UNIQUEIDENTIFIER NOT NULL,
    [BusId] UNIQUEIDENTIFIER NOT NULL,
    [PeriodId] INT NOT NULL,
    [LocationId] UNIQUEIDENTIFIER NULL,
    [AttendanceDate] DATE NOT NULL,
    [AttendanceType] NVARCHAR(50) NOT NULL,
    [UnregisteredStudentsCount] INT NOT NULL DEFAULT 0,
    [SyncStatus] NVARCHAR(50) NOT NULL DEFAULT 'Synced',
    [CreatedOffline] BIT NOT NULL DEFAULT 0,
    [SyncedAt] DATETIME2 NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] UNIQUEIDENTIFIER NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_AttendanceSessions] PRIMARY KEY CLUSTERED ([SessionId] ASC),
    CONSTRAINT [FK_AttendanceSessions_Buses] FOREIGN KEY ([BusId]) REFERENCES [dbo].[Buses]([BusId]),
    CONSTRAINT [FK_AttendanceSessions_Locations] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Locations]([LocationId]),
    CONSTRAINT [UQ_AttendanceSessions_BusDateType] UNIQUE ([BusId], [AttendanceDate], [AttendanceType])
);
CREATE NONCLUSTERED INDEX [IX_AttendanceSessions_AttendanceDate] ON [dbo].[AttendanceSessions] ([AttendanceDate]);
CREATE NONCLUSTERED INDEX [IX_AttendanceSessions_BusId] ON [dbo].[AttendanceSessions] ([BusId]);
CREATE NONCLUSTERED INDEX [IX_AttendanceSessions_SupervisorId] ON [dbo].[AttendanceSessions] ([SupervisorId]);
CREATE NONCLUSTERED INDEX [IX_AttendanceSessions_SyncStatus] ON [dbo].[AttendanceSessions] ([SyncStatus]);
CREATE NONCLUSTERED INDEX [IX_AttendanceSessions_IsDeleted] ON [dbo].[AttendanceSessions] ([IsDeleted]);
GO
PRINT 'Created AttendanceSessions table';
GO

-- ============================================
-- Table 11: AttendanceRecords (Depends on AttendanceSessions, Students)
-- ============================================
CREATE TABLE [dbo].[AttendanceRecords] (
    [RecordId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [SessionId] UNIQUEIDENTIFIER NOT NULL,
    [StudentId] UNIQUEIDENTIFIER NOT NULL,
    [AttendanceStatus] NVARCHAR(50) NOT NULL DEFAULT 'Present',
    [Notes] NVARCHAR(500) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] UNIQUEIDENTIFIER NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_AttendanceRecords] PRIMARY KEY CLUSTERED ([RecordId] ASC),
    CONSTRAINT [FK_AttendanceRecords_Sessions] FOREIGN KEY ([SessionId]) REFERENCES [dbo].[AttendanceSessions]([SessionId]) ON DELETE CASCADE,
    CONSTRAINT [FK_AttendanceRecords_Students] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Students]([StudentId])
);
CREATE NONCLUSTERED INDEX [IX_AttendanceRecords_SessionId] ON [dbo].[AttendanceRecords] ([SessionId]);
CREATE NONCLUSTERED INDEX [IX_AttendanceRecords_StudentId] ON [dbo].[AttendanceRecords] ([StudentId]);
CREATE NONCLUSTERED INDEX [IX_AttendanceRecords_AttendanceStatus] ON [dbo].[AttendanceRecords] ([AttendanceStatus]);
CREATE NONCLUSTERED INDEX [IX_AttendanceRecords_IsDeleted] ON [dbo].[AttendanceRecords] ([IsDeleted]);
GO
PRINT 'Created AttendanceRecords table';
GO

PRINT '';
PRINT '========================================';
PRINT 'Phase 1 Schema created successfully!';
PRINT '========================================';
GO
