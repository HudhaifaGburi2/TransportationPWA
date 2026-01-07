# Transport Unit Management System (TUMS)
## Complete Technical Specification Document

---

## Table of Contents
1. [Project Overview](#1-project-overview)
2. [Technology Stack](#2-technology-stack)
3. [System Architecture](#3-system-architecture)
4. [Project Structure](#4-project-structure)
5. [Database Schema](#5-database-schema)
6. [Development Phases](#6-development-phases)
7. [Implementation Guidelines](#7-implementation-guidelines)

---

## 1. Project Overview

### 1.1 System Purpose
A Progressive Web Application for managing student transportation services for Quran and Islamic Studies centers, supporting offline-first attendance tracking, student registration, bus management, and comprehensive reporting.

### 1.2 Core Features
- Student self-registration with GPS location
- Bus and route management across 5 daily periods
- Offline-first attendance tracking
- Student lifecycle management (suspension, reactivation, leave)
- Multi-role access (Admin, Registration Staff, Supervisors)
- Real-time and historical reporting
- Special operations (bus merging, summer programs)

---

## 2. Technology Stack

### 2.1 Frontend
- **Framework**: Vue 3 (Composition API)
- **Build Tool**: Vite
- **State Management**: Pinia
- **UI Framework**: Tailwind CSS + DaisyUI
- **PWA**: Workbox
- **Offline Storage**: Dexie.js (IndexedDB wrapper)
- **HTTP Client**: Axios
- **Maps**: Leaflet.js or Google Maps API
- **Charts**: Chart.js

### 2.2 Backend
- **.NET**: .NET 8 Web API
- **ORM**: Entity Framework Core 8
- **Authentication**: JWT (JSON Web Tokens)
- **Database**: SQL Server 2019+
- **Architecture**: Clean Architecture
- **Patterns**: Repository, Unit of Work, CQRS (optional)

### 2.3 DevOps & Tools
- **Version Control**: Git
- **API Documentation**: Swagger/OpenAPI
- **Testing**: xUnit, Moq, Vue Test Utils
- **Logging**: Serilog
- **Validation**: FluentValidation

---

## 3. System Architecture

### 3.1 Clean Architecture Layers

```
┌─────────────────────────────────────────┐
│         Presentation Layer              │
│    (Vue 3 PWA - API Controllers)        │
├─────────────────────────────────────────┤
│         Application Layer               │
│  (Use Cases, DTOs, Interfaces, CQRS)   │
├─────────────────────────────────────────┤
│           Domain Layer                  │
│     (Entities, Value Objects,           │
│      Domain Services, Interfaces)       │
├─────────────────────────────────────────┤
│       Infrastructure Layer              │
│  (EF Core, Repositories, Services,      │
│   External APIs, File Storage)          │
└─────────────────────────────────────────┘
```

### 3.2 Architecture Principles
- **Dependency Rule**: Dependencies point inward (outer layers depend on inner layers)
- **Domain-Centric**: Business logic in Domain layer
- **Interface Segregation**: Small, focused interfaces
- **Separation of Concerns**: Each layer has distinct responsibilities

---

## 4. Project Structure

### 4.1 Backend Structure (.NET)

```
TUMS.Solution/
│
├── src/
│   ├── TUMS.Domain/
│   │   ├── Entities/
│   │   │   ├── Student.cs
│   │   │   ├── Bus.cs
│   │   │   ├── Period.cs
│   │   │   ├── District.cs
│   │   │   ├── Location.cs
│   │   │   ├── Route.cs
│   │   │   ├── AttendanceSession.cs
│   │   │   ├── AttendanceRecord.cs
│   │   │   ├── Supervisor.cs
│   │   │   ├── RegistrationRequest.cs
│   │   │   ├── StudentLeave.cs
│   │   │   ├── StudentSuspension.cs
│   │   │   ├── BusMerge.cs
│   │   │   ├── AuditLog.cs
│   │   │   └── User.cs
│   │   │
│   │   ├── ValueObjects/
│   │   │   ├── Address.cs
│   │   │   ├── GeoLocation.cs
│   │   │   ├── PhoneNumber.cs
│   │   │   └── TimeSlot.cs
│   │   │
│   │   ├── Enums/
│   │   │   ├── PeriodType.cs
│   │   │   ├── AttendanceStatus.cs
│   │   │   ├── StudentStatus.cs
│   │   │   ├── RequestStatus.cs
│   │   │   ├── UserRole.cs
│   │   │   └── Gender.cs
│   │   │
│   │   ├── Interfaces/
│   │   │   ├── IEntity.cs
│   │   │   ├── IAuditable.cs
│   │   │   └── ISoftDelete.cs
│   │   │
│   │   └── Exceptions/
│   │       ├── DomainException.cs
│   │       ├── NotFoundException.cs
│   │       └── ValidationException.cs
│   │
│   ├── TUMS.Application/
│   │   ├── Interfaces/
│   │   │   ├── Repositories/
│   │   │   │   ├── IStudentRepository.cs
│   │   │   │   ├── IBusRepository.cs
│   │   │   │   ├── IAttendanceRepository.cs
│   │   │   │   ├── IRegistrationRequestRepository.cs
│   │   │   │   ├── IDistrictRepository.cs
│   │   │   │   ├── ILocationRepository.cs
│   │   │   │   └── IUnitOfWork.cs
│   │   │   │
│   │   │   └── Services/
│   │   │       ├── IAuthenticationService.cs
│   │   │       ├── IStudentService.cs
│   │   │       ├── IAttendanceService.cs
│   │   │       ├── IBusService.cs
│   │   │       └── IReportService.cs
│   │   │
│   │   ├── DTOs/
│   │   │   ├── Authentication/
│   │   │   │   ├── LoginRequestDto.cs
│   │   │   │   ├── LoginResponseDto.cs
│   │   │   │   └── UserDto.cs
│   │   │   │
│   │   │   ├── Student/
│   │   │   │   ├── StudentDto.cs
│   │   │   │   ├── CreateStudentDto.cs
│   │   │   │   ├── UpdateStudentDto.cs
│   │   │   │   └── StudentDetailDto.cs
│   │   │   │
│   │   │   ├── Bus/
│   │   │   │   ├── BusDto.cs
│   │   │   │   ├── BusDetailDto.cs
│   │   │   │   └── BusStudentListDto.cs
│   │   │   │
│   │   │   ├── Attendance/
│   │   │   │   ├── AttendanceSessionDto.cs
│   │   │   │   ├── AttendanceRecordDto.cs
│   │   │   │   ├── BulkAttendanceDto.cs
│   │   │   │   └── AttendanceSyncDto.cs
│   │   │   │
│   │   │   ├── Registration/
│   │   │   │   ├── RegistrationRequestDto.cs
│   │   │   │   ├── CreateRegistrationRequestDto.cs
│   │   │   │   └── ReviewRegistrationDto.cs
│   │   │   │
│   │   │   └── Reports/
│   │   │       ├── DailyAttendanceReportDto.cs
│   │   │       ├── MonthlyReportDto.cs
│   │   │       └── AnnualReportDto.cs
│   │   │
│   │   ├── UseCases/
│   │   │   ├── Students/
│   │   │   │   ├── Commands/
│   │   │   │   │   ├── CreateStudentCommand.cs
│   │   │   │   │   ├── SuspendStudentCommand.cs
│   │   │   │   │   ├── ReactivateStudentCommand.cs
│   │   │   │   │   └── TransferStudentCommand.cs
│   │   │   │   │
│   │   │   │   └── Queries/
│   │   │   │       ├── GetStudentByIdQuery.cs
│   │   │   │       ├── GetStudentsByBusQuery.cs
│   │   │   │       └── SearchStudentsQuery.cs
│   │   │   │
│   │   │   ├── Attendance/
│   │   │   │   ├── Commands/
│   │   │   │   │   ├── RecordAttendanceCommand.cs
│   │   │   │   │   └── SyncAttendanceCommand.cs
│   │   │   │   │
│   │   │   │   └── Queries/
│   │   │   │       ├── GetAttendanceByBusQuery.cs
│   │   │   │       └── GetWeeklyAttendanceQuery.cs
│   │   │   │
│   │   │   └── Registration/
│   │   │       ├── Commands/
│   │   │       │   ├── CreateRegistrationRequestCommand.cs
│   │   │       │   └── ReviewRegistrationCommand.cs
│   │   │       │
│   │   │       └── Queries/
│   │   │           └── GetPendingRegistrationsQuery.cs
│   │   │
│   │   ├── Validators/
│   │   │   ├── CreateStudentValidator.cs
│   │   │   ├── AttendanceValidator.cs
│   │   │   └── RegistrationRequestValidator.cs
│   │   │
│   │   ├── Mappings/
│   │   │   └── AutoMapperProfile.cs
│   │   │
│   │   └── Common/
│   │       ├── Result.cs
│   │       ├── PagedResult.cs
│   │       └── Constants.cs
│   │
│   ├── TUMS.Infrastructure/
│   │   ├── Persistence/
│   │   │   ├── TUMSDbContext.cs
│   │   │   ├── Configurations/
│   │   │   │   ├── StudentConfiguration.cs
│   │   │   │   ├── BusConfiguration.cs
│   │   │   │   ├── AttendanceConfiguration.cs
│   │   │   │   └── ... (other entity configurations)
│   │   │   │
│   │   │   ├── Repositories/
│   │   │   │   ├── BaseRepository.cs
│   │   │   │   ├── StudentRepository.cs
│   │   │   │   ├── BusRepository.cs
│   │   │   │   ├── AttendanceRepository.cs
│   │   │   │   └── ... (other repositories)
│   │   │   │
│   │   │   ├── UnitOfWork.cs
│   │   │   │
│   │   │   └── Migrations/
│   │   │
│   │   ├── Identity/
│   │   │   ├── JwtSettings.cs
│   │   │   ├── JwtTokenGenerator.cs
│   │   │   └── PasswordHasher.cs
│   │   │
│   │   ├── Services/
│   │   │   ├── AuthenticationService.cs
│   │   │   ├── StudentService.cs
│   │   │   ├── AttendanceService.cs
│   │   │   ├── BusService.cs
│   │   │   └── ReportService.cs
│   │   │
│   │   ├── ExternalServices/
│   │   │   └── HalaqaViewService.cs (reads from central DB view)
│   │   │
│   │   └── DependencyInjection.cs
│   │
│   └── TUMS.API/
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── StudentsController.cs
│       │   ├── BusesController.cs
│       │   ├── AttendanceController.cs
│       │   ├── RegistrationController.cs
│       │   ├── ReportsController.cs
│       │   ├── DistrictsController.cs
│       │   └── LookupsController.cs
│       │
│       ├── Middleware/
│       │   ├── ErrorHandlingMiddleware.cs
│       │   ├── JwtMiddleware.cs
│       │   └── RequestLoggingMiddleware.cs
│       │
│       ├── Filters/
│       │   ├── AuthorizeAttribute.cs
│       │   └── ValidateModelAttribute.cs
│       │
│       ├── Extensions/
│       │   ├── ServiceCollectionExtensions.cs
│       │   └── ApplicationBuilderExtensions.cs
│       │
│       ├── appsettings.json
│       ├── appsettings.Development.json
│       ├── Program.cs
│       └── Startup.cs (optional in .NET 8)
│
└── tests/
    ├── TUMS.Domain.Tests/
    ├── TUMS.Application.Tests/
    ├── TUMS.Infrastructure.Tests/
    └── TUMS.API.Tests/
```

### 4.2 Frontend Structure (Vue 3)

```
tums-frontend/
│
├── public/
│   ├── manifest.json
│   ├── robots.txt
│   └── icons/
│
├── src/
│   ├── main.ts
│   ├── App.vue
│   │
│   ├── router/
│   │   ├── index.ts
│   │   ├── guards/
│   │   │   ├── auth.guard.ts
│   │   │   └── role.guard.ts
│   │   │
│   │   └── routes/
│   │       ├── auth.routes.ts
│   │       ├── admin.routes.ts
│   │       ├── registration.routes.ts
│   │       └── supervisor.routes.ts
│   │
│   ├── stores/
│   │   ├── auth.store.ts
│   │   ├── student.store.ts
│   │   ├── bus.store.ts
│   │   ├── attendance.store.ts
│   │   ├── offline.store.ts
│   │   └── ui.store.ts
│   │
│   ├── services/
│   │   ├── api/
│   │   │   ├── axios.config.ts
│   │   │   ├── auth.service.ts
│   │   │   ├── student.service.ts
│   │   │   ├── bus.service.ts
│   │   │   ├── attendance.service.ts
│   │   │   ├── registration.service.ts
│   │   │   └── report.service.ts
│   │   │
│   │   ├── offline/
│   │   │   ├── db.ts (Dexie configuration)
│   │   │   ├── sync.service.ts
│   │   │   └── cache.service.ts
│   │   │
│   │   └── utils/
│   │       ├── location.service.ts
│   │       └── notification.service.ts
│   │
│   ├── views/
│   │   ├── auth/
│   │   │   ├── LoginView.vue
│   │   │   └── ForgotPasswordView.vue
│   │   │
│   │   ├── dashboard/
│   │   │   ├── AdminDashboardView.vue
│   │   │   ├── RegistrationDashboardView.vue
│   │   │   └── SupervisorDashboardView.vue
│   │   │
│   │   ├── students/
│   │   │   ├── StudentListView.vue
│   │   │   ├── StudentDetailView.vue
│   │   │   ├── StudentFormView.vue
│   │   │   ├── SuspendedStudentsView.vue
│   │   │   └── StudentRegistrationView.vue (self-registration)
│   │   │
│   │   ├── buses/
│   │   │   ├── BusListView.vue
│   │   │   ├── BusDetailView.vue
│   │   │   ├── BusMergeView.vue
│   │   │   └── BusByPeriodView.vue
│   │   │
│   │   ├── attendance/
│   │   │   ├── AttendanceView.vue (supervisor)
│   │   │   ├── WeeklyReviewView.vue
│   │   │   └── AttendanceHistoryView.vue
│   │   │
│   │   ├── registration/
│   │   │   ├── PendingRequestsView.vue
│   │   │   └── ReviewRequestView.vue
│   │   │
│   │   ├── reports/
│   │   │   ├── DailyReportView.vue
│   │   │   ├── MonthlyReportView.vue
│   │   │   └── AnnualReportView.vue
│   │   │
│   │   └── settings/
│   │       ├── ProfileView.vue
│   │       └── SystemSettingsView.vue
│   │
│   ├── components/
│   │   ├── common/
│   │   │   ├── AppHeader.vue
│   │   │   ├── AppSidebar.vue
│   │   │   ├── AppFooter.vue
│   │   │   ├── LoadingSpinner.vue
│   │   │   ├── ErrorMessage.vue
│   │   │   ├── ConfirmDialog.vue
│   │   │   ├── DataTable.vue
│   │   │   ├── Pagination.vue
│   │   │   └── SearchBar.vue
│   │   │
│   │   ├── students/
│   │   │   ├── StudentCard.vue
│   │   │   ├── StudentForm.vue
│   │   │   ├── StudentStatusBadge.vue
│   │   │   └── StudentSearchForm.vue
│   │   │
│   │   ├── buses/
│   │   │   ├── BusCard.vue
│   │   │   ├── BusRouteMap.vue
│   │   │   └── BusPeriodSelector.vue
│   │   │
│   │   ├── attendance/
│   │   │   ├── AttendanceForm.vue
│   │   │   ├── AttendanceStatusButton.vue
│   │   │   ├── SyncStatusIndicator.vue
│   │   │   └── OfflineIndicator.vue
│   │   │
│   │   └── maps/
│   │       ├── LocationPicker.vue
│   │       └── RouteViewer.vue
│   │
│   ├── composables/
│   │   ├── useAuth.ts
│   │   ├── useOfflineSync.ts
│   │   ├── useGeolocation.ts
│   │   ├── useNotification.ts
│   │   └── useDebounce.ts
│   │
│   ├── types/
│   │   ├── api.types.ts
│   │   ├── student.types.ts
│   │   ├── bus.types.ts
│   │   ├── attendance.types.ts
│   │   └── common.types.ts
│   │
│   ├── utils/
│   │   ├── constants.ts
│   │   ├── helpers.ts
│   │   ├── validators.ts
│   │   └── date.utils.ts
│   │
│   ├── assets/
│   │   ├── styles/
│   │   │   ├── main.css
│   │   │   └── tailwind.css
│   │   │
│   │   └── images/
│   │
│   └── sw.ts (Service Worker)
│
├── .env
├── .env.production
├── vite.config.ts
├── tailwind.config.js
├── tsconfig.json
├── package.json
└── README.md
```

---

## 5. Database Schema

### 5.1 Core Tables

```sql
-- ============================================
-- Users & Authentication will come from central db
-- ============================================



CREATE TABLE Districts (
    DistrictId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    DistrictNameAr NVARCHAR(200) NOT NULL,
    DistrictNameEn NVARCHAR(200),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    INDEX IX_Districts_Name (DistrictNameAr)
);

CREATE TABLE Locations (
    LocationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    LocationCode NVARCHAR(20) NOT NULL UNIQUE, -- A8, A9, B10, etc.
    LocationName NVARCHAR(100) NOT NULL,
    LocationType NVARCHAR(50), -- Parking, Mosque, etc.
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE Routes (
    RouteId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RouteName NVARCHAR(200) NOT NULL,
    RouteDescription NVARCHAR(MAX),
    IsActive BIT NOT NULL DEFAULT 1
);

-- ============================================
-- Students
-- ============================================

CREATE TABLE Students (
    StudentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    
    -- From Central View (Read-Only Reference)
    ExternalUserId INT, -- User_ID from central system
    ExternalStudentId NVARCHAR(50), -- Student_ID like S154352
    
    -- Student Information
    FullNameAr NVARCHAR(200) NOT NULL,
    FullNameEn NVARCHAR(200),
    RegistrationNumber NVARCHAR(50) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20),
    
    -- Halaqa Information (from central view)
    HalaqaType NVARCHAR(100),
    Halaqa_section_Id UNIQUEIDENTIFIER,
    TeacherName NVARCHAR(200),
    ClassCircleNumber NVARCHAR(50),
    
    -- Transport-Specific Information
    DistrictId UNIQUEIDENTIFIER NOT NULL,
    HomeAddress NVARCHAR(500),
    Latitude DECIMAL(10, 8),
    Longitude DECIMAL(11, 8),
    
    -- Status
    Status NVARCHAR(50) NOT NULL DEFAULT 'Active', -- Active, Suspended, OnLeave
    
    -- Audit
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    CreatedBy UNIQUEIDENTIFIER,
    UpdatedBy UNIQUEIDENTIFIER,
    IsDeleted BIT NOT NULL DEFAULT 0,
    
    CONSTRAINT FK_Students_Districts FOREIGN KEY (DistrictId) REFERENCES Districts(DistrictId),
    CONSTRAINT FK_Students_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(UserId),
    CONSTRAINT FK_Students_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES Users(UserId),
    
    INDEX IX_Students_Status (Status),
    INDEX IX_Students_District (DistrictId),
    INDEX IX_Students_RegistrationNumber (RegistrationNumber),
    INDEX IX_Students_ExternalStudentId (ExternalStudentId)
);

-- ============================================
-- Buses
-- ============================================

CREATE TABLE Buses (
    BusId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    BusNumber NVARCHAR(20) NOT NULL,
    PeriodId UNIQUEIDENTIFIER NOT NULL,
    RouteId UNIQUEIDENTIFIER,
    
    -- Driver Information
    DriverName NVARCHAR(200),
    DriverPhoneNumber NVARCHAR(20),
    
    -- Capacity
    Capacity INT NOT NULL DEFAULT 30,
    
    -- Status
    IsActive BIT NOT NULL DEFAULT 1,
    IsMerged BIT NOT NULL DEFAULT 0,
    MergedWithBusId UNIQUEIDENTIFIER, -- Reference to parent bus if merged
    
    -- Audit
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    
    CONSTRAINT FK_Buses_Periods FOREIGN KEY (PeriodId) REFERENCES Periods(PeriodId),
    CONSTRAINT FK_Buses_Routes FOREIGN KEY (RouteId) REFERENCES Routes(RouteId),
    CONSTRAINT FK_Buses_MergedWith FOREIGN KEY (MergedWithBusId) REFERENCES Buses(BusId),
    
    INDEX IX_Buses_Period (PeriodId),
    INDEX IX_Buses_BusNumber (BusNumber)
);

CREATE TABLE BusDistricts (
    BusDistrictId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    BusId UNIQUEIDENTIFIER NOT NULL,
    DistrictId UNIQUEIDENTIFIER NOT NULL,
    
    CONSTRAINT FK_BusDistricts_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId) ON DELETE CASCADE,
    CONSTRAINT FK_BusDistricts_District FOREIGN KEY (DistrictId) REFERENCES Districts(DistrictId),
    CONSTRAINT UQ_BusDistricts UNIQUE (BusId, DistrictId)
);

-- ============================================
-- Student-Bus Assignments
-- ============================================

CREATE TABLE StudentBusAssignments (
    AssignmentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    StudentId UNIQUEIDENTIFIER NOT NULL,
    BusId UNIQUEIDENTIFIER NOT NULL,
    
    -- Transport Type
    TransportType NVARCHAR(50) NOT NULL, -- Arrival, Return, Both
    
    -- Different buses for arrival/return
    ArrivalBusId UNIQUEIDENTIFIER,
    ReturnBusId UNIQUEIDENTIFIER,
    
    -- Status
    IsActive BIT NOT NULL DEFAULT 1,
    
    -- Audit
    AssignedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    AssignedBy UNIQUEIDENTIFIER,
    
    CONSTRAINT FK_StudentBus_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    CONSTRAINT FK_StudentBus_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId),
    CONSTRAINT FK_StudentBus_ArrivalBus FOREIGN KEY (ArrivalBusId) REFERENCES Buses(BusId),
    CONSTRAINT FK_StudentBus_ReturnBus FOREIGN KEY (ReturnBusId) REFERENCES Buses(BusId),
    CONSTRAINT FK_StudentBus_AssignedBy FOREIGN KEY (AssignedBy) REFERENCES Users(UserId),
    
    INDEX IX_StudentBus_Student (StudentId),
    INDEX IX_StudentBus_Bus (BusId),
    INDEX IX_StudentBus_Active (IsActive)
);

-- ============================================
-- Student Lifecycle Management
-- ============================================

CREATE TABLE StudentSuspensions (
    SuspensionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    StudentId UNIQUEIDENTIFIER NOT NULL,
    BusId UNIQUEIDENTIFIER,
    
    Reason NVARCHAR(500) NOT NULL DEFAULT N'غياب لمدة ثلاثة أيام متتالية', -- Absent for 3 days
    SuspendedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    SuspendedBy UNIQUEIDENTIFIER NOT NULL,
    
    -- Reactivation
    IsReactivated BIT NOT NULL DEFAULT 0,
    ReactivatedAt DATETIME2,
    ReactivatedBy UNIQUEIDENTIFIER,
    ReactivatedToBusId UNIQUEIDENTIFIER,
    
    CONSTRAINT FK_Suspension_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    CONSTRAINT FK_Suspension_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId),
    CONSTRAINT FK_Suspension_SuspendedBy FOREIGN KEY (SuspendedBy) REFERENCES Users(UserId),
    CONSTRAINT FK_Suspension_ReactivatedBy FOREIGN KEY (ReactivatedBy) REFERENCES Users(UserId),
    CONSTRAINT FK_Suspension_ReactivatedBus FOREIGN KEY (ReactivatedToBusId) REFERENCES Buses(BusId),
    
    INDEX IX_Suspension_Student (StudentId),
    INDEX IX_Suspension_IsReactivated (IsReactivated)
);

CREATE TABLE StudentLeaves (
    LeaveId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    StudentId UNIQUEIDENTIFIER NOT NULL,
    
    Reason NVARCHAR(500) NOT NULL,
    AttachmentUrl NVARCHAR(500),
    
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    
    -- Approval
    RequestedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    RequestedBy UNIQUEIDENTIFIER NOT NULL,
    ApprovedAt DATETIME2,
    ApprovedBy UNIQUEIDENTIFIER,
    
    Status NVARCHAR(50) NOT NULL DEFAULT 'Active', -- Active, Completed, Cancelled
    
    CONSTRAINT FK_Leave_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    CONSTRAINT FK_Leave_RequestedBy FOREIGN KEY (RequestedBy) REFERENCES Users(UserId),
    CONSTRAINT FK_Leave_ApprovedBy FOREIGN KEY (ApprovedBy) REFERENCES Users(UserId),
    
    INDEX IX_Leave_Student (StudentId),
    INDEX IX_Leave_Dates (StartDate, EndDate),
    INDEX IX_Leave_Status (Status)
);

CREATE TABLE StudentTransfers (
    TransferId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    StudentId UNIQUEIDENTIFIER NOT NULL,
    
    FromBusId UNIQUEIDENTIFIER NOT NULL,
    ToBusId UNIQUEIDENTIFIER NOT NULL,
    
    Reason NVARCHAR(500),
    TransferredAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    TransferredBy UNIQUEIDENTIFIER NOT NULL,
    
    CONSTRAINT FK_Transfer_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    CONSTRAINT FK_Transfer_FromBus FOREIGN KEY (FromBusId) REFERENCES Buses(BusId),
    CONSTRAINT FK_Transfer_ToBus FOREIGN KEY (ToBusId) REFERENCES Buses(BusId),
    CONSTRAINT FK_Transfer_TransferredBy FOREIGN KEY (TransferredBy) REFERENCES Users(UserId),
    
    INDEX IX_Transfer_Student (StudentId)
);

-- ============================================
-- Registration Requests (Self-Registration)
-- ============================================

CREATE TABLE RegistrationRequests (
    RequestId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    
    -- From Central View
    ExternalStudentId NVARCHAR(50),
    
    -- Student Information (Pre-filled, Read-Only)
    StudentName NVARCHAR(200) NOT NULL,
    RegistrationNumber NVARCHAR(50) NOT NULL,
    HalaqaType NVARCHAR(100),
    TeacherName NVARCHAR(200),
    
    -- Transport Request Details
    DistrictId UNIQUEIDENTIFIER NOT NULL,
    HomeAddress NVARCHAR(500),
    Latitude DECIMAL(10, 8) NOT NULL,
    Longitude DECIMAL(11, 8) NOT NULL,
    
    -- Request Status
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending', -- Pending, Approved, Rejected
    
    -- Review
    ReviewedAt DATETIME2,
    ReviewedBy UNIQUEIDENTIFIER,
    ReviewNotes NVARCHAR(1000),
    
    -- Assignment (if approved)
    AssignedBusId UNIQUEIDENTIFIER,
    
    -- Audit
    RequestedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_RegRequest_District FOREIGN KEY (DistrictId) REFERENCES Districts(DistrictId),
    CONSTRAINT FK_RegRequest_ReviewedBy FOREIGN KEY (ReviewedBy) REFERENCES Users(UserId),
    CONSTRAINT FK_RegRequest_AssignedBus FOREIGN KEY (AssignedBusId) REFERENCES Buses(BusId),
    
    INDEX IX_RegRequest_Status (Status),
    INDEX IX_RegRequest_District (DistrictId),
    INDEX IX_RegRequest_ExternalStudentId (ExternalStudentId)
);

-- ============================================
-- Attendance Management
-- ============================================

CREATE TABLE Supervisors (
    SupervisorId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL UNIQUE,
    LocationId UNIQUEIDENTIFIER,
    
    IsActive BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT FK_Supervisor_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Supervisor_Location FOREIGN KEY (LocationId) REFERENCES Locations(LocationId)
);

CREATE TABLE SupervisorBusAssignments (
    AssignmentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SupervisorId UNIQUEIDENTIFIER NOT NULL,
    BusId UNIQUEIDENTIFIER NOT NULL,
    
    IsActive BIT NOT NULL DEFAULT 1,
    AssignedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_SupervisorBus_Supervisor FOREIGN KEY (SupervisorId) REFERENCES Supervisors(SupervisorId),
    CONSTRAINT FK_SupervisorBus_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId),
    CONSTRAINT UQ_SupervisorBus UNIQUE (SupervisorId, BusId)
);

CREATE TABLE AttendanceSessions (
    SessionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    
    SupervisorId UNIQUEIDENTIFIER NOT NULL,
    BusId UNIQUEIDENTIFIER NOT NULL,
    PeriodId UNIQUEIDENTIFIER NOT NULL,
    LocationId UNIQUEIDENTIFIER,
    
    AttendanceDate DATE NOT NULL,
    AttendanceType NVARCHAR(50) NOT NULL, -- Arrival, Return
    
    UnregisteredStudentsCount INT NOT NULL DEFAULT 0,
    
    -- Sync Status
    SyncStatus NVARCHAR(50) NOT NULL DEFAULT 'Synced', -- Pending, Syncing, Synced, Failed
    
    -- Audit
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedOffline BIT NOT NULL DEFAULT 0,
    SyncedAt DATETIME2,
    
    CONSTRAINT FK_AttSession_Supervisor FOREIGN KEY (SupervisorId) REFERENCES Supervisors(SupervisorId),
    CONSTRAINT FK_AttSession_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId),
    CONSTRAINT FK_AttSession_Period FOREIGN KEY (PeriodId) REFERENCES Periods(PeriodId),
    CONSTRAINT FK_AttSession_Location FOREIGN KEY (LocationId) REFERENCES Locations(LocationId),
    
    INDEX IX_AttSession_Date (AttendanceDate),
    INDEX IX_AttSession_Bus (BusId),
    INDEX IX_AttSession_Supervisor (SupervisorId),
    INDEX IX_AttSession_SyncStatus (SyncStatus),
    CONSTRAINT UQ_AttSession UNIQUE (BusId, AttendanceDate, AttendanceType)
);

CREATE TABLE AttendanceRecords (
    RecordId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SessionId UNIQUEIDENTIFIER NOT NULL,
    StudentId UNIQUEIDENTIFIER NOT NULL,
    
    AttendanceStatus NVARCHAR(50) NOT NULL DEFAULT 'Present', -- Present, Absent, Late, Excused
    
    Notes NVARCHAR(500),
    
    CONSTRAINT FK_AttRecord_Session FOREIGN KEY (SessionId) REFERENCES AttendanceSessions(SessionId) ON DELETE CASCADE,
    CONSTRAINT FK_AttRecord_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    
    INDEX IX_AttRecord_Session (SessionId),
    INDEX IX_AttRecord_Student (StudentId),
    INDEX IX_AttRecord_Status (AttendanceStatus)
);

-- ============================================
-- Special Operations
-- ============================================

CREATE TABLE BusMergeOperations (
    MergeId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    
    MergeName NVARCHAR(200) NOT NULL,
    MergeReason NVARCHAR(500),
    
    StartDate DATE NOT NULL,
    EndDate DATE,
    
    IsActive BIT NOT NULL DEFAULT 1,
    IsRestored BIT NOT NULL DEFAULT 0,
    
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    RestoredAt DATETIME2,
    RestoredBy UNIQUEIDENTIFIER,
    
    CONSTRAINT FK_BusMerge_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(UserId),
    CONSTRAINT FK_BusMerge_RestoredBy FOREIGN KEY (RestoredBy) REFERENCES Users(UserId)
);

CREATE TABLE BusMergeDetails (
    MergeDetailId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    MergeId UNIQUEIDENTIFIER NOT NULL,
    
    SourceBusId UNIQUEIDENTIFIER NOT NULL,
    TargetBusId UNIQUEIDENTIFIER NOT NULL,
    
    CONSTRAINT FK_MergeDetail_Merge FOREIGN KEY (MergeId) REFERENCES BusMergeOperations(MergeId) ON DELETE CASCADE,
    CONSTRAINT FK_MergeDetail_SourceBus FOREIGN KEY (SourceBusId) REFERENCES Buses(BusId),
    CONSTRAINT FK_MergeDetail_TargetBus FOREIGN KEY (TargetBusId) REFERENCES Buses(BusId)
);

CREATE TABLE SummerProgramData (
    SummerDataId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    
    Year INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    
    CONSTRAINT FK_SummerData_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(UserId),
    INDEX IX_SummerData_Year (Year),
    CONSTRAINT UQ_SummerData_Year UNIQUE (Year)
);

-- Link summer-specific student assignments
CREATE TABLE SummerStudentAssignments (
    SummerAssignmentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SummerDataId UNIQUEIDENTIFIER NOT NULL,
    
    StudentId UNIQUEIDENTIFIER, -- NULL for temporary summer-only students
    TemporaryStudentName NVARCHAR(200),
    TemporaryStudentPhone NVARCHAR(20),
    
    BusId UNIQUEIDENTIFIER NOT NULL,
    
    IsActive BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT FK_SummerAssignment_SummerData FOREIGN KEY (SummerDataId) REFERENCES SummerProgramData(SummerDataId) ON DELETE CASCADE,
    CONSTRAINT FK_SummerAssignment_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    CONSTRAINT FK_SummerAssignment_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId)
);

-- ============================================
-- Audit & Logging
-- ============================================

CREATE TABLE AuditLogs (
    AuditId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    
    UserId UNIQUEIDENTIFIER,
    Action NVARCHAR(100) NOT NULL, -- Create, Update, Delete, Login, etc.
    EntityType NVARCHAR(100), -- Student, Bus, Attendance, etc.
    EntityId UNIQUEIDENTIFIER,
    
    OldValues NVARCHAR(MAX), -- JSON
    NewValues NVARCHAR(MAX), -- JSON
    
    IpAddress NVARCHAR(50),
    UserAgent NVARCHAR(500),
    
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_AuditLog_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    
    INDEX IX_AuditLog_User (UserId),
    INDEX IX_AuditLog_Entity (EntityType, EntityId),
    INDEX IX_AuditLog_Date (CreatedAt)
);
```

### 5.2 Initial Data Seed

```sql
-- ============================================
-- Seed Data
-- ============================================

-- Periods
INSERT INTO Periods (PeriodCode, PeriodNameAr, PeriodNameEn, StartTime, EndTime, DisplayOrder) VALUES
('FAJR', N'الفجر', 'Fajr', '04:30:00', '06:00:00', 1),
('DUHA', N'الضحى', 'Duha', '08:00:00', '10:00:00', 2),
('ASR', N'العصر', 'Asr', '15:00:00', '17:00:00', 3),
('MAGHRIB', N'المغرب', 'Maghrib', '17:30:00', '19:00:00', 4),
('ISHA', N'العشاء', 'Isha', '20:00:00', '21:30:00', 5);

-- Locations (Parking Areas)
INSERT INTO Locations (LocationCode, LocationName, LocationType) VALUES
('A8', N'موقف أ٨', 'Parking'),
('A9', N'موقف أ٩', 'Parking'),
('A10', N'موقف أ١٠', 'Parking'),
('A11', N'موقف أ١١', 'Parking'),
('B8', N'موقف ب٨', 'Parking'),
('B9', N'موقف ب٩', 'Parking'),
('B10', N'موقف ب١٠', 'Parking'),
('B11', N'موقف ب١١', 'Parking');

-- Sample Districts (from your document)
INSERT INTO Districts (DistrictNameAr, DistrictNameEn) VALUES
(N'العزيزية', 'Al Aziziyah'),
(N'الصالحية', 'As Salihiyah'),
(N'الربوة', 'Ar Rabwah'),
(N'النصر', 'An Nasr'),
(N'الخالدية', 'Al Khalidiyah'),
(N'العنابس', 'Al Anabis'),
(N'السحمان', 'As Suhman'),
(N'الحزام', 'Al Hizam'),
(N'العوالي', 'Al Awali'),
(N'الكردي', 'Al Kurdi'),
(N'السيح', 'As Saih');

-- Default Admin User (Password: Admin@123)
INSERT INTO Users (Username, PasswordHash, FullName, Email, Role) VALUES
('admin', 'AQAAAAEAACcQAAAAEJ...', 'System Administrator', 'admin@tums.local', 'Admin');
```

### 5.3 Views for Reporting

```sql
-- View: Daily Attendance Summary
CREATE VIEW vw_DailyAttendanceSummary AS
SELECT 
    s.AttendanceDate,
    p.PeriodNameAr AS Period,
    b.BusNumber,
    COUNT(r.RecordId) AS TotalStudents,
    SUM(CASE WHEN r.AttendanceStatus = 'Present' THEN 1 ELSE 0 END) AS PresentCount,
    SUM(CASE WHEN r.AttendanceStatus = 'Absent' THEN 1 ELSE 0 END) AS AbsentCount,
    SUM(CASE WHEN r.AttendanceStatus = 'Late' THEN 1 ELSE 0 END) AS LateCount,
    SUM(CASE WHEN r.AttendanceStatus = 'Excused' THEN 1 ELSE 0 END) AS ExcusedCount
FROM AttendanceSessions s
INNER JOIN Buses b ON s.BusId = b.BusId
INNER JOIN Periods p ON s.PeriodId = p.PeriodId
LEFT JOIN AttendanceRecords r ON s.SessionId = r.SessionId
GROUP BY s.AttendanceDate, p.PeriodNameAr, b.BusNumber, p.DisplayOrder
GO

-- View: Student Bus Information
CREATE VIEW vw_StudentBusInfo AS
SELECT 
    st.StudentId,
    st.FullNameAr AS StudentName,
    st.RegistrationNumber,
    st.PhoneNumber,
    st.Status,
    d.DistrictNameAr AS District,
    b.BusNumber,
    p.PeriodNameAr AS Period,
    sba.TransportType,
    ab.BusNumber AS ArrivalBusNumber,
    rb.BusNumber AS ReturnBusNumber
FROM Students st
LEFT JOIN StudentBusAssignments sba ON st.StudentId = sba.StudentId AND sba.IsActive = 1
LEFT JOIN Buses b ON sba.BusId = b.BusId
LEFT JOIN Buses ab ON sba.ArrivalBusId = ab.BusId
LEFT JOIN Buses rb ON sba.ReturnBusId = rb.BusId
LEFT JOIN Periods p ON b.PeriodId = p.PeriodId
LEFT JOIN Districts d ON st.DistrictId = d.DistrictId
WHERE st.IsDeleted = 0
GO
```

---

## 6. Development Phases

### **Phase 1: Foundation & Core Setup** (Weeks 1-3)

#### **Sprint 1.1: Infrastructure Setup** (Week 1)
**Backend:**
- ✅ Set up .NET 8 solution structure (Domain, Application, Infrastructure, API)
- ✅ Configure EF Core with SQL Server
- ✅ Implement base entities (IEntity, IAuditable, ISoftDelete)
- ✅ Set up Repository pattern and Unit of Work
- ✅ Configure AutoMapper
- ✅ Set up FluentValidation
- ✅ Configure Serilog for logging
- ✅ Set up Swagger/OpenAPI

**Frontend:**
- ✅ Initialize Vue 3 + Vite project
- ✅ Configure Tailwind CSS + DaisyUI
- ✅ Set up Vue Router
- ✅ Configure Pinia stores
- ✅ Set up Axios with interceptors
- ✅ Configure PWA with Workbox
- ✅ Set up Dexie.js for IndexedDB

**Database:**
- ✅ Create initial database schema
- ✅ Create migration scripts
- ✅ Seed master data (Periods, Locations)

**Deliverables:**
- Working development environment
- Database with initial schema
- Basic project structure for both frontend and backend

---

#### **Sprint 1.2: Authentication & Authorization** (Week 2)
**Backend:**
- ✅ Implement JWT token generation and validation
- ✅ Create User entity and repository
- ✅ Implement authentication service
- ✅ Create AuthController (Login, Refresh
, Logout)
- ✅ Implement role-based authorization middleware
- ✅ Add password hashing service
- ✅ Create audit logging infrastructure

**Frontend:**
- ✅ Create login page
- ✅ Implement auth store (Pinia)
- ✅ Create auth service (API calls)
- ✅ Set up JWT token management (storage, refresh)
- ✅ Implement route guards
- ✅ Create role-based UI navigation

**Deliverables:**
- Working authentication system
- Role-based access control
- Secure API endpoints

---

#### **Sprint 1.3: Master Data Management** (Week 3)
**Backend:**
- ✅ Implement Districts CRUD (controller, service, repository)
- ✅ Implement Periods management (read-only)
- ✅ Implement Locations management
- ✅ Create lookups endpoints

**Frontend:**
- ✅ Create admin dashboard layout
- ✅ Build Districts management UI
- ✅ Build Locations management UI
- ✅ Create reusable components (DataTable, Pagination, SearchBar)

**Database:**
- ✅ Seed Districts data from requirements document
- ✅ Create indexes for performance

**Deliverables:**
- Complete master data management
- Admin interface for configurations

---

### **Phase 2: Bus Management** (Weeks 4-5)

#### **Sprint 2.1: Bus Core Features** (Week 4)
**Backend:**
- ✅ Implement Bus entity and repository
- ✅ Create Route entity and management
- ✅ Implement BusService with CRUD operations
- ✅ Create BusesController
- ✅ Implement bus-district associations
- ✅ Add validation rules

**Frontend:**
- ✅ Create bus list view (grouped by period)
- ✅ Build bus detail view
- ✅ Create bus form (add/edit)
- ✅ Implement bus card component
- ✅ Add period selector component
- ✅ Display driver information

**Deliverables:**
- Complete bus management system
- Period-based bus organization

---

#### **Sprint 2.2: Bus Operations & Reporting** (Week 5)
**Backend:**
- ✅ Implement bus statistics endpoints
- ✅ Create bus capacity validation
- ✅ Add bus utilization reporting
- ✅ Implement search and filtering

**Frontend:**
- ✅ Create bus statistics dashboard
- ✅ Build route visualization (map integration)
- ✅ Add capacity indicators
- ✅ Implement search and filters

**Deliverables:**
- Bus reporting features
- Visual route display
- Capacity management

---

### **Phase 3: Student Management** (Weeks 6-8)

#### **Sprint 3.1: Student Registration from Central System** (Week 6)
**Backend:**
- ✅ Create integration service for central DB view (vw_Student_Halaqa_Teacher_information_Transportation_Dep)
- ✅ Implement Student entity and repository
- ✅ Create StudentService
- ✅ Implement student search (by name, registration number, phone)
- ✅ Create StudentsController

**Frontend:**
- ✅ Build student search interface
- ✅ Create student detail view
- ✅ Display Halaqa/Teacher information from central system
- ✅ Show student status badges

**Deliverables:**
- Student data integration with central system
- Student search and viewing

---

#### **Sprint 3.2: Student-Bus Assignment** (Week 7)
**Backend:**
- ✅ Implement StudentBusAssignment entity
- ✅ Create assignment service with validation
- ✅ Support different arrival/return buses
- ✅ Implement transfer functionality
- ✅ Add capacity validation

**Frontend:**
- ✅ Create student assignment interface
- ✅ Build bus student list view
- ✅ Implement transfer wizard
- ✅ Visual indicators for split transport (arrival/return)
- ✅ Show warnings for capacity

**Deliverables:**
- Student bus assignment system
- Transfer capabilities
- Split transport support

---

#### **Sprint 3.3: Student Lifecycle Management** (Week 8)
**Backend:**
- ✅ Implement StudentSuspension entity and service
- ✅ Create suspension workflow
- ✅ Implement reactivation process
- ✅ Create StudentLeave entity and management
- ✅ Implement StudentTransfer entity and history

**Frontend:**
- ✅ Create suspended students list view
- ✅ Build suspension dialog with reason
- ✅ Implement reactivation workflow
- ✅ Create leave management interface
- ✅ Display student history timeline

**Deliverables:**
- Complete student lifecycle management
- Suspension and reactivation workflows
- Leave management system

---

### **Phase 4: Self-Registration System** (Weeks 9-10)

#### **Sprint 4.1: Student Self-Registration** (Week 9)
**Backend:**
- ✅ Implement RegistrationRequest entity
- ✅ Create public registration endpoint (no auth)
- ✅ Integrate with central view for student data
- ✅ Implement GPS coordinates storage
- ✅ Create validation rules

**Frontend:**
- ✅ Build public registration form (mobile-optimized)
- ✅ Implement GPS location capture
- ✅ Create map-based location picker
- ✅ Display read-only student data from central system
- ✅ Add district selector
- ✅ Form validation and submission

**Deliverables:**
- Public student registration interface
- GPS location capture
- Mobile-friendly PWA form

---

#### **Sprint 4.2: Admin Review & Approval** (Week 10)
**Backend:**
- ✅ Create registration review service
- ✅ Implement approve/reject workflow
- ✅ Create automatic student record on approval
- ✅ Implement bus assignment logic
- ✅ Add notification system (optional)

**Frontend:**
- ✅ Create pending requests dashboard
- ✅ Build request review interface
- ✅ Show student location on map
- ✅ Implement approve/reject actions
- ✅ Add bus assignment during approval

**Deliverables:**
- Admin review dashboard
- Registration approval workflow
- Automatic student creation

---

### **Phase 5: Offline-First Attendance System** (Weeks 11-14)

#### **Sprint 5.1: Offline Infrastructure** (Week 11)
**Backend:**
- ✅ Create Supervisor entity and repository
- ✅ Implement AttendanceSession entity
- ✅ Create AttendanceRecord entity
- ✅ Build sync endpoint with idempotency
- ✅ Implement conflict resolution

**Frontend:**
- ✅ Configure Dexie.js schema for attendance
- ✅ Create sync service
- ✅ Implement sync queue management
- ✅ Build offline detection
- ✅ Create sync status indicators

**Deliverables:**
- Offline database structure
- Sync infrastructure
- Conflict resolution system

---

#### **Sprint 5.2: Supervisor Attendance Interface** (Week 12)
**Frontend:**
- ✅ Create supervisor dashboard
- ✅ Build attendance capture interface
- ✅ Implement default "all present" functionality
- ✅ Create fast bulk attendance entry
- ✅ Add unregistered student counter
- ✅ Optimize for touch and speed (< 5 seconds per bus)

**Backend:**
- ✅ Create supervisor assignment endpoints
- ✅ Implement bus-supervisor linking
- ✅ Add attendance validation

**Deliverables:**
- High-performance attendance UI
- Offline-first capture
- Sub-5-second entry target

---

#### **Sprint 5.3: Attendance Synchronization** (Week 13)
**Frontend:**
- ✅ Implement automatic sync on reconnect
- ✅ Add manual sync button
- ✅ Create sync status dashboard
- ✅ Build retry mechanism with exponential backoff
- ✅ Handle sync errors gracefully

**Backend:**
- ✅ Implement batch sync endpoint
- ✅ Add sync logging
- ✅ Create duplicate detection
- ✅ Implement sync audit trail

**Deliverables:**
- Reliable sync system
- No data loss guarantee
- Sync monitoring

---

#### **Sprint 5.4: Weekly Attendance Review** (Week 14)
**Backend:**
- ✅ Create weekly attendance query endpoints
- ✅ Implement leave status checking
- ✅ Build attendance analytics

**Frontend:**
- ✅ Create weekly review interface
- ✅ Display attendance by date/bus/period
- ✅ Visual indicators for students on leave
- ✅ Implement direct suspension from review
- ✅ Add filters and search

**Deliverables:**
- Weekly attendance cleanup tool
- Leave-aware absence tracking
- Quick suspension workflow

---

### **Phase 6: Special Operations** (Weeks 15-16)

#### **Sprint 6.1: Bus Merging** (Week 15)
**Backend:**
- ✅ Implement BusMergeOperation entity
- ✅ Create merge service with validation
- ✅ Implement merge execution
- ✅ Build restore functionality
- ✅ Handle student reassignments

**Frontend:**
- ✅ Create bus merge interface
- ✅ Build merge wizard
- ✅ Show route compatibility
- ✅ Display merged bus status
- ✅ Implement restore operation

**Deliverables:**
- Bus merging system
- Temporary merge support
- Easy restoration

---

#### **Sprint 6.2: Summer Program Mode** (Week 16)
**Backend:**
- ✅ Implement SummerProgramData entity
- ✅ Create summer mode activation
- ✅ Implement data isolation
- ✅ Support temporary students
- ✅ Build summer-specific reporting

**Frontend:**
- ✅ Create summer program toggle
- ✅ Build summer student management
- ✅ Show mode indicator throughout UI
- ✅ Implement summer-specific views

**Deliverables:**
- Summer program mode
- Data isolation
- Temporary student support

---

### **Phase 7: Reporting & Analytics** (Weeks 17-19)

#### **Sprint 7.1: Daily Reports** (Week 17)
**Backend:**
- ✅ Create daily attendance summary service
- ✅ Implement bus attendance report
- ✅ Build supervisor performance report
- ✅ Create suspension/reactivation report
- ✅ Add registration requests report
- ✅ Implement PDF/Excel export

**Frontend:**
- ✅ Create daily reports dashboard
- ✅ Build attendance summary view
- ✅ Implement date range selector
- ✅ Add export buttons
- ✅ Create print-friendly layouts

**Deliverables:**
- Daily operational reports
- Export capabilities

---

#### **Sprint 7.2: Monthly Reports** (Week 18)
**Backend:**
- ✅ Implement monthly trend analysis
- ✅ Create bus utilization metrics
- ✅ Build district coverage report
- ✅ Add supervisor activity tracking
- ✅ Create suspension/leave statistics

**Frontend:**
- ✅ Build monthly reports interface
- ✅ Add data visualizations (Chart.js)
- ✅ Create trend charts
- ✅ Implement comparison views

**Deliverables:**
- Monthly trend reports
- Visual analytics
- Performance metrics

---

#### **Sprint 7.3: Annual Reports** (Week 19)
**Backend:**
- ✅ Create annual summary service
- ✅ Implement year-over-year comparisons
- ✅ Build strategic planning reports
- ✅ Add forecasting data

**Frontend:**
- ✅ Create annual reports dashboard
- ✅ Build comprehensive visualizations
- ✅ Implement drill-down capability
- ✅ Create executive summary view

**Deliverables:**
- Annual strategic reports
- Long-term analytics
- Executive summaries

---

### **Phase 8: Testing, Optimization & Deployment** (Weeks 20-22)

#### **Sprint 8.1: Testing & Quality Assurance** (Week 20)
**Backend:**
- ✅ Write unit tests (xUnit)
- ✅ Create integration tests
- ✅ Perform load testing
- ✅ Security audit
- ✅ API documentation review

**Frontend:**
- ✅ Write component tests (Vue Test Utils)
- ✅ E2E testing (Cypress or Playwright)
- ✅ PWA offline testing
- ✅ Cross-browser testing
- ✅ Mobile responsiveness testing

**Deliverables:**
- Comprehensive test coverage
- Performance benchmarks
- Security validation

---

#### **Sprint 8.2: Performance Optimization** (Week 21)
**Backend:**
- ✅ Database query optimization
- ✅ Add caching layer (Redis optional)
- ✅ Optimize API response times
- ✅ Implement database indexes review
- ✅ Add compression

**Frontend:**
- ✅ Code splitting and lazy loading
- ✅ PWA caching strategy optimization
- ✅ Image optimization
- ✅ Bundle size reduction
- ✅ Service worker optimization

**Deliverables:**
- Performance improvements
- Optimized user experience
- Faster load times

---

#### **Sprint 8.3: Deployment & Documentation** (Week 22)
**Infrastructure:**
- ✅ Set up production environment
- ✅ Configure CI/CD pipeline
- ✅ Set up monitoring (Application Insights or similar)
- ✅ Configure backups
- ✅ SSL/TLS setup

**Documentation:**
- ✅ API documentation (Swagger)
- ✅ User manual
- ✅ Admin guide
- ✅ Deployment guide
- ✅ Maintenance procedures

**Training:**
- ✅ Admin training sessions
- ✅ Registration staff training
- ✅ Supervisor training
- ✅ Create training videos

**Deliverables:**
- Production-ready system
- Complete documentation
- Trained users
- Support procedures

---

## 7. Implementation Guidelines

### 7.1 Clean Architecture Implementation

#### **Domain Layer (TUMS.Domain)**
```csharp
// Example: Student Entity
public class Student : BaseEntity, IAuditable, ISoftDelete
{
    // From Central System
    public int? ExternalUserId { get; set; }
    public string ExternalStudentId { get; set; }
    
    // Student Information
    public string FullNameAr { get; set; }
    public string FullNameEn { get; set; }
    public string RegistrationNumber { get; set; }
    public string PhoneNumber { get; set; }
    
    // Halaqa Information
    public string HalaqaType { get; set; }
    public string TeacherName { get; set; }
    public string ClassCircleNumber { get; set; }
    
    // Transport Information
    public Guid DistrictId { get; set; }
    public District District { get; set; }
    public string HomeAddress { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    
    // Status
    public StudentStatus Status { get; set; }
    
    // Navigation Properties
    public ICollection<StudentBusAssignment> BusAssignments { get; set; }
    public ICollection<StudentSuspension> Suspensions { get; set; }
    public ICollection<StudentLeave> Leaves { get; set; }
    
    // IAuditable
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    
    // ISoftDelete
    public bool IsDeleted { get; set; }
}
```

#### **Application Layer (TUMS.Application)**
```csharp
// Example: Use Case Implementation
public class CreateStudentCommand : IRequest<Result<StudentDto>>
{
    public string RegistrationNumber { get; set; }
    public Guid DistrictId { get; set; }
    // ... other properties
}

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Result<StudentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public async Task<Result<StudentDto>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        // Business logic here
        var student = new Student { /* map properties */ };
        
        await _unitOfWork.Students.AddAsync(student);
        await _unitOfWork.CompleteAsync();
        
        return Result<StudentDto>.Success(_mapper.Map<StudentDto>(student));
    }
}
```

#### **Infrastructure Layer (TUMS.Infrastructure)**
```csharp
// Repository Implementation
public class StudentRepository : BaseRepository<Student>, IStudentRepository
{
    public StudentRepository(TUMSDbContext context) : base(context) { }
    
    public async Task<Student> GetByRegistrationNumberAsync(string registrationNumber)
    {
        return await _context.Students
            .Include(s => s.District)
            .FirstOrDefaultAsync(s => s.RegistrationNumber == registrationNumber && !s.IsDeleted);
    }
    
    public async Task<IEnumerable<Student>> GetByBusIdAsync(Guid busId)
    {
        return await _context.Students
            .Include(s => s.BusAssignments)
            .Where(s => s.BusAssignments.Any(ba => ba.BusId == busId && ba.IsActive) && !s.IsDeleted)
            .ToListAsync();
    }
}

// Unit of Work Implementation
public class UnitOfWork : IUnitOfWork
{
    private readonly TUMSDbContext _context;
    
    public IStudentRepository Students { get; }
    public IBusRepository Buses { get; }
    public IAttendanceRepository Attendance { get; }
    // ... other repositories
    
    public UnitOfWork(TUMSDbContext context)
    {
        _context = context;
        Students = new StudentRepository(_context);
        Buses = new BusRepository(_context);
        // ... initialize other repositories
    }
    
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}
```

### 7.2 JWT Authentication Implementation

```csharp
// JWT Token Generator
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    
    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

// API Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });
```

### 7.3 Frontend Offline Sync Implementation

```typescript
// Dexie Database Configuration
import Dexie, { Table } from 'dexie';

export interface AttendanceSession {
  sessionId: string;
  supervisorId: string;
  busId: string;
  periodId: string;
  attendanceDate: string;
  attendanceType: 'Arrival' | 'Return';
  status: 'draft' | 'pending_sync' | 'synced' | 'error';
  createdAt: string;
}

export interface AttendanceRecord {
  recordId: string;
  sessionId: string;
  studentId: string;
  attendanceStatus: 'Present' | 'Absent' | 'Late' | 'Excused';
}

export interface SyncQueue {
  queueId: string;
  entityType: string;
  entityId: string;
  retryCount: number;
  lastAttemptAt?: string;
  syncStatus: 'pending' | 'syncing' | 'failed';
}

class AttendanceDB extends Dexie {
  attendanceSessions!: Table<AttendanceSession, string>;
  attendanceRecords!: Table<AttendanceRecord, string>;
  syncQueue!: Table<SyncQueue, string>;

  constructor() {
    super('transport_attendance_db');
    this.version(1).stores({
      attendanceSessions: 'sessionId, supervisorId, busId, status',
      attendanceRecords: 'recordId, sessionId, studentId',
      syncQueue: 'queueId, entityType, syncStatus'
    });
  }
}

export const db = new AttendanceDB();

// Sync Service
export class SyncService {
  private isOnline = navigator.onLine;
  
  async saveAttendance(sessionData: AttendanceSession, records: AttendanceRecord[]) {
    // Save to IndexedDB
    await db.attendanceSessions.add(sessionData);
    await db.attendanceRecords.bulkAdd(records);
    
    // Add to sync queue
    await db.syncQueue.add({
      queueId: crypto.randomUUID(),
      entityType: 'attendance_session',
      entityId: sessionData.sessionId,
      retryCount: 0,
      syncStatus: 'pending'
    });
    
    // Attempt immediate sync if online
    if (this.isOnline) {
      await this.syncPendingAttendance();
    }
  }
  
  async syncPendingAttendance() {
    const pendingItems = await db.syncQueue
      .where('syncStatus').equals('pending')
      .toArray();
    
    for (const item of pendingItems) {
      try {
        await this.syncItem(item);
      } catch (error) {
        console.error('Sync failed:', error);
        await this.handleSyncError(item);
      }
    }
  }
  
  private async syncItem(item: SyncQueue) {
    // Mark as syncing
    await db.syncQueue.update(item.queueId, { syncStatus: 'syncing' });
    
    // Get session and records
    const session = await db.attendanceSessions.get(item.entityId);
    const records = await db.attendanceRecords
      .where('sessionId').equals(item.entityId)
      .toArray();
    
    // Send to API
    const response = await api.post('/api/attendance/sync', {
      sessionId: session.sessionId,
      ...session,
      records
    });
    
    if (response.status === 200) {
      // Mark as synced
      await db.attendanceSessions.update(session.sessionId, { status: 'synced' });
      await db.syncQueue.delete(item.queueId);
    }
  }
  
  private async handleSyncError(item: SyncQueue) {
    const retryCount = item.retryCount + 1;
    
    if (retryCount < 5) {
      await db.syncQueue.update(item.queueId, {
        syncStatus: 'pending',
        retryCount,
        lastAttemptAt: new Date().toISOString()
      });
    } else {
      await db.syncQueue.update(item.queueId, { syncStatus: 'failed' });
    }
  }
}
```

### 7.4 API Response Patterns

```csharp
// Standardized API Response
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
    
    public static ApiResponse<T> SuccessResult(T data, string message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message ?? "Operation completed successfully"
        };
    }
    
    public static ApiResponse<T> ErrorResult(string error)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Errors = new List<string> { error }
        };
    }
}

// Controller Example
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<StudentDto>>> GetById(Guid id)
    {
        var result = await _studentService.GetByIdAsync(id);
        
        if (!result.IsSuccess)
            return NotFound(ApiResponse<StudentDto>.ErrorResult(result.Error));
        
        return Ok(ApiResponse<StudentDto>.SuccessResult(result.Value));
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin,RegistrationStaff")]
    public async Task<ActionResult<ApiResponse<StudentDto>>> Create(CreateStudentDto dto)
    {
        var result = await _studentService.CreateAsync(dto);
        
        if (!result.IsSuccess)
            return BadRequest(ApiResponse<StudentDto>.ErrorResult(result.Error));
        
        return CreatedAtAction(nameof(GetById), 
            new { id = result.Value.StudentId }, 
            ApiResponse<StudentDto>.SuccessResult(result.Value));
    }
}
```

---

## 8. Deployment Checklist

### 8.1 Pre-Deployment
- [ ] All tests passing (unit, integration, E2E)
- [ ] Security audit completed
- [ ] Performance benchmarks met
- [ ] Database migrations tested
- [ ] Backup strategy in place
- [ ] Monitoring configured
- [ ] SSL certificates ready
- [ ] Environment variables configured

### 8.2 Deployment Steps
- [ ] Deploy database (run migrations)
- [ ] Deploy backend API
- [ ] Deploy frontend (PWA)
- [ ] Configure CDN (if applicable)
- [ ] Configure reverse proxy/load balancer
- [ ] Set up monitoring and alerting
- [ ] Perform smoke tests
- [ ] Update DNS records

### 8.3 Post-Deployment
- [ ] Verify all endpoints
- [ ] Test authentication flow
- [ ] Test offline functionality
- [ ] Verify reporting accuracy
- [ ] Conduct user acceptance testing
- [ ] Training sessions completed
- [ ] Documentation available
- [ ] Support channels active

---

## 9. Success Metrics

### 9.1 Performance Metrics
- API response time < 200ms (95th percentile)
- Page load time < 2 seconds
- Attendance entry < 5 seconds per bus
- Offline sync success rate > 99%

### 9.2 Business Metrics
- 100% digital attendance capture
- Zero data loss incidents
- 90% user satisfaction
- 50% reduction in administrative time
- 95% attendance accuracy

### 9.3 Technical Metrics
- 90%+ test coverage
- Zero critical security vulnerabilities
- 99.5% uptime
- < 5% sync failures

---

## 10. Maintenance & Support

### 10.1 Regular Maintenance
- **Daily**: Monitor sync status, check error logs
- **Weekly**: Review attendance accuracy, user feedback
- **Monthly**: Performance optimization, security updates
- **Quarterly**: Feature enhancements, user training refresh

### 10.2 Support Tiers
- **Tier 1**: Basic user questions (help desk)
- **Tier 2**: Technical issues (IT support)
- **Tier 3**: System bugs, critical issues (development team)

---

**Document Version**: 1.0  
**Last Updated**: January 2026  
**Author**: Technical Architecture Team  
**Status**: Ready for Implementation