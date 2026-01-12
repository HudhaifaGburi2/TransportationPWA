---
trigger: always_on
---

---

trigger: always_on
---

# 🚌 **Transportation Attendance System - GLOBAL ENGINEERING RULES (STRICT / NON-NEGOTIABLE)**

## ⚙️ **CORE PRINCIPLES**

1. The solution **must compile cleanly**: zero errors, zero warnings, zero ambiguity.
2. The architecture **must follow Clean Architecture, SOLID, Repository Pattern, and Unit of Work** at all times.
3. **Business logic exists only in Application Services.**
4. **Persistence logic exists only in Repositories.**
5. **EF Core must never appear outside Infrastructure.**
6. **Async is mandatory end-to-end**, including `CancellationToken`.
7. **Dependency Injection is mandatory** — manual instantiation is forbidden.
8. **BDD testing with Gherkin is mandatory** for all features.
9. **Offline-first architecture is mandatory** — all operations must queue and sync.
10. **Two-database strategy is mandatory** — TransportationDB (read/write) + CentralStudentDB (read-only).

---

## 🧩 **ARCHITECTURE RULES**

1. Layer order is immutable:

    ```
    Controller → Application Service → Repository → DbContext
    PWA → API → Service → Repository → Database
    ```

2. Layers may only depend **inward**.
3. No layer may be skipped, bypassed, or merged.
4. Controllers depend only on **service interfaces** (`IAttendanceService`, `IQRCodeService`).
5. Services depend only on **IUnitOfWork and repository interfaces**.
6. Repositories depend only on **DbContext** (never call other repositories).
7. **DTOs are the only allowed cross-layer data contract**.
8. No static helpers, no shared mutable state, no global shortcuts.
9. Each layer has exactly one responsibility — no bleeding of concerns.
10. PWA must never directly access databases — only through API endpoints.

---

## 📁 **PROJECT STRUCTURE RULES**

1. Backend project structure must follow:

    ```
    TransportationAttendance.API/              → Controllers, Middleware, Filters
    TransportationAttendance.Application/      → Services, DTOs, Validators, Mapping
    TransportationAttendance.Domain/           → Entities, Enums, Interfaces, ValueObjects
    TransportationAttendance.Infrastructure/   → Repositories, DbContexts, UnitOfWork, External Services
    TransportationAttendance.Shared/           → Exceptions, Extensions, Utilities
    TransportationAttendance.Tests/            → Unit, Integration, BDD tests
    ```

2. Frontend project structure must follow:

    ```
    ClientApp/
    ├── src/
    │   ├── components/        → Reusable UI components
    │   ├── composables/       → Composition API logic
    │   ├── stores/            → Pinia state management
    │   ├── services/          → API clients, business logic
    │   ├── views/             → Page components
    │   ├── router/            → Vue Router configuration
    │   └── utils/             → Helper functions
    ├── public/                → Static assets, PWA manifest
    └── tests/                 → Vue component tests
    ```

3. Folders and namespaces must match exactly.
4. No mixing of concerns within folders.
5. All interfaces belong in Domain layer.
6. All implementations belong in Infrastructure or Application.
7. PWA service workers must be separate from application code.

---

## 🧱 **CONTROLLER RULES (API LAYER)**

1. Controllers are **orchestration only**.
2. Controllers may:
    * Validate request models (via filters)
    * Call application services
    * Return HTTP responses
    * Handle authentication/authorization
3. Controllers must never:
    * Contain business logic
    * Call repositories directly
    * Use DbContext
    * Start transactions
    * Perform data mapping
    * Expose domain entities
    * Access external services directly
4. Controllers must be versioned (`/api/v1/...`).
5. Controllers rely on **global exception middleware only** — no try-catch blocks.
6. All endpoints must accept `CancellationToken`.
7. All responses must return DTOs, never entities.
8. HTTP status codes must match REST conventions:
    * 200 OK → Success
    * 201 Created → Resource created
    * 204 No Content → Success with no body
    * 400 Bad Request → Validation failure
    * 401 Unauthorized → Authentication required
    * 403 Forbidden → Insufficient permissions
    * 404 Not Found → Resource not found
    * 409 Conflict → Duplicate or concurrency issue
    * 500 Internal Server Error → Unhandled exception
