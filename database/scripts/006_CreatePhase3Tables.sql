-- Phase 3: Student Lifecycle Tables Migration
-- Creates StudentSuspensions, StudentLeaves, StudentTransfers tables

SET NOCOUNT ON;
GO

-- ============================================
-- StudentSuspensions Table
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudentSuspensions')
BEGIN
    CREATE TABLE StudentSuspensions (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        StudentId UNIQUEIDENTIFIER NOT NULL,
        BusId UNIQUEIDENTIFIER NULL,
        Reason NVARCHAR(500) NOT NULL DEFAULT N'غياب لمدة ثلاثة أيام متتالية',
        SuspendedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsReactivated BIT NOT NULL DEFAULT 0,
        ReactivatedAt DATETIME2 NULL,
        NewBusIdAfterReactivation UNIQUEIDENTIFIER NULL,
        ReactivationNotes NVARCHAR(500) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        
        CONSTRAINT FK_StudentSuspensions_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
        CONSTRAINT FK_StudentSuspensions_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId),
        CONSTRAINT FK_StudentSuspensions_NewBus FOREIGN KEY (NewBusIdAfterReactivation) REFERENCES Buses(BusId)
    );
    
    CREATE INDEX IX_StudentSuspensions_StudentId ON StudentSuspensions(StudentId);
    CREATE INDEX IX_StudentSuspensions_IsReactivated ON StudentSuspensions(IsReactivated);
    CREATE INDEX IX_StudentSuspensions_SuspendedAt ON StudentSuspensions(SuspendedAt);
    
    PRINT 'Created StudentSuspensions table';
END
ELSE
BEGIN
    PRINT 'StudentSuspensions table already exists';
END
GO

-- ============================================
-- StudentLeaves Table
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudentLeaves')
BEGIN
    CREATE TABLE StudentLeaves (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        StudentId UNIQUEIDENTIFIER NOT NULL,
        StartDate DATE NOT NULL,
        EndDate DATE NOT NULL,
        Reason NVARCHAR(500) NOT NULL,
        AttachmentUrl NVARCHAR(500) NULL,
        AttachmentFileName NVARCHAR(255) NULL,
        IsApproved BIT NOT NULL DEFAULT 0,
        ApprovedAt DATETIME2 NULL,
        IsCancelled BIT NOT NULL DEFAULT 0,
        CancelledAt DATETIME2 NULL,
        CancellationReason NVARCHAR(500) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        
        CONSTRAINT FK_StudentLeaves_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
        CONSTRAINT CK_StudentLeaves_DateRange CHECK (EndDate >= StartDate)
    );
    
    CREATE INDEX IX_StudentLeaves_StudentId ON StudentLeaves(StudentId);
    CREATE INDEX IX_StudentLeaves_DateRange ON StudentLeaves(StartDate, EndDate);
    CREATE INDEX IX_StudentLeaves_IsApproved ON StudentLeaves(IsApproved);
    CREATE INDEX IX_StudentLeaves_IsCancelled ON StudentLeaves(IsCancelled);
    
    PRINT 'Created StudentLeaves table';
END
ELSE
BEGIN
    PRINT 'StudentLeaves table already exists';
END
GO

-- ============================================
-- StudentTransfers Table
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudentTransfers')
BEGIN
    CREATE TABLE StudentTransfers (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        StudentId UNIQUEIDENTIFIER NOT NULL,
        FromBusId UNIQUEIDENTIFIER NOT NULL,
        ToBusId UNIQUEIDENTIFIER NOT NULL,
        Reason NVARCHAR(500) NULL,
        TransferredAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        EffectiveDate DATE NOT NULL DEFAULT CAST(GETUTCDATE() AS DATE),
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        
        CONSTRAINT FK_StudentTransfers_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
        CONSTRAINT FK_StudentTransfers_FromBus FOREIGN KEY (FromBusId) REFERENCES Buses(BusId),
        CONSTRAINT FK_StudentTransfers_ToBus FOREIGN KEY (ToBusId) REFERENCES Buses(BusId),
        CONSTRAINT CK_StudentTransfers_DifferentBuses CHECK (FromBusId <> ToBusId)
    );
    
    CREATE INDEX IX_StudentTransfers_StudentId ON StudentTransfers(StudentId);
    CREATE INDEX IX_StudentTransfers_FromBusId ON StudentTransfers(FromBusId);
    CREATE INDEX IX_StudentTransfers_ToBusId ON StudentTransfers(ToBusId);
    CREATE INDEX IX_StudentTransfers_TransferredAt ON StudentTransfers(TransferredAt);
    
    PRINT 'Created StudentTransfers table';
END
ELSE
BEGIN
    PRINT 'StudentTransfers table already exists';
END
GO

PRINT 'Phase 3 migration completed successfully';
GO
