# Phase 1: Foundation & Core Setup

## Weeks 1-3 | Infrastructure, Authentication & Master Data

---

## Phase Overview

| Attribute | Details |
|-----------|---------|
| **Duration** | 3 Weeks |
| **Start Date** | _TBD_ |
| **End Date** | _TBD_ |
| **Status** | 🔴 Not Started |
| **Progress** | 0% |

---

## Objectives

1. Establish robust development infrastructure for both frontend and backend
2. Implement secure JWT-based authentication and role-based authorization
3. Create master data management for Districts, Locations, and Periods
4. Set up offline-first database infrastructure (IndexedDB)
5. Configure PWA capabilities with Workbox

---

## Sprint 1.1: Infrastructure Setup (Week 1)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Set up .NET 8 solution structure (Domain, Application, Infrastructure, API) | ⬜ Pending | | Clean Architecture layers |
| Configure EF Core with SQL Server | ⬜ Pending | | Connection strings, DbContext |
| Implement base entities (IEntity, IAuditable, ISoftDelete) | ⬜ Pending | | Shared interfaces |
| Set up Repository pattern and Unit of Work | ⬜ Pending | | Generic repository base |
| Configure AutoMapper | ⬜ Pending | | DTO mapping profiles |
| Set up FluentValidation | ⬜ Pending | | Request validation |
| Configure Serilog for logging | ⬜ Pending | | File + Console sinks |
| Set up Swagger/OpenAPI | ⬜ Pending | | API documentation |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Initialize Vue 3 + Vite project | ⬜ Pending | | TypeScript enabled |
| Configure Tailwind CSS + DaisyUI | ⬜ Pending | | Theme customization |
| Set up Vue Router | ⬜ Pending | | Route structure |
| Configure Pinia stores | ⬜ Pending | | State management |
| Set up Axios with interceptors | ⬜ Pending | | API client, error handling |
| Configure PWA with Workbox | ⬜ Pending | | Service worker, caching |
| Set up Dexie.js for IndexedDB | ⬜ Pending | | Offline database schema |

### Database Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create initial database schema | ⬜ Pending | | Core tables |
| Create migration scripts | ⬜ Pending | | EF Core migrations |
| Seed master data (Periods, Locations) | ⬜ Pending | | Initial data |

### Deliverables Checklist

- [ ] Working development environment (frontend + backend)
- [ ] Database with initial schema created
- [ ] Basic project structure established
- [ ] API documentation accessible via Swagger
- [ ] PWA manifest configured

---

## Sprint 1.2: Authentication & Authorization (Week 2)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Implement JWT token generation and validation | ⬜ Pending | | JwtTokenGenerator service |
| Create User entity and repository | ⬜ Pending | | User model with roles |
| Implement authentication service | ⬜ Pending | | Login, token refresh |
| Create AuthController (Login, Refresh, Logout) | ⬜ Pending | | Auth endpoints |
| Implement role-based authorization middleware | ⬜ Pending | | [Authorize(Roles="")] |
| Add password hashing service | ⬜ Pending | | BCrypt or Identity |
| Create audit logging infrastructure | ⬜ Pending | | AuditLog entity |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create login page | ⬜ Pending | | Mobile-responsive design |
| Implement auth store (Pinia) | ⬜ Pending | | Token storage, user state |
| Create auth service (API calls) | ⬜ Pending | | Login, logout, refresh |
| Set up JWT token management | ⬜ Pending | | Storage, auto-refresh |
| Implement route guards | ⬜ Pending | | Protected routes |
| Create role-based UI navigation | ⬜ Pending | | Dynamic menu based on role |

### Deliverables Checklist

- [ ] Working login/logout flow
- [ ] JWT tokens generated and validated
- [ ] Role-based access control functional
- [ ] Secure API endpoints (401/403 responses)
- [ ] Token refresh mechanism working
- [ ] Audit logging for auth events