9. All attendance endpoints must support **offline sync** batch operations.
10. Controllers must validate authorization before calling services.

---

## 🧠 **APPLICATION SERVICE RULES**

1. Services enforce **business invariants and workflows**.
2. Services orchestrate repositories **only via `IUnitOfWork`**.
3. **All transaction control lives in services**.
4. Services must never:
    * Reference DbContext directly
    * Use EF Core directly
    * Write SQL queries
    * Expose domain entities to controllers
    * Call other services directly (use mediator pattern if needed)
    * Access HttpContext
5. Each service method:
    * Has a single, explicit responsibility
    * Returns DTOs or `Result<T>`
    * Accepts DTOs or primitives
    * Validates all business rules
6. Services must be deterministic and side-effect controlled.
7. Services must validate all inputs (use FluentValidation).
8. Services must handle all business exceptions.
9. Large services (>500 lines) must be refactored immediately.
10. All service methods must be async with `CancellationToken`.
11. **Attendance services must handle duplicate detection** (same student, same checkpoint, same day).
12. **QR code services must validate expiration and uniqueness**.
13. **Notification services must queue failed notifications for retry**.
14. **Sync services must handle conflict resolution** (offline vs online data).

---

## 🔄 **UNIT OF WORK RULES**

1. **All write operations must use Unit of Work**.
2. Multi-repository operations must be transactional.
3. Transactions must:
    * Begin in services via `BeginTransactionAsync()`
    * Commit on success via `CommitAsync()`
    * Rollback on failure via `RollbackAsync()`
4. Nested transactions are **forbidden**.
5. `DbContext.SaveChanges()` must only be invoked by `IUnitOfWork.SaveChangesAsync()`.
6. Unit of Work must expose all repositories as properties:
    * `IAttendanceRepository Attendances { get; }`
    * `IStudentRouteAssignmentRepository StudentRoutes { get; }`
    * `IQRCodeDataRepository QRCodes { get; }`
    * `INotificationLogRepository Notifications { get; }`
7. Unit of Work must implement `IDisposable` and `IAsyncDisposable`.
8. Transaction scope must be minimal — commit as soon as possible.
9. Each service method requiring transaction must follow:

    ```csharp
    await _unitOfWork.BeginTransactionAsync(cancellationToken);
    try
    {
        // Operations
        await _unitOfWork.CommitAsync(cancellationToken);
    }
    catch
    {
        await _unitOfWork.RollbackAsync(cancellationToken);
        throw;
    }
    ```

