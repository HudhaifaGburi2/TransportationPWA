# 🔐 Authentication, Authorization & Central Database Integration Guide

## Transportation Attendance System - Security Architecture

---

## Table of Contents

1. [Overview](#overview)
2. [Architecture Diagram](#architecture-diagram)
3. [Two-Database Strategy](#two-database-strategy)
4. [Authentication Flow](#authentication-flow)
5. [JWT Token Structure](#jwt-token-structure)
6. [Central Database Schemas](#central-database-schemas)
7. [Role Management](#role-management)
8. [Authorization Policies](#authorization-policies)
9. [API Endpoints](#api-endpoints)
10. [Frontend Integration](#frontend-integration)
11. [Security Best Practices](#security-best-practices)

---

## Overview

The Transportation Attendance System implements a **dual-database architecture** with JWT-based authentication:

| Component | Purpose |
|-----------|---------|
| **TransportationDB** | Read/Write - Stores attendance, routes, registrations, QR codes |
| **CentralStudentDB** | Read-Only - Source of truth for users, students, roles |
| **JWT Tokens** | Stateless authentication with role-based claims |
| **Policy-Based Authorization** | Fine-grained access control per feature |

---

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              CLIENT LAYER                                    │
├─────────────────────────────────────────────────────────────────────────────┤
│  ┌─────────────┐    ┌─────────────┐    ┌─────────────┐                     │
│  │  Student    │    │   Staff     │    │   Admin     │                     │
│  │    PWA      │    │    PWA      │    │   Portal    │                     │
│  └──────┬──────┘    └──────┬──────┘    └──────┬──────┘                     │
│         │                  │                  │                             │
│         └──────────────────┼──────────────────┘                             │
│                            │                                                │
│                    ┌───────▼───────┐                                        │
│                    │  JWT Token    │                                        │
│                    │  (Bearer)     │                                        │
│                    └───────┬───────┘                                        │
└────────────────────────────┼────────────────────────────────────────────────┘
                             │
┌────────────────────────────▼────────────────────────────────────────────────┐
│                              API LAYER                                       │
├─────────────────────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                     AuthController (/api/v1/auth)                    │   │
│  │  • POST /login          - Authenticate & get JWT                     │   │
│  │  • GET  /me             - Get current user info                      │   │
│  │  • POST /refresh-token  - Refresh expired token                      │   │
│  │  • POST /logout         - Invalidate refresh token                   │   │
│  │  • POST /validate-token - Check if token is valid                    │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                             │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │              [Authorize] Protected Controllers                       │   │
│  │  • AttendanceController   [Policy: CanMarkAttendance]               │   │
│  │  • RegistrationController [Policy: CanSubmitRegistration]           │   │
│  │  • QRCodeController       [Policy: CanViewOwnQRCode]                │   │
│  │  • RouteController        [Policy: CanManageRoutes]                 │   │
│  │  • ReportController       [Policy: CanGenerateReports]              │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
└────────────────────────────┬────────────────────────────────────────────────┘
                             │
┌────────────────────────────▼────────────────────────────────────────────────┐
│                          SERVICE LAYER                                       │
├─────────────────────────────────────────────────────────────────────────────┤
│  ┌───────────────────┐    ┌───────────────────┐    ┌───────────────────┐   │
│  │  IIdentityService │    │ IJwtTokenService  │    │    IAuthService   │   │
│  │  (Central Auth)   │    │ (Token Gen/Val)   │    │  (Local Staff)    │   │
│  └─────────┬─────────┘    └─────────┬─────────┘    └─────────┬─────────┘   │
│            │                        │                        │              │
└────────────┼────────────────────────┼────────────────────────┼──────────────┘
             │                        │                        │
┌────────────▼────────────────────────▼────────────────────────▼──────────────┐
│                        INFRASTRUCTURE LAYER                                  │
├─────────────────────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────┐         ┌─────────────────────────┐           │
│  │   EnhancedIdentity      │         │     JwtTokenService     │           │
│  │      Service            │         │                         │           │
│  │  • MD5 Password Hash    │         │  • HMAC-SHA256 Signing  │           │
│  │  • Role Resolution      │         │  • Claims Generation    │           │
│  └───────────┬─────────────┘         └─────────────────────────┘           │
│              │                                                              │
└──────────────┼──────────────────────────────────────────────────────────────┘
               │
┌──────────────▼──────────────────────────────────────────────────────────────┐
│                          DATABASE LAYER                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ┌─────────────────────────────┐    ┌─────────────────────────────┐        │
│  │    CentralStudentDB         │    │    TransportationDB         │        │
│  │      (READ-ONLY)            │    │     (READ/WRITE)            │        │
│  │                             │    │                             │        │
│  │  Tables:                    │    │  Tables:                    │        │
│  │  • Regt_User               │    │  • Staff                    │        │
│  │  • Regt_User_Role          │    │  • Attendance               │        │
│  │  • Students                │    │  • Routes                   │        │
│  │  • Parents                 │    │  • TransportationRegistrations│       │
│  │  • SET_Area (Districts)    │    │  • QRCodes                  │        │
│  │                             │    │  • Districts                │        │
│  │  Views:                     │    │  • StudentPenalties         │        │
│  │  • vw_Student_Halaqa_*     │    │  • NotificationLogs         │        │
│  └─────────────────────────────┘    └─────────────────────────────┘        │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## Two-Database Strategy

### CentralStudentDB (Read-Only)

The Central Database is the **source of truth** for user identity and student information. It is managed by the institution's central IT department.

```csharp
// CentralStudentDbContext - Enforces read-only access
public class CentralStudentDbContext : DbContext
{
    public CentralStudentDbContext(DbContextOptions<CentralStudentDbContext> options)
        : base(options)
    {
        // Force no-tracking for read-only operations
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    // Override SaveChanges to prevent writes
    public override int SaveChanges()
    {
        throw new InvalidOperationException("CentralStudentDbContext is read-only.");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("CentralStudentDbContext is read-only.");
    }
}
```

**Available DbSets:**

| DbSet | Table/View | Purpose |
|-------|------------|---------|
| `RegtUsers` | `Regt_User` | User authentication data |
| `RegtUserRoles` | `Regt_User_Role` | Role definitions |
| `Students` | `Students` | Student master data |
| `Parents` | `Parents` | Parent/guardian contacts |
| `Areas` | `SET_Area` | District/area definitions |
| `StudentHalaqaTeacherInfos` | `vw_Student_Halaqa_Teacher_information_Transportation_Dep` | Student class info (Arabic) |
| `StudentHalaqaTeacherInfosEn` | `vw_Student_Halaqa_Teacher_information_Transportation_Dep_En` | Student class info (English) |

### TransportationDB (Read/Write)

The Transportation Database stores all application-specific data.

```csharp
// TransportationDbContext - Full read/write access
public class TransportationDbContext : DbContext
{
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<Bus> Buses => Set<Bus>();
    public DbSet<TransportationRegistration> TransportationRegistrations => Set<TransportationRegistration>();
    public DbSet<QRCodeData> QRCodes => Set<QRCodeData>();
    public DbSet<District> Districts => Set<District>();
    public DbSet<Staff> Staff => Set<Staff>();
    // ... more DbSets
}
```

---

## Authentication Flow

### Login Sequence

```
┌─────────┐          ┌─────────┐          ┌─────────────────┐          ┌─────────────┐
│ Client  │          │   API   │          │ IdentityService │          │  CentralDB  │
└────┬────┘          └────┬────┘          └────────┬────────┘          └──────┬──────┘
     │                    │                        │                          │
     │  POST /auth/login  │                        │                          │
     │  {username, pwd}   │                        │                          │
     │───────────────────>│                        │                          │
     │                    │                        │                          │
     │                    │  AuthenticateWithMD5   │                          │
     │                    │───────────────────────>│                          │
     │                    │                        │                          │
     │                    │                        │  SELECT FROM Regt_User   │
     │                    │                        │─────────────────────────>│
     │                    │                        │                          │
     │                    │                        │  User + Role_ID_Comma_Sep│
     │                    │                        │<─────────────────────────│
     │                    │                        │                          │
     │                    │                        │  SELECT FROM Regt_User_Role
     │                    │                        │─────────────────────────>│
     │                    │                        │                          │
     │                    │                        │  Role Names              │
     │                    │                        │<─────────────────────────│
     │                    │                        │                          │
     │                    │  (User, Roles)         │                          │
     │                    │<───────────────────────│                          │
     │                    │                        │                          │
     │                    │  Generate JWT Token    │                          │
     │                    │  (userId, roles)       │                          │
     │                    │                        │                          │
     │  {token, roles,    │                        │                          │
     │   userId, expiry}  │                        │                          │
     │<───────────────────│                        │                          │
     │                    │                        │                          │
```

### Password Verification

The Central DB uses **MD5 password hashing** (legacy system). The authentication process:

```csharp
public async Task<(IdentityResult Result, Regt_User? User, List<Regt_User_Role> Roles)> 
    AuthenticateWithMD5Async(string username, string password, CancellationToken ct)
{
    // 1. Find user by username or user_id
    var user = await _centralContext.RegtUsers
        .FirstOrDefaultAsync(u => u.Username == username || 
                                  u.User_Id.ToString() == username, ct);

    if (user?.UserPassword == null)
        return (IdentityResult.Failure("Invalid username or password."), null, new());

    // 2. Hash input password with MD5 and compare
    var encryptedInputPassword = MD5PasswordHasher.GetMD5Hash(password);
    if (!user.UserPassword.Equals(encryptedInputPassword))
        return (IdentityResult.Failure("Invalid username or password."), null, new());

    // 3. Resolve roles from comma-separated Role_IDs
    var roles = await ResolveRolesAsync(user.Role_ID_Comma_Sep, ct);

    return (IdentityResult.Success(), user, roles);
}
```

### Role Resolution

Users in the Central DB have roles stored as comma-separated IDs:

```sql
-- Regt_User table
User_Id: 12345
Username: "john.doe"
Role_ID_Comma_Sep: "1,5,12"  -- Multiple roles
```

The system resolves these IDs to role names:

```csharp
private async Task<List<Regt_User_Role>> ResolveRolesAsync(string? roleIdCommaSep, CancellationToken ct)
{
    if (string.IsNullOrWhiteSpace(roleIdCommaSep))
        return new List<Regt_User_Role>();

    // Parse "1,5,12" into [1, 5, 12]
    var roleIds = roleIdCommaSep
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(s => int.TryParse(s, out var id) ? id : (int?)null)
        .Where(id => id.HasValue)
        .Select(id => id!.Value)
        .ToList();

    // Fetch role names from Regt_User_Role table
    return await _centralContext.RegtUserRoles
        .Where(r => roleIds.Contains(r.Role_ID))
        .ToListAsync(ct);
}
```

---

## JWT Token Structure

### Token Generation

```csharp
public string GenerateToken(string userId, string username, IEnumerable<string> roles)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_secretKey);

    var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, userId),
        new(ClaimTypes.Name, username),
        new("userId", userId),
        new("username", username)
    };

    // Add role claims
    foreach (var role in roles)
    {
        claims.Add(new Claim(ClaimTypes.Role, role));
    }

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddHours(_expirationHours),
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature),
        Issuer = "TransportationAttendanceAPI",
        Audience = "TransportationAttendancePWA"
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}
```

### Token Configuration (appsettings.json)

```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "TransportationAttendanceAPI",
    "Audience": "TransportationAttendancePWA",
    "ExpirationHours": 24
  }
}
```

### Decoded JWT Payload Example

```json
{
  "sub": "12345",
  "name": "john.doe",
  "userId": "12345",
  "username": "john.doe",
  "role": ["Admin", "Supervisor", "SYSTEM_ADMINISTRATOR"],
  "jti": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "exp": 1704672000,
  "iss": "TransportationAttendanceAPI",
  "aud": "TransportationAttendancePWA"
}
```

---

## Central Database Schemas

### Regt_User (User Authentication)

```sql
CREATE TABLE [dbo].[Regt_User] (
    [User_Id]              INT            PRIMARY KEY,
    [Username]             NVARCHAR(50)   NOT NULL,
    [UserPassword]         NVARCHAR(50)   NOT NULL,  -- MD5 hashed
    [Role_ID_Comma_Sep]    NVARCHAR(250)  NULL,      -- "1,5,12"
    [Full_Name_Official_EN] NVARCHAR(500) NULL,
    [Email_id]             NVARCHAR(200)  NULL,
    [Mobile_no]            NVARCHAR(50)   NULL,
    [Is_Active]            BIT            DEFAULT 1
);
```

### Regt_User_Role (Role Definitions)

```sql
CREATE TABLE [dbo].[Regt_User_Role] (
    [Role_ID]    INT           PRIMARY KEY,
    [Role_Name]  NVARCHAR(50)  NOT NULL,
    [Role_Desc]  NVARCHAR(250) NULL
);

-- Sample Data
INSERT INTO Regt_User_Role VALUES 
(1, 'Admin', 'Full administrative access'),
(2, 'Supervisor', 'Supervisory access'),
(3, 'Staff', 'Basic staff access'),
(4, 'Student', 'Student access'),
(5, 'SuperAdmin', 'Super administrator'),
(6, 'SYSTEM_ADMINISTRATOR', 'External system admin');
```

### Students (Student Master Data)

```sql
CREATE TABLE [dbo].[Students] (
    [Id]               UNIQUEIDENTIFIER PRIMARY KEY,
    [FullName]         NVARCHAR(200)    NOT NULL,
    [FirstName]        NVARCHAR(100)    NOT NULL,
    [LastName]         NVARCHAR(100)    NOT NULL,
    [Grade]            NVARCHAR(20)     NOT NULL,
    [Section]          NVARCHAR(20)     NULL,
    [EnrollmentNumber] NVARCHAR(50)     NOT NULL,
    [PhotoUrl]         NVARCHAR(500)    NULL,
    [AcademicYear]     NVARCHAR(20)     NULL,
    [IsActive]         BIT              DEFAULT 1
);
```

### SET_Area (Districts/Areas)

```sql
CREATE TABLE [dbo].[SET_Area] (
    [Area_ID]          INT           PRIMARY KEY,
    [Area_Name]        NVARCHAR(200) NOT NULL,
    [Area_Description] NVARCHAR(500) NULL,
    [Is_Active]        BIT           DEFAULT 1
);
```

---

## Role Management

### Defined Roles

```csharp
// TransportationAttendance.Domain.Authorization.Roles
public static class Roles
{
    // Individual Roles
    public const string Staff = "Staff";
    public const string Supervisor = "Supervisor";
    public const string Admin = "Admin";
    public const string SuperAdmin = "SuperAdmin";
    public const string SystemAdministrator = "SYSTEM_ADMINISTRATOR";
    public const string Student = "Student";

    // Role Groups
    public static readonly string[] AdminRoles = 
        { Admin, SuperAdmin, SystemAdministrator };
    
    public static readonly string[] StaffRoles = 
        { Staff, Supervisor, Admin, SuperAdmin, SystemAdministrator };
    
    public static readonly string[] SupervisorAndAbove = 
        { Supervisor, Admin, SuperAdmin, SystemAdministrator };
    
    public static readonly string[] SuperAdminOnly = 
        { SuperAdmin, SystemAdministrator };
}
```

### Role Hierarchy

```
┌─────────────────────────────────────────────────────────────────┐
│                        ROLE HIERARCHY                           │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌─────────────────┐                                           │
│  │   SuperAdmin    │ ◄─── Full system access                   │
│  │ SYSTEM_ADMIN    │      Settings, Audit logs, Role mgmt     │
│  └────────┬────────┘                                           │
│           │                                                     │
│           ▼                                                     │
│  ┌─────────────────┐                                           │
│  │     Admin       │ ◄─── Management access                    │
│  │                 │      Routes, Buses, Registrations         │
│  └────────┬────────┘                                           │
│           │                                                     │
│           ▼                                                     │
│  ┌─────────────────┐                                           │
│  │   Supervisor    │ ◄─── Supervisory access                   │
│  │                 │      Reports, Penalties, Bus assignment   │
│  └────────┬────────┘                                           │
│           │                                                     │
│           ▼                                                     │
│  ┌─────────────────┐                                           │
│  │     Staff       │ ◄─── Basic operations                     │
│  │                 │      Mark attendance, Scan QR, View       │
│  └─────────────────┘                                           │
│                                                                 │
│  ┌─────────────────┐                                           │
│  │    Student      │ ◄─── Self-service only                    │
│  │                 │      View own QR, Submit registration     │
│  └─────────────────┘                                           │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## Authorization Policies

### Policy Definitions

```csharp
// TransportationAttendance.API.Extensions.AuthorizationExtensions
public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
{
    services.AddAuthorization(options =>
    {
        // ATTENDANCE POLICIES
        options.AddPolicy(Policies.CanMarkAttendance, policy =>
            policy.RequireRole(Roles.StaffRoles));

        options.AddPolicy(Policies.CanViewAttendance, policy =>
            policy.RequireRole(Roles.StaffRoles));

        options.AddPolicy(Policies.CanEditAttendance, policy =>
            policy.RequireRole(Roles.SupervisorAndAbove));

        // ROUTE MANAGEMENT POLICIES
        options.AddPolicy(Policies.CanViewRoutes, policy =>
            policy.RequireAuthenticatedUser());

        options.AddPolicy(Policies.CanManageRoutes, policy =>
            policy.RequireRole(Roles.AdminRoles));

        // REGISTRATION POLICIES
        options.AddPolicy(Policies.CanSubmitRegistration, policy =>
            policy.RequireRole(Roles.Student));

        options.AddPolicy(Policies.CanApproveRegistrations, policy =>
            policy.RequireRole(Roles.AdminRoles));

        // QR CODE POLICIES
        options.AddPolicy(Policies.CanViewOwnQRCode, policy =>
            policy.RequireRole(Roles.Student));

        options.AddPolicy(Policies.CanGenerateQRCodes, policy =>
            policy.RequireRole(Roles.AdminRoles));

        options.AddPolicy(Policies.CanScanQRCodes, policy =>
            policy.RequireRole(Roles.StaffRoles));

        // SYSTEM ADMIN POLICIES
        options.AddPolicy(Policies.CanAccessSettings, policy =>
            policy.RequireRole(Roles.SuperAdminOnly));

        options.AddPolicy(Policies.CanViewAuditLogs, policy =>
            policy.RequireRole(Roles.SuperAdminOnly));
    });

    return services;
}
```

### Complete Policy Reference

| Policy | Required Roles | Description |
|--------|----------------|-------------|
| **Attendance** | | |
| `CanMarkAttendance` | Staff, Supervisor, Admin, SuperAdmin | Mark student attendance |
| `CanViewAttendance` | Staff, Supervisor, Admin, SuperAdmin | View attendance records |
| `CanEditAttendance` | Supervisor, Admin, SuperAdmin | Modify attendance |
| **Routes** | | |
| `CanViewRoutes` | Any authenticated | View route information |
| `CanManageRoutes` | Admin, SuperAdmin | CRUD routes |
| `CanAssignStudents` | Admin, SuperAdmin | Assign students to routes |
| **Buses & Drivers** | | |
| `CanViewBuses` | Staff+ | View bus information |
| `CanManageBuses` | Admin, SuperAdmin | CRUD buses |
| `CanManageDrivers` | Admin, SuperAdmin | CRUD drivers |
| `CanAssignBuses` | Supervisor+ | Assign buses to routes |
| **Registration** | | |
| `CanSubmitRegistration` | Student | Submit transport request |
| `CanViewOwnRegistration` | Student | View own registration |
| `CanViewRegistrations` | Admin, SuperAdmin | View all registrations |
| `CanApproveRegistrations` | Admin, SuperAdmin | Approve/reject requests |
| **Penalties** | | |
| `CanViewPenalties` | Staff+ | View penalty records |
| `CanIssuePenalties` | Supervisor+ | Issue penalties |
| `CanResolvePenalties` | Admin, SuperAdmin | Resolve/waive penalties |
| **QR Codes** | | |
| `CanViewOwnQRCode` | Student | View own QR code |
| `CanGenerateQRCodes` | Admin, SuperAdmin | Generate QR codes |
| `CanScanQRCodes` | Staff+ | Scan QR codes |
| **Reports** | | |
| `CanViewDashboard` | Supervisor+ | Access dashboard |
| `CanGenerateReports` | Supervisor+ | Generate reports |
| **System Admin** | | |
| `CanViewStaff` | Supervisor+ | View staff members |
| `CanManageStaff` | Admin, SuperAdmin | Manage staff |
| `CanAssignRoles` | SuperAdmin only | Assign roles |
| `CanAccessSettings` | SuperAdmin only | System settings |
| `CanViewAuditLogs` | SuperAdmin only | View audit logs |
| `CanSyncData` | Admin, SuperAdmin | Data synchronization |

---

## API Endpoints

### Authentication Endpoints

```http
### Login
POST /api/v1/auth/login
Content-Type: application/json
X-Login-Gateway-Token: {optional-gateway-token}

{
  "username": "john.doe",
  "password": "secretPassword123"
}

### Response
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "userId": "12345",
    "username": "john.doe",
    "fullName": "John Doe",
    "roles": ["Admin", "Supervisor"],
    "expiresAt": "2026-01-13T08:00:00Z"
  },
  "message": "Login successful."
}
```

```http
### Get Current User
GET /api/v1/auth/me
Authorization: Bearer {jwt-token}

### Response
{
  "success": true,
  "data": {
    "userId": "12345",
    "username": "john.doe",
    "roles": ["Admin", "Supervisor"]
  }
}
```

```http
### Refresh Token
POST /api/v1/auth/refresh-token
Content-Type: application/json

{
  "accessToken": "expired-jwt-token",
  "refreshToken": "refresh-token-string"
}
```

```http
### Logout
POST /api/v1/auth/logout
Authorization: Bearer {jwt-token}
```

```http
### Validate Token
POST /api/v1/auth/validate-token
Authorization: Bearer {jwt-token}
```

---

## Frontend Integration

### Vue.js Auth Store (Pinia)

```javascript
// stores/auth.js
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '@/services/api'

export const useAuthStore = defineStore('auth', () => {
  const user = ref(null)
  const token = ref(localStorage.getItem('token'))
  const refreshToken = ref(localStorage.getItem('refreshToken'))

  // Computed properties for role checks
  const isAuthenticated = computed(() => !!token.value)
  
  const isStudent = computed(() => 
    user.value?.roles?.includes('Student'))
  
  const isStaff = computed(() => 
    user.value?.roles?.some(r => 
      ['Staff', 'Supervisor', 'Admin', 'SuperAdmin', 'SYSTEM_ADMINISTRATOR'].includes(r)))
  
  const isAdmin = computed(() => 
    user.value?.roles?.some(r => 
      ['Admin', 'SuperAdmin', 'SYSTEM_ADMINISTRATOR'].includes(r)))
  
  const isSuperAdmin = computed(() => 
    user.value?.roles?.some(r => 
      ['SuperAdmin', 'SYSTEM_ADMINISTRATOR'].includes(r)))

  // Actions
  async function login(username, password) {
    const response = await api.post('/auth/login', { username, password })
    
    token.value = response.data.data.token
    user.value = {
      id: response.data.data.userId,
      username: response.data.data.username,
      fullName: response.data.data.fullName,
      roles: response.data.data.roles
    }
    
    localStorage.setItem('token', token.value)
    api.defaults.headers.common['Authorization'] = `Bearer ${token.value}`
    
    return response.data
  }

  async function logout() {
    try {
      await api.post('/auth/logout')
    } finally {
      token.value = null
      refreshToken.value = null
      user.value = null
      localStorage.removeItem('token')
      localStorage.removeItem('refreshToken')
      delete api.defaults.headers.common['Authorization']
    }
  }

  // Check specific policy
  function hasPolicy(policy) {
    const policyRoleMap = {
      'CanMarkAttendance': ['Staff', 'Supervisor', 'Admin', 'SuperAdmin', 'SYSTEM_ADMINISTRATOR'],
      'CanViewOwnQRCode': ['Student'],
      'CanGenerateQRCodes': ['Admin', 'SuperAdmin', 'SYSTEM_ADMINISTRATOR'],
      'CanApproveRegistrations': ['Admin', 'SuperAdmin', 'SYSTEM_ADMINISTRATOR'],
      // ... more policies
    }
    
    const requiredRoles = policyRoleMap[policy] || []
    return user.value?.roles?.some(r => requiredRoles.includes(r)) ?? false
  }

  return {
    user, token, refreshToken,
    isAuthenticated, isStudent, isStaff, isAdmin, isSuperAdmin,
    login, logout, hasPolicy
  }
})
```

### Route Guards

```javascript
// router/index.js
import { useAuthStore } from '@/stores/auth'

const routes = [
  {
    path: '/admin/registrations',
    component: () => import('@/views/admin/RegistrationsView.vue'),
    meta: { 
      requiresAuth: true,
      policy: 'CanViewRegistrations'
    }
  },
  {
    path: '/student/qr-code',
    component: () => import('@/views/student/QRCodeView.vue'),
    meta: { 
      requiresAuth: true,
      policy: 'CanViewOwnQRCode'
    }
  }
]

router.beforeEach((to, from, next) => {
  const auth = useAuthStore()
  
  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    return next('/login')
  }
  
  if (to.meta.policy && !auth.hasPolicy(to.meta.policy)) {
    return next('/unauthorized')
  }
  
  next()
})
```

---

## Security Best Practices

### 1. Token Storage

```javascript
// ❌ Don't store sensitive tokens in localStorage for production
localStorage.setItem('token', token)

// ✅ Use httpOnly cookies for refresh tokens (server-side)
// Access tokens can be in memory only
```

### 2. Token Expiration

| Token Type | Expiration | Storage |
|------------|------------|---------|
| Access Token | 8-24 hours | Memory / localStorage |
| Refresh Token | 7 days | httpOnly cookie (preferred) |

### 3. API Security Headers

```csharp
// Program.cs
app.UseHsts();
app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    await next();
});
```

### 4. Rate Limiting

```csharp
// Recommended rate limits
services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("auth", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 10; // 10 login attempts per minute
    });
});
```

### 5. Password Security

- Central DB uses MD5 (legacy) - **do not change**
- Local Staff table uses BCrypt - **recommended**
- Minimum password length: 8 characters
- Never log passwords or tokens

### 6. Audit Logging

```csharp
// Log all authentication events
_logger.LogInformation("Login successful for {Username}", username);
_logger.LogWarning("Login failed: Invalid password for {Username}", username);
_logger.LogWarning("Unauthorized access attempt to {Resource} by {UserId}", resource, userId);
```

---

## Troubleshooting

### Common Issues

| Issue | Cause | Solution |
|-------|-------|----------|
| "Invalid token" | Token expired | Use refresh token or re-login |
| "Access denied" | Missing role | Check user's roles in Central DB |
| "User not found" | Wrong database | Verify CentralDB connection |
| "401 Unauthorized" | Missing/invalid Bearer | Check Authorization header format |
| "403 Forbidden" | Policy not satisfied | User lacks required role for endpoint |

### Debug Token Claims

```csharp
// In controller, inspect claims
var userId = User.FindFirst("userId")?.Value;
var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value);
_logger.LogDebug("User {UserId} has roles: {Roles}", userId, string.Join(", ", roles));
```

---

## References

- [JWT.io](https://jwt.io/) - JWT Debugger
- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)
- [Policy-Based Authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies)

---

*Last Updated: January 2026*
*Version: 1.0*