---

## Sprint 1.3: Master Data Management (Week 3)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Implement Districts CRUD (controller, service, repository) | ⬜ Pending | | Full CRUD operations |
| Implement Periods management (read-only) | ⬜ Pending | | Fixed 5 periods |
| Implement Locations management | ⬜ Pending | | Parking areas A8-B11 |
| Create lookups endpoints | ⬜ Pending | | /api/lookups/* |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create admin dashboard layout | ⬜ Pending | | Sidebar, header, content |
| Build Districts management UI | ⬜ Pending | | List, add, edit, delete |
| Build Locations management UI | ⬜ Pending | | Parking area management |
| Create reusable components | ⬜ Pending | | DataTable, Pagination, SearchBar |

### Database Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Seed Districts data from requirements | ⬜ Pending | | 11 districts |
| Create indexes for performance | ⬜ Pending | | Query optimization |

### Deliverables Checklist

- [ ] Districts CRUD fully functional
- [ ] Locations management working
- [ ] Periods displayed correctly
- [ ] Admin interface accessible
- [ ] Reusable UI components library
- [ ] Data seeded and verified

---

## Technical Specifications

### Backend Project Structure

```
TUMS.Solution/
├── src/
│   ├── TUMS.Domain/
│   │   ├── Entities/
│   │   │   ├── BaseEntity.cs
│   │   │   ├── User.cs
│   │   │   ├── District.cs
│   │   │   ├── Location.cs
│   │   │   └── Period.cs
│   │   ├── Interfaces/
│   │   │   ├── IEntity.cs
│   │   │   ├── IAuditable.cs
│   │   │   └── ISoftDelete.cs
│   │   └── Enums/
│   │       └── UserRole.cs
│   │
│   ├── TUMS.Application/
│   │   ├── Interfaces/
│   │   │   ├── Repositories/
│   │   │   └── Services/
│   │   ├── DTOs/
│   │   └── Validators/
│   │
│   ├── TUMS.Infrastructure/
│   │   ├── Persistence/
│   │   │   ├── TUMSDbContext.cs
│   │   │   └── Repositories/
│   │   └── Identity/
│   │       └── JwtTokenGenerator.cs
│   │
│   └── TUMS.API/
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── DistrictsController.cs
│       │   └── LookupsController.cs
│       └── Middleware/
```

### Frontend Project Structure

```
tums-frontend/
├── src/
│   ├── router/
│   │   ├── index.ts
│   │   └── guards/
│   │       └── auth.guard.ts
│   ├── stores/
│   │   ├── auth.store.ts
│   │   └── ui.store.ts
│   ├── services/
│   │   ├── api/
│   │   │   ├── axios.config.ts
│   │   │   └── auth.service.ts
│   │   └── offline/
│   │       └── db.ts
│   ├── views/
│   │   ├── auth/
│   │   │   └── LoginView.vue
│   │   └── dashboard/
│   │       └── AdminDashboardView.vue
│   └── components/
│       └── common/
```

### Database Schema (Phase 1)

```sql
-- Users Table
CREATE TABLE Users (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    FullName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200),
    Role NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastLoginAt DATETIME2
);

-- Periods Table
CREATE TABLE Periods (
    PeriodId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PeriodCode NVARCHAR(20) NOT NULL UNIQUE,
    PeriodNameAr NVARCHAR(100) NOT NULL,
    PeriodNameEn NVARCHAR(100),
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    DisplayOrder INT NOT NULL
);

-- Districts Table
CREATE TABLE Districts (
    DistrictId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    DistrictNameAr NVARCHAR(200) NOT NULL,
    DistrictNameEn NVARCHAR(200),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Locations Table
CREATE TABLE Locations (
    LocationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    LocationCode NVARCHAR(20) NOT NULL UNIQUE,
    LocationName NVARCHAR(100) NOT NULL,
    LocationType NVARCHAR(50),
    IsActive BIT NOT NULL DEFAULT 1
);

-- Audit Logs Table
CREATE TABLE AuditLogs (
    AuditId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER,
    Action NVARCHAR(100) NOT NULL,
    EntityType NVARCHAR(100),
    EntityId UNIQUEIDENTIFIER,
    OldValues NVARCHAR(MAX),
    NewValues NVARCHAR(MAX),
    IpAddress NVARCHAR(50),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
```

---

## API Endpoints (Phase 1)

### Authentication

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/auth/login` | User login | No |
| POST | `/api/auth/refresh` | Refresh token | Yes |
| POST | `/api/auth/logout` | Logout user | Yes |
| GET | `/api/auth/me` | Get current user | Yes |

### Districts

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/districts` | List all districts | Yes |
| GET | `/api/districts/{id}` | Get district by ID | Yes |
| POST | `/api/districts` | Create district | Admin |
| PUT | `/api/districts/{id}` | Update district | Admin |
| DELETE | `/api/districts/{id}` | Delete district | Admin |

### Lookups

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/lookups/periods` | List all periods | Yes |
| GET | `/api/lookups/locations` | List all locations | Yes |
| GET | `/api/lookups/districts` | List active districts | Yes |

---

## Acceptance Criteria

### Sprint 1.1
- [ ] Backend solution compiles and runs without errors
- [ ] Frontend dev server starts successfully
- [ ] Database connection established
- [ ] Swagger UI accessible at `/swagger`
- [ ] PWA installable on mobile device

### Sprint 1.2
- [ ] User can login with valid credentials
- [ ] Invalid credentials return 401
- [ ] JWT token expires after configured time
- [ ] Token refresh extends session
- [ ] Role-based routes enforce access control

### Sprint 1.3
- [ ] Districts can be created, read, updated, deleted
- [ ] Locations display correctly
- [ ] Periods show in correct order
- [ ] Admin dashboard displays master data
- [ ] Search and pagination work on data tables

---

## Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Central DB connection issues | High | Create mock data layer for development |
| JWT security vulnerabilities | High | Follow OWASP guidelines, security audit |
| PWA caching conflicts | Medium | Clear versioning strategy for service worker |
| UI/UX inconsistencies | Low | Design system with DaisyUI components |

---

## Achievement Tracking

### Week 1 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 8 | 0 | 0% |
| Frontend Tasks | 7 | 0 | 0% |
| Database Tasks | 3 | 0 | 0% |
| **Total** | **18** | **0** | **0%** |

### Week 2 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 7 | 0 | 0% |
| Frontend Tasks | 6 | 0 | 0% |
| **Total** | **13** | **0** | **0%** |

### Week 3 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 4 | 0 | 0% |
| Frontend Tasks | 4 | 0 | 0% |
| Database Tasks | 2 | 0 | 0% |
| **Total** | **10** | **0** | **0%** |

---

## Phase Summary

### Overall Progress

```
Phase 1 Completion: ░░░░░░░░░░ 0%
```

| Sprint | Status | Completion |
|--------|--------|------------|
| Sprint 1.1 - Infrastructure | 🔴 Not Started | 0% |
| Sprint 1.2 - Authentication | 🔴 Not Started | 0% |
| Sprint 1.3 - Master Data | 🔴 Not Started | 0% |

### Key Milestones

- [ ] 🎯 Development environment ready
- [ ] 🎯 Authentication system functional
- [ ] 🎯 Admin dashboard accessible
- [ ] 🎯 Master data management complete

---

## Notes & Decisions

_Document important decisions, blockers, and notes during this phase._

### Decision Log

| Date | Decision | Rationale |
|------|----------|-----------|
| | | |

### Blockers

| Issue | Status | Resolution |
|-------|--------|------------|
| | | |

---

**Phase Owner:** _TBD_  
**Last Updated:** _TBD_  
**Next Review:** _TBD_