10. **Sync operations must batch commits** (don't commit per record).

---

## 🧱 **REPOSITORY RULES**

1. One repository per aggregate root:
    * `IAttendanceRepository` → Attendance entity
    * `IStudentRepository` → Student entity (read-only from CentralDB)
    * `IRouteRepository` → Route entity
    * `IQRCodeDataRepository` → QRCodeData entity
    * `IStudentFaceDataRepository` → StudentFaceData entity (Phase 2)
2. Repositories perform **data access only** — no business logic.
3. Repositories must never:
    * Contain business rules
    * Start transactions
    * Call other repositories
    * Perform validation
    * Call services
    * Access HttpContext
4. Repositories must not expose `IQueryable<T>` — always return `List<T>`, `T`, or `PagedResult<T>`.
5. Every repository method performs exactly **one database concern**.
6. Shared query logic must use **base repository or specification pattern**.
7. All repository operations must be async.
8. Use `AsNoTracking()` for all read-only operations.
9. Include navigation properties only when explicitly needed.
10. Repository methods must have clear, single-purpose names:
    * `GetByIdAsync()`
    * `GetByStudentAndCheckpointAsync()`
    * `GetPendingSyncRecordsAsync()`
    * `AddAsync()`
    * `UpdateAsync()`
    * `DeleteAsync()`
    * `ExistsAsync()`
11. Repositories must pass `CancellationToken` to all EF Core methods.
12. **CentralStudentDB repositories must be read-only** — no Add/Update/Delete methods.
13. **Attendance repositories must support bulk operations** for offline sync.
14. **QR repositories must validate hash uniqueness**.

---

## 🧱 **DOMAIN RULES (CORE LAYER)**

1. Domain entities must be persistence-agnostic.
2. No EF Core, no data annotations, no infrastructure references in entities.
3. Domain logic must be encapsulated inside entities or value objects.
4. Entities must use private setters to enforce encapsulation.
5. Domain exceptions represent business failures only:
    * `AttendanceAlreadyMarkedException`
    * `QRCodeExpiredException`
    * `StudentNotAssignedToRouteException`
    * `InvalidCheckpointSequenceException`
6. Entities may contain:
    * Properties (with private setters)
    * Navigation properties
    * Domain methods (business logic)
    * Domain events
    * Factory methods
7. Entities must never contain:
    * Persistence logic
    * Validation attributes (use FluentValidation)
    * HTTP concerns
    * Service calls
    * Static methods with side effects
8. All domain interfaces belong in Domain layer.
9. Domain layer must have **zero dependencies** on other layers.
10. **ValueObjects must be immutable** (QRCodeHash, FaceDescriptor).
11. **Enums must be used for all status fields** (AttendanceStatus, CheckpointType, RecognitionMethod).

---

## 🧱 **DTO & MAPPING RULES**

1. Entities must **never** cross API boundaries.
2. DTO naming conventions:
    * Input: `QRScanRequest`, `MarkAttendanceRequest`, `BulkSyncRequest`
    * Output: `AttendanceResponseDto`, `StudentResponseDto`, `RouteResponseDto`
    * Query: `AttendanceQueryDto`, `StudentFilterDto`, `ReportQueryDto`
3. DTOs must be explicit, minimal, and validated.
4. Mapping must be isolated using AutoMapper or dedicated mapper classes.
5. No anonymous objects or dynamic payloads across layers.
6. DTOs must use data annotations for validation:
    * `[Required]`
    * `[MaxLength]`
    * `[Range]`
    * `[RegularExpression]`
7. Complex validation must use FluentValidation.
8. Input DTOs must be immutable (init-only properties preferred).
9. **Sync DTOs must include timestamp and conflict resolution data**.
10. **QR DTOs must never expose raw hash values** (security).
11. **Response DTOs must include sync status** (synced, pending, failed).

---

## 🧱 **ASYNC & PERFORMANCE RULES**

1. All I/O operations must be async.
2. `.Result`, `.Wait()`, or any blocking calls are **forbidden**.
3. `CancellationToken` must be passed through **every layer**.
4. All async methods must use `Async` suffix.
5. Use `AsNoTracking()` for read-only queries.
6. No in-memory filtering of large datasets — use database queries.
7. Use pagination for all list endpoints:
     * `pageNumber`, `pageSize` parameters
     * Return `PagedResult<T>` with total count
     * Default page size: 50, max: 200
8. Avoid N+1 queries — use `Include()` or projections.
9. Use `Select()` projections to DTOs when possible.
10. No `ToList()` then filter — filter in database.
11. **Batch sync operations must process in chunks** (max 100 records per batch).
12. **IndexedDB queries must use indexed fields** (studentId, timestamp, synced status).
13. **API responses must be compressed** (gzip/brotli).
14. **Service workers must cache all static assets**.
15. **Database queries must have appropriate indexes**:
     * Attendance: (StudentId, CheckpointType, Date)
     * QRCodeData: (Hash, ExpirationDate)
     * StudentRouteAssignment: (StudentId, RouteId)

---
---

trigger: always_on
---

## 🧱 **ERROR HANDLING RULES**

1. Global middleware handles all exceptions.
2. Repositories throw domain-specific exceptions only:
     * `EntityNotFoundException`
     * `DuplicateEntityException`
     * `ConcurrencyException`
     * `DatabaseConnectionException`
3. Services may translate exceptions to `Result<T>` or let them bubble.
4. Controllers map results to HTTP status codes.
5. Stack traces and internal details must **never** be exposed in API responses.
6. All exceptions must be logged with structured logging (Serilog).
7. No empty catch blocks.
8. No generic `catch (Exception)` in business logic — only in global middleware.
9. **Offline sync errors must be queued for retry** — never lost.
10. **QR validation errors must return specific error codes** for PWA handling.
11. **Notification failures must be logged and retried** (max 3 attempts).

---

## 🔒 **SECURITY RULES**

1. Never expose internal database IDs in URLs — use GUIDs or encrypted IDs.
2. Authorization is **mandatory** for all non-public endpoints.
3. Validate and sanitize all inputs.
4. Never log secrets, tokens, passwords, QR hashes, or sensitive data.
5. Rely on parameterized EF Core queries only — never string concatenation.
6. Use `[Authorize]` attribute on all protected endpoints.
7. Implement role-based authorization:
     * `Staff` → Mark attendance, scan QR, view assigned routes
     * `Admin` → Manage routes, view all reports, configure system
     * `SuperAdmin` → Full system access, user management
8. Always validate user ownership of resources before modification.
9. Use HTTPS only in production.
10. Implement rate limiting on public endpoints:
     * QR generation: 100/hour per user
     * Attendance marking: 200/hour per staff
     * Sync endpoints: 50/hour per device
11. **QR codes must use HMAC-SHA256 with secret key**.
12. **QR codes must expire after 24-48 hours**.
13. **QR codes must be one-time use or track scan count**.
14. **JWT tokens must expire after 8 hours** (refresh tokens: 7 days).
15. **PWA must validate JWT on every API call**.
16. **Offline sync must authenticate before accepting data**.
17. **Camera/location permissions must be explicit opt-in**.

---

## 🧪 **TESTING RULES - BDD WITH GHERKIN**

### **General Testing Rules**

1. **All features must have BDD tests** using Gherkin syntax (SpecFlow).
2. Test structure must follow:

     ```
     TransportationAttendance.Tests/
     ├── Unit/              → Service & validator tests (xUnit)
     ├── Integration/       → Repository & database tests
     ├── BDD/
     │   ├── Features/      → .feature files (Gherkin)
     │   ├── Steps/         → Step definitions
     │   └── Hooks/         → Setup/teardown
     └── API/               → Controller endpoint tests
     ```

3. All tests must be isolated — no shared state.
4. All tests must use `async/await`.
5. All tests must clean up after themselves.
6. Tests must not depend on execution order.
7. Use in-memory database or test containers for integration tests.
8. **PWA tests must use Jest + Vue Test Utils**.

### **BDD/Gherkin Rules**

1. Every feature must have a `.feature` file in Gherkin syntax.
2. Feature files must follow this structure:

     ```gherkin
     Feature: Mark Student Attendance via QR Code
       As a transportation staff member
       I want to scan student QR codes at checkpoints
       So that attendance is recorded and parents are notified
     
     Scenario: Successfully mark attendance at pickup point
       Given a student "Ahmed Ali" is assigned to route "North-12"
       And the student has a valid QR code
       And I am at the "Pickup" checkpoint
       When I scan the student's QR code
       Then the attendance should be marked as "Present"
       And the parent should receive a notification
       And the system should show "Attendance recorded successfully"
     
     Scenario: Reject expired QR code
       Given a student has a QR code generated 3 days ago
       When I scan the expired QR code
       Then the system should show "QR code expired"
       And the attendance should not be recorded
       And I should see manual entry option
     ```

3. Scenario naming must be clear and business-focused.
4. Use `Background` for common preconditions across scenarios.
5. Use `Scenario Outline` with `Examples` for data-driven tests:

     ```gherkin
     Scenario Outline: Mark attendance at different checkpoints
       Given a student is at "<Checkpoint>"
       When attendance is marked
       Then the status should be "<Status>"
     
     Examples:
       | Checkpoint | Status    |
       | Pickup     | PickedUp  |
       | Arrival    | Arrived   |
       | Departure  | Departed  |
       | Dropoff    | DroppedOff|
     ```

6. Step definitions must be reusable across multiple scenarios.
7. One step definition class per domain aggregate.
8. Step definitions must use dependency injection.
9. Feature files must match business requirements exactly.
10. **Offline sync scenarios must be tested** (queue, retry, conflict).
11. **QR expiration scenarios must be tested**.
12. **Duplicate attendance scenarios must be tested**.

### **BDD Example Structure**

1. Feature file location: `TransportationAttendance.Tests/BDD/Features/Attendance/MarkAttendanceQR.feature`
2. Step definition location: `TransportationAttendance.Tests/BDD/Steps/AttendanceSteps.cs`
3. Feature must describe business value, not implementation.
4. Steps must use business language, not technical terms.
5. Avoid UI-specific steps (click, type) — focus on behavior.

### **Unit Test Rules**

1. All service methods require unit tests.
2. Mock all dependencies using Moq or NSubstitute.
3. Test naming: `MethodName_Scenario_ExpectedResult`
     * `MarkAttendanceAsync_ValidQRCode_ReturnsSuccess`
     * `ValidateQRCodeAsync_ExpiredCode_ThrowsQRCodeExpiredException`
4. Use `[Fact]` for single tests, `[Theory]` for parameterized tests.
5. Assert one thing per test.
6. Use `FluentAssertions` for readable assertions.
7. **Test offline sync queue logic**.
8. **Test conflict resolution strategies**.

### **Integration Test Rules**

1. All repository methods require integration tests.
2. Use real database (in-memory or test container).
3. Test actual EF Core queries and mappings.
4. Verify database state after operations.
5. Test transaction rollback scenarios.
6. **Test dual database context integration** (TransportationDB + CentralStudentDB).
7. **Test bulk sync operations with large datasets**.

---

## 📊 **OBSERVABILITY & LOGGING RULES**

1. Log service entry, exit, and failures using structured logging (Serilog).
2. Use correlation IDs for request tracking across services.
3. Logs must be structured (JSON format preferred).
4. Sensitive data must **never** be logged:
     * QR code hashes
     * Parent phone numbers
     * Student photos
     * Authentication tokens
5. Log levels:
     * `Information` → Service entry/exit, attendance marked, sync completed
     * `Warning` → Business validation failures, QR expiration, duplicate scan
     * `Error` → Exceptions, sync failures, notification failures
     * `Debug` → Development debugging only
6. Track metrics:
     * Request latency per endpoint
     * Error rates per service
     * Throughput per checkpoint
     * Sync success/failure rates
     * QR scan success rates
     * Notification delivery rates
7. Use Application Insights or similar for production monitoring.
8. **Log all offline sync operations** (queue, retry, success, failure).
9. **Log all QR generation and validation attempts**.
10. **Log all notification send attempts and results**.

---

## 🧱 **NAMING CONVENTIONS**

### **Interfaces**

1. Service interfaces: `IAttendanceService`, `IQRCodeService`, `INotificationService`
2. Repository interfaces: `IAttendanceRepository`, `IStudentRouteAssignmentRepository`
3. Unit of Work: `IUnitOfWork`

### **Implementations**

1. Services: `AttendanceService`, `QRCodeService`, `NotificationService`
2. Repositories: `AttendanceRepository`, `StudentRouteAssignmentRepository`
3. Unit of Work: `UnitOfWork`

### **DTOs**

1. Request: `QRScanRequest`, `MarkAttendanceRequest`, `BulkSyncRequest`
2. Response: `AttendanceResponseDto`, `QRCodeResponseDto`, `SyncStatusResponseDto`
3. Query: `AttendanceQueryDto`, `StudentFilterDto`, `ReportQueryDto`

### **Methods**

1. Async methods: `MarkAttendanceAsync()`, `GenerateQRCodeAsync()`, `SyncPendingRecordsAsync()`
2. Repository CRUD: `AddAsync()`, `UpdateAsync()`, `DeleteAsync()`, `GetByIdAsync()`
3. Boolean methods: `IsExpiredAsync()`, `HasAttendanceAsync()`, `ExistsAsync()`
4. Validation methods: `ValidateQRCodeAsync()`, `ValidateCheckpointSequenceAsync()`

### **Variables**

1. Use meaningful names — no abbreviations except common ones (id, dto, qr).
2. Use camelCase for local variables: `attendanceRecord`, `qrCodeData`, `syncStatus`
3. Use PascalCase for public properties: `StudentId`, `CheckpointType`, `Timestamp`
4. Avoid `var` when type is not obvious.
5. **Use descriptive names for offline sync status**: `pendingSyncCount`, `failedSyncRecords`, `lastSyncTimestamp`

---

## 🌐 **PWA-SPECIFIC RULES**

### **Offline-First Architecture**

1. **All attendance operations must work offline**.
2. **IndexedDB must be primary data storage** — localStorage only for settings.
3. **Service worker must cache all static assets** (JS, CSS, HTML, models).
4. **Service worker must implement background sync** for attendance records.
5. **Background sync must retry failed syncs** (exponential backoff).
6. **PWA must show visual sync indicators**:
     * Green: Synced
     * Yellow: Pending sync (with count)
     * Red: Sync failed (with retry option)
7. **Network status must be continuously monitored** and displayed.

### **QR Scanning Rules**

1. **QR scanner must use device camera** (no file uploads).
2. **QR scanner must validate format before API call** (client-side).
3. **QR scanner must show real-time feedback** (detecting, valid, invalid).
4. **QR scanner must support manual entry fallback**.
5. **QR scanner must throttle scan attempts** (max 1 per second).
6. **QR scanner must cache student list per route** for offline validation.

### **State Management Rules**

1. **Use Pinia for all global state** (no direct localStorage mutations).
2. **State must be persisted to IndexedDB** automatically.
3. **State must be hydrated on app load**.
4. **Auth state must sync with JWT expiration**.
5. **Sync queue state must reflect IndexedDB accurately**.

### **Performance Rules**

1. **PWA must load in under 3 seconds** on 3G connection.
2. **Service worker cache must be versioned** and updated on deployment.
3. **Large assets must be lazy-loaded** (face recognition models).
4. **Images must be optimized** (WebP format, responsive sizes).
5. **API calls must be debounced** (search inputs, filters).

---

## 🧱 **VERSIONING & DEPLOYMENT RULES**

1. All APIs must be versioned starting with `v1`.
2. Breaking changes require new major version (`v2`).
3. Maintain backward compatibility within same major version.
4. EF Core migrations must be tracked in source control.
5. Migrations must be reviewed before merge.
6. Migrations must run successfully in CI pipeline.
7. No manual database changes in production.
8. No manual production hotfixes — always through CI/CD.
9. All deployments must pass:
     * `dotnet build`
     * `dotnet test`
     * Static code analysis (SonarQube)
     * Security scans (OWASP)
     * PWA build (`npm run build`)
     * PWA tests (`npm test`)
10. **PWA version must match API version** for compatibility.
11. **Service worker version must increment on every deployment**.
12. **Database migrations must support rollback**.

---

## 🧱 **GIT WORKFLOW RULES**

1. Use feature branches: `feature/qr-attendance`, `feature/offline-sync`
2. Branch naming: `feature/`, `bugfix/`, `hotfix/`, `test/`
3. Commit messages must be descriptive and follow conventional commits:
     * `feat: Add QR code generation service`
     * `fix: Resolve offline sync conflict resolution`
     * `test: Add BDD tests for attendance marking`
     * `refactor: Extract QR validation to separate method`
     * `docs: Update API documentation`
4. No direct commits to `main` or `develop`.
5. All changes require pull requests.
6. PRs must include:
     * Description of changes
     * BDD scenarios covered
     * Screenshots (if UI changes)
     * Migration notes (if database changes)
     * Breaking changes (if any)
7. No force push to shared branches.
8. Squash commits before merging to keep history clean.
9. **Tag releases with semantic versioning** (v1.0.0, v1.1.0, v2.0.0).

---

## 🧱 **CODE REVIEW RULES**

1. No merge without peer review.
2. Minimum 1 approval required (2 for critical features).
3. PRs must document:
     * Rules followed (reference rule numbers)
     * Architecture compliance
     * Business logic preservation
     * Test coverage report
     * Performance impact
4. Reviewers must verify:
     * Clean Architecture compliance
     * Unit of Work usage
     * No EF Core outside repositories
     * No business logic in controllers
     * Async/await usage
     * DTO usage (no entity exposure)
     * BDD test coverage
     * Naming conventions
     * Offline-first architecture
     * Security best practices
5. Any PR violating architecture rules is **auto-rejected**.
6. All reviewer comments must be addressed before merge.
7. **Security-sensitive changes require security review** (auth, QR generation, data exposure).
8. **Performance-impacting changes require benchmarking**.

---

## 🧱 **CI/CD REQUIREMENTS**

1. All PRs must pass automated checks:
     * Backend build succeeds (`dotnet build`)
     * All backend tests pass (`dotnet test`)
     * Frontend build succeeds (`npm run build`)
     * All frontend tests pass (`npm test`)
     * Code coverage ≥ 80% (backend) and ≥ 70% (frontend)
     * No compiler warnings
     * Static code analysis passes (SonarQube)
     * Security scan passes (OWASP Dependency Check)
2. Deployment pipeline must include:
     * Build (backend + frontend)
     * Unit tests
     * Integration tests
     * BDD tests
     * Database migration check
     * Security scan
     * Performance tests (Lighthouse for PWA)
     * Accessibility tests (WCAG 2.1 AA)
3. Failed builds block deployment.
4. Production deployments require manual approval.
5. **Rollback procedure must be tested and documented**.
6. **Database backups must be created before migration**.

---

## 🚫 **ZERO-TOLERANCE VIOLATIONS**

1. DbContext usage outside Infrastructure → **IMMEDIATE REJECTION**
2. Business logic in Controller or Repository → **IMMEDIATE REJECTION**
3. Entity exposure to API layer → **IMMEDIATE REJECTION**
4. Sync I/O operations (`.Result`, `.Wait()`) → **IMMEDIATE REJECTION**
5. Async without `CancellationToken` → **IMMEDIATE REJECTION**
6. Skipped architectural layers → **IMMEDIATE REJECTION**
7. Ignored compiler warnings → **IMMEDIATE REJECTION**
8. Missing BDD tests for new features → **IMMEDIATE REJECTION**
9. Direct repository calls from controllers → **IMMEDIATE REJECTION**
10. No transaction management for multi-repo operations → **IMMEDIATE REJECTION**
11. Hardcoded connection strings or secrets → **IMMEDIATE REJECTION**
12. **Offline operations that don't queue for sync → IMMEDIATE REJECTION**
13. **QR codes without expiration or validation → IMMEDIATE REJECTION**
14. **Attendance operations without duplicate detection → IMMEDIATE REJECTION**
15. **Write operations on CentralStudentDB context → IMMEDIATE REJECTION**
16. **Notification failures that are silently ignored → IMMEDIATE REJECTION**
17. **Security vulnerabilities (exposed secrets, SQL injection) → IMMEDIATE REJECTION**
18. **PWA without offline functionality → IMMEDIATE REJECTION**

---

## ⚡ **FINAL RULE**

1. **If the code is not clean, layered, async, transactional, testable, BDD-covered, offline-capable, secure, and explicit — it does not belong in Transportation Attendance System.**

**No shortcuts. No exceptions. No compromises.**

---

## 📋 **PRE-MERGE CHECKLIST**

Before submitting any PR, verify:

### **Backend**

* [ ] Solution compiles with zero warnings
* [ ] All unit tests pass
* [ ] All integration tests pass
* [ ] All BDD tests pass
* [ ] Clean Architecture followed strictly
* [ ] Unit of Work pattern used for all transactions
* [ ] No EF Core outside Infrastructure layer
* [ ] No business logic in Controllers or Repositories
* [ ] All methods are async with CancellationToken
* [ ] DTOs used for all API communication
* [ ] AutoMapper configured for all mappings
* [ ] FluentValidation implemented for all requests
* [ ] BDD tests written in Gherkin for new features
* [ ] Global exception middleware handles all errors
* [ ] Structured logging implemented with correlation IDs
* [ ] Security rules followed (authorization, validation, rate limiting)
* [ ] Performance optimizations applied (AsNoTracking, pagination, indexing)
* [ ] Two-database strategy followed (no writes to CentralStudentDB)
* [ ] Offline sync endpoints support batch operations
* [ ] QR code generation includes HMAC and expiration

### **Frontend (PWA)**

* [ ] PWA builds without errors or warnings
* [ ] All component tests pass
* [ ] Offline functionality works (IndexedDB, service worker)
* [ ] Background sync configured and tested
* [ ] QR scanner works on test devices (iOS + Android)
* [ ] Camera permissions handled gracefully
* [ ] Network status indicator displays correctly
* [ ] Sync queue shows accurate pending count
* [ ] Auth tokens refresh before expiration
* [ ] State persists across app restarts
* [ ] PWA manifest configured correctly
* [ ] Service worker caches all static assets
* [ ] PWA installable on mobile devices
* [ ] Performance: Lighthouse score ≥ 90
* [ ] Accessibility: WCAG 2.1 AA compliance

### **Documentation**

* [ ] API documentation updated (if endpoints changed)
* [ ] Database schema documented (if migrations
