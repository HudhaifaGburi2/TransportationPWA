IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE TABLE [AuditLogs] (
        [AuditId] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NULL,
        [Action] nvarchar(100) NOT NULL,
        [EntityType] nvarchar(100) NULL,
        [EntityId] uniqueidentifier NULL,
        [OldValues] nvarchar(max) NULL,
        [NewValues] nvarchar(max) NULL,
        [IpAddress] nvarchar(50) NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([AuditId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE TABLE [Districts] (
        [DistrictId] uniqueidentifier NOT NULL,
        [DistrictNameAr] nvarchar(200) NOT NULL,
        [DistrictNameEn] nvarchar(200) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Districts] PRIMARY KEY ([DistrictId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE TABLE [Locations] (
        [LocationId] uniqueidentifier NOT NULL,
        [LocationCode] nvarchar(20) NOT NULL,
        [LocationName] nvarchar(100) NOT NULL,
        [LocationType] nvarchar(50) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Locations] PRIMARY KEY ([LocationId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE TABLE [Routes] (
        [RouteId] uniqueidentifier NOT NULL,
        [RouteName] nvarchar(200) NOT NULL,
        [RouteDescription] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Routes] PRIMARY KEY ([RouteId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE TABLE [RegistrationRequests] (
        [RequestId] uniqueidentifier NOT NULL,
        [StudentUserId] int NOT NULL,
        [ExternalStudentId] nvarchar(50) NOT NULL,
        [StudentName] nvarchar(200) NOT NULL,
        [HalaqaTypeCode] nvarchar(50) NULL,
        [HalaqaSectionId] uniqueidentifier NULL,
        [HalaqaGender] nvarchar(20) NULL,
        [PeriodId] int NULL,
        [AgeGroupId] int NULL,
        [HalaqaLocationId] int NULL,
        [TeacherName] nvarchar(200) NULL,
        [DistrictId] uniqueidentifier NOT NULL,
        [HomeAddress] nvarchar(500) NULL,
        [Latitude] decimal(10,8) NOT NULL,
        [Longitude] decimal(11,8) NOT NULL,
        [Status] nvarchar(50) NOT NULL,
        [RequestedAt] datetime2 NOT NULL,
        [ReviewedAt] datetime2 NULL,
        [ReviewedBy] uniqueidentifier NULL,
        [ReviewNotes] nvarchar(1000) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_RegistrationRequests] PRIMARY KEY ([RequestId]),
        CONSTRAINT [FK_RegistrationRequests_Districts_DistrictId] FOREIGN KEY ([DistrictId]) REFERENCES [Districts] ([DistrictId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE TABLE [Students] (
        [StudentId] uniqueidentifier NOT NULL,
        [StudentUserId] int NOT NULL,
        [ExternalStudentId] nvarchar(50) NOT NULL,
        [FullNameAr] nvarchar(200) NOT NULL,
        [HalaqaTypeCode] nvarchar(50) NULL,
        [HalaqaSectionId] uniqueidentifier NULL,
        [HalaqaGender] nvarchar(20) NULL,
        [PeriodId] int NULL,
        [AgeGroupId] int NULL,
        [HalaqaLocationId] int NULL,
        [TeacherName] nvarchar(200) NULL,
        [DistrictId] uniqueidentifier NOT NULL,
        [HomeAddress] nvarchar(500) NULL,
        [Latitude] decimal(10,8) NULL,
        [Longitude] decimal(11,8) NULL,
        [PhoneNumber] nvarchar(20) NULL,
        [Status] nvarchar(50) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Students] PRIMARY KEY ([StudentId]),
        CONSTRAINT [FK_Students_Districts_DistrictId] FOREIGN KEY ([DistrictId]) REFERENCES [Districts] ([DistrictId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE TABLE [Buses] (
        [BusId] uniqueidentifier NOT NULL,
        [BusNumber] nvarchar(20) NOT NULL,
        [PeriodId] int NOT NULL,
        [RouteId] uniqueidentifier NULL,
        [DriverName] nvarchar(200) NULL,
        [DriverPhoneNumber] nvarchar(20) NULL,
        [Capacity] int NOT NULL,
        [IsActive] bit NOT NULL,
        [IsMerged] bit NOT NULL,
        [MergedWithBusId] uniqueidentifier NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_Buses] PRIMARY KEY ([BusId]),
        CONSTRAINT [FK_Buses_Buses_MergedWithBusId] FOREIGN KEY ([MergedWithBusId]) REFERENCES [Buses] ([BusId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Buses_Routes_RouteId] FOREIGN KEY ([RouteId]) REFERENCES [Routes] ([RouteId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE TABLE [AttendanceSessions] (
        [SessionId] uniqueidentifier NOT NULL,
        [SupervisorId] uniqueidentifier NOT NULL,
        [BusId] uniqueidentifier NOT NULL,
        [PeriodId] int NOT NULL,
        [LocationId] uniqueidentifier NULL,
        [AttendanceDate] date NOT NULL,
        [AttendanceType] nvarchar(50) NOT NULL,
        [UnregisteredStudentsCount] int NOT NULL,
        [SyncStatus] nvarchar(50) NOT NULL,
        [CreatedOffline] bit NOT NULL,
        [SyncedAt] datetime2 NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_AttendanceSessions] PRIMARY KEY ([SessionId]),
        CONSTRAINT [FK_AttendanceSessions_Buses_BusId] FOREIGN KEY ([BusId]) REFERENCES [Buses] ([BusId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AttendanceSessions_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([LocationId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE TABLE [StudentBusAssignments] (
        [Id] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [BusId] uniqueidentifier NOT NULL,
        [TransportType] int NOT NULL,
        [ArrivalBusId] uniqueidentifier NULL,
        [ReturnBusId] uniqueidentifier NULL,
        [IsActive] bit NOT NULL,
        [AssignedAt] datetime2 NOT NULL,
        [AssignedBy] uniqueidentifier NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_StudentBusAssignments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_StudentBusAssignments_Buses_ArrivalBusId] FOREIGN KEY ([ArrivalBusId]) REFERENCES [Buses] ([BusId]),
        CONSTRAINT [FK_StudentBusAssignments_Buses_BusId] FOREIGN KEY ([BusId]) REFERENCES [Buses] ([BusId]) ON DELETE CASCADE,
        CONSTRAINT [FK_StudentBusAssignments_Buses_ReturnBusId] FOREIGN KEY ([ReturnBusId]) REFERENCES [Buses] ([BusId]),
        CONSTRAINT [FK_StudentBusAssignments_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE TABLE [AttendanceRecords] (
        [RecordId] uniqueidentifier NOT NULL,
        [SessionId] uniqueidentifier NOT NULL,
        [StudentId] uniqueidentifier NOT NULL,
        [AttendanceStatus] nvarchar(50) NOT NULL,
        [Notes] nvarchar(500) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] uniqueidentifier NULL,
        CONSTRAINT [PK_AttendanceRecords] PRIMARY KEY ([RecordId]),
        CONSTRAINT [FK_AttendanceRecords_AttendanceSessions_SessionId] FOREIGN KEY ([SessionId]) REFERENCES [AttendanceSessions] ([SessionId]) ON DELETE CASCADE,
        CONSTRAINT [FK_AttendanceRecords_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AttendanceRecords_AttendanceStatus] ON [AttendanceRecords] ([AttendanceStatus]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AttendanceRecords_IsDeleted] ON [AttendanceRecords] ([IsDeleted]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AttendanceRecords_SessionId] ON [AttendanceRecords] ([SessionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AttendanceRecords_StudentId] ON [AttendanceRecords] ([StudentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AttendanceSessions_AttendanceDate] ON [AttendanceSessions] ([AttendanceDate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AttendanceSessions_BusId] ON [AttendanceSessions] ([BusId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_AttendanceSessions_BusId_AttendanceDate_AttendanceType] ON [AttendanceSessions] ([BusId], [AttendanceDate], [AttendanceType]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AttendanceSessions_IsDeleted] ON [AttendanceSessions] ([IsDeleted]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AttendanceSessions_LocationId] ON [AttendanceSessions] ([LocationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AttendanceSessions_SupervisorId] ON [AttendanceSessions] ([SupervisorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AttendanceSessions_SyncStatus] ON [AttendanceSessions] ([SyncStatus]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AuditLogs_CreatedAt] ON [AuditLogs] ([CreatedAt]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AuditLogs_EntityType] ON [AuditLogs] ([EntityType]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_AuditLogs_UserId] ON [AuditLogs] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Buses_BusNumber] ON [Buses] ([BusNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Buses_IsActive] ON [Buses] ([IsActive]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Buses_IsDeleted] ON [Buses] ([IsDeleted]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Buses_MergedWithBusId] ON [Buses] ([MergedWithBusId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Buses_PeriodId] ON [Buses] ([PeriodId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Buses_RouteId] ON [Buses] ([RouteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Districts_DistrictNameAr] ON [Districts] ([DistrictNameAr]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Districts_IsActive] ON [Districts] ([IsActive]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Districts_IsDeleted] ON [Districts] ([IsDeleted]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Locations_IsActive] ON [Locations] ([IsActive]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Locations_IsDeleted] ON [Locations] ([IsDeleted]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Locations_LocationCode] ON [Locations] ([LocationCode]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_RegistrationRequests_DistrictId] ON [RegistrationRequests] ([DistrictId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_RegistrationRequests_ExternalStudentId] ON [RegistrationRequests] ([ExternalStudentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_RegistrationRequests_IsDeleted] ON [RegistrationRequests] ([IsDeleted]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_RegistrationRequests_Status] ON [RegistrationRequests] ([Status]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_RegistrationRequests_StudentUserId] ON [RegistrationRequests] ([StudentUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Routes_IsActive] ON [Routes] ([IsActive]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Routes_IsDeleted] ON [Routes] ([IsDeleted]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_StudentBusAssignments_ArrivalBusId] ON [StudentBusAssignments] ([ArrivalBusId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_StudentBusAssignments_BusId] ON [StudentBusAssignments] ([BusId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_StudentBusAssignments_ReturnBusId] ON [StudentBusAssignments] ([ReturnBusId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_StudentBusAssignments_StudentId] ON [StudentBusAssignments] ([StudentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Students_DistrictId] ON [Students] ([DistrictId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Students_ExternalStudentId] ON [Students] ([ExternalStudentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Students_IsDeleted] ON [Students] ([IsDeleted]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE INDEX [IX_Students_Status] ON [Students] ([Status]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Students_StudentUserId] ON [Students] ([StudentUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112134844_InitialPhase1Schema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260112134844_InitialPhase1Schema', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    ALTER TABLE [StudentBusAssignments] DROP CONSTRAINT [FK_StudentBusAssignments_Buses_ArrivalBusId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    ALTER TABLE [StudentBusAssignments] DROP CONSTRAINT [FK_StudentBusAssignments_Buses_BusId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    ALTER TABLE [StudentBusAssignments] DROP CONSTRAINT [FK_StudentBusAssignments_Buses_ReturnBusId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    ALTER TABLE [StudentBusAssignments] DROP CONSTRAINT [FK_StudentBusAssignments_Students_StudentId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    EXEC sp_rename N'[StudentBusAssignments].[Id]', N'StudentBusAssignmentId', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    CREATE INDEX [IX_StudentBusAssignments_IsActive] ON [StudentBusAssignments] ([IsActive]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    CREATE INDEX [IX_StudentBusAssignments_IsDeleted] ON [StudentBusAssignments] ([IsDeleted]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_StudentBusAssignments_StudentId_BusId] ON [StudentBusAssignments] ([StudentId], [BusId]) WHERE [IsDeleted] = 0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    ALTER TABLE [StudentBusAssignments] ADD CONSTRAINT [FK_StudentBusAssignments_Buses_ArrivalBusId] FOREIGN KEY ([ArrivalBusId]) REFERENCES [Buses] ([BusId]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    ALTER TABLE [StudentBusAssignments] ADD CONSTRAINT [FK_StudentBusAssignments_Buses_BusId] FOREIGN KEY ([BusId]) REFERENCES [Buses] ([BusId]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    ALTER TABLE [StudentBusAssignments] ADD CONSTRAINT [FK_StudentBusAssignments_Buses_ReturnBusId] FOREIGN KEY ([ReturnBusId]) REFERENCES [Buses] ([BusId]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    ALTER TABLE [StudentBusAssignments] ADD CONSTRAINT [FK_StudentBusAssignments_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112154519_UpdatePhase1Schema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260112154519_UpdatePhase1Schema', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260114062509_Phase1Complete'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260114062509_Phase1Complete', N'9.0.0');
END;

COMMIT;
GO

