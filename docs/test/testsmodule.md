# 🚀 Lean API Integration Testing Module

## Production-Proven, Low Complexity, High Value

## 🎯 Core Philosophy

**Test the system exactly as the PWA uses it:**

- Real HTTP calls
- Real JWT tokens
- Real authorization policies
- Real database operations
- **No mocking of business flows**

## 📁 Simple Project Structure

```
tests/
└── TransportationAttendance.ApiTests/
    ├── TestInfrastructure/
    │   ├── TestBase.cs                    # Base class with WebApplicationFactory
    │   ├── TestAuthHelper.cs              # JWT token management
    │   ├── TestDataSeeder.cs              # Seed test data once
    │   └── TestDatabaseFixture.cs         # Database setup/teardown
    ├── Auth/
    │   ├── LoginTests.cs                  # Login, refresh token, logout
    │   └── AuthorizationTests.cs          # Role-based access
    ├── Student/
    │   ├── StudentRegistrationTests.cs    # Register from Central DB
    │   ├── StudentProfileTests.cs         # Read student info
    │   └── StudentQRCodeTests.cs          # QR generation/validation
    ├── Staff/
    │   ├── StaffAttendanceTests.cs        # Mark attendance
    │   ├── StaffRouteTests.cs             # View assigned routes
    │   └── StaffSyncTests.cs              # Offline sync operations
    ├── Admin/
    │   ├── AdminReportsTests.cs           # Reports endpoints
    │   ├── AdminRouteManagementTests.cs   # CRUD routes
    │   └── AdminUserManagementTests.cs    # Manage staff
    └── TransportationAttendance.ApiTests.csproj
```

**One test project. No BDD. No UI automation.**

## 🛠️ Minimal Tooling

```xml
<!-- TransportationAttendance.ApiTests.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <!-- Core Testing -->
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.0" />
    <PackageReference Include="xUnit" Version="2.9.0" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
    
    <!-- Integration Testing -->
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="9.0.0" />
    
    <!-- Assertions -->
    <PackageReference Include="FluentAssertions" Version="6.12.0" />
    
    <!-- Database (Optional but Recommended) -->
    <PackageReference Include="Testcontainers.MsSql" Version="3.9.0" />
    <PackageReference Include="Respawn" Version="6.2.1" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\src\TransportationAttendance.API\TransportationAttendance.API.csproj" />
  </ItemGroup>
</Project>
```

## 🔧 Core Test Infrastructure

### 1. TestBase.cs - Foundation for All Tests

```csharp
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace TransportationAttendance.ApiTests.TestInfrastructure;

public abstract class TestBase : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    protected readonly HttpClient Client;
    protected readonly WebApplicationFactory<Program> Factory;
    private readonly TestAuthHelper _authHelper;

    protected TestBase(WebApplicationFactory<Program> factory)
    {
        Factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Use test database (in-memory or Testcontainers)
                // Configure test-specific services
            });
        });

        Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        _authHelper = new TestAuthHelper(Client);
    }

    // Initialize once per test class
    public virtual async Task InitializeAsync()
    {
        await SeedTestDataAsync();
    }

    // Cleanup once per test class
    public virtual async Task DisposeAsync()
    {
        await CleanupTestDataAsync();
    }

    #region Authentication Helpers

    protected async Task<string> LoginAsStudentAsync()
        => await _authHelper.LoginAsync("student@test.com", "Student123!");

    protected async Task<string> LoginAsStaffAsync()
        => await _authHelper.LoginAsync("staff@test.com", "Staff123!");

    protected async Task<string> LoginAsSupervisorAsync()
        => await _authHelper.LoginAsync("supervisor@test.com", "Supervisor123!");

    protected async Task<string> LoginAsAdminAsync()
        => await _authHelper.LoginAsync("admin@test.com", "Admin123!");

    protected void SetAuthToken(string token)
    {
        Client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }

    #endregion

    #region HTTP Helpers

    protected async Task<HttpResponseMessage> GetAsync(string url, string? token = null)
    {
        if (token != null) SetAuthToken(token);
        return await Client.GetAsync(url);
    }

    protected async Task<HttpResponseMessage> PostAsync<T>(string url, T content, string? token = null)
    {
        if (token != null) SetAuthToken(token);
        return await Client.PostAsJsonAsync(url, content);
    }

    protected async Task<HttpResponseMessage> PutAsync<T>(string url, T content, string? token = null)
    {
        if (token != null) SetAuthToken(token);
        return await Client.PutAsJsonAsync(url, content);
    }

    protected async Task<HttpResponseMessage> DeleteAsync(string url, string? token = null)
    {
        if (token != null) SetAuthToken(token);
        return await Client.DeleteAsync(url);
    }

    protected async Task<T?> GetJsonAsync<T>(string url, string? token = null)
    {
        var response = await GetAsync(url, token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    #endregion

    #region Test Data Management

    protected virtual Task SeedTestDataAsync()
    {
        // Override in specific test classes if needed
        return Task.CompletedTask;
    }

    protected virtual Task CleanupTestDataAsync()
    {
        // Override in specific test classes if needed
        return Task.CompletedTask;
    }

    #endregion
}
```

### 2. TestAuthHelper.cs - Token Management

```csharp
using System.Net.Http.Json;

namespace TransportationAttendance.ApiTests.TestInfrastructure;

public class TestAuthHelper
{
    private readonly HttpClient _client;
    private readonly Dictionary<string, TokenCache> _tokenCache = new();

    public TestAuthHelper(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        // Check cache first
        if (_tokenCache.TryGetValue(email, out var cached) && !cached.IsExpired)
        {
            return cached.Token;
        }

        // Real login call
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new
        {
            email,
            password
        });

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        
        // Cache token
        _tokenCache[email] = new TokenCache
        {
            Token = result!.AccessToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(55) // Slightly before actual expiry
        };

        return result.AccessToken;
    }

    public async Task<string> RefreshTokenAsync(string refreshToken)
    {
        var response = await _client.PostAsJsonAsync("/api/v1/auth/refresh-token", new
        {
            refreshToken
        });

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result!.AccessToken;
    }

    private class TokenCache
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    }

    private record LoginResponse(string AccessToken, string RefreshToken, int ExpiresIn);
}
```

### 3. TestDatabaseFixture.cs - Database Setup with Testcontainers

```csharp
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;
using Respawn;
using Xunit;

namespace TransportationAttendance.ApiTests.TestInfrastructure;

public class TestDatabaseFixture : IAsyncLifetime
{
    private IContainer? _sqlServerContainer;
    private Respawner? _respawner;

    public string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        // Start SQL Server container
        _sqlServerContainer = new ContainerBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("SA_PASSWORD", "Test123!@#")
            .WithPortBinding(1433, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
            .Build();

        await _sqlServerContainer.StartAsync();

        var host = _sqlServerContainer.Hostname;
        var port = _sqlServerContainer.GetMappedPublicPort(1433);

        ConnectionString = $"Server={host},{port};Database=TransportationAttendanceTest;User Id=sa;Password=Test123!@#;TrustServerCertificate=True;";

        // Run migrations
        await ApplyMigrationsAsync();

        // Initialize Respawner for fast database cleanup
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = new[] { "dbo" },
            TablesToIgnore = new[] { "__EFMigrationsHistory" }
        });
    }

    public async Task ResetDatabaseAsync()
    {
        if (_respawner == null) return;

        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await _respawner.ResetAsync(connection);
    }

    public async Task DisposeAsync()
    {
        if (_sqlServerContainer != null)
        {
            await _sqlServerContainer.StopAsync();
            await _sqlServerContainer.DisposeAsync();
        }
    }

    private async Task ApplyMigrationsAsync()
    {
        // Apply EF Core migrations programmatically
        // Or execute SQL scripts directly
    }
}
```

## 📋 Example Test Cases

### Auth Tests

```csharp
using System.Net;
using FluentAssertions;
using TransportationAttendance.ApiTests.TestInfrastructure;
using Xunit;

namespace TransportationAttendance.ApiTests.Auth;

public class LoginTests : TestBase
{
    public LoginTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        // Act
        var response = await PostAsync("/api/v1/auth/login", new
        {
            email = "staff@test.com",
            password = "Staff123!"
        });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        result.Should().NotBeNull();
        result!.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Act
        var response = await PostAsync("/api/v1/auth/login", new
        {
            email = "staff@test.com",
            password = "WrongPassword!"
        });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RefreshToken_WithValidToken_ReturnsNewToken()
    {
        // Arrange
        var loginResponse = await PostAsync("/api/v1/auth/login", new
        {
            email = "staff@test.com",
            password = "Staff123!"
        });
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        // Act
        var response = await PostAsync("/api/v1/auth/refresh-token", new
        {
            refreshToken = loginResult!.RefreshToken
        });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        result!.AccessToken.Should().NotBeNullOrEmpty();
    }

    private record LoginResponse(string AccessToken, string RefreshToken, int ExpiresIn);
}
```

### Authorization Tests

```csharp
using System.Net;
using FluentAssertions;
using TransportationAttendance.ApiTests.TestInfrastructure;
using Xunit;

namespace TransportationAttendance.ApiTests.Auth;

public class AuthorizationTests : TestBase
{
    public AuthorizationTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task AdminEndpoint_WithoutToken_ReturnsUnauthorized()
    {
        // Act
        var response = await GetAsync("/api/v1/admin/reports");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task AdminEndpoint_WithStaffToken_ReturnsForbidden()
    {
        // Arrange
        var token = await LoginAsStaffAsync();

        // Act
        var response = await GetAsync("/api/v1/admin/reports", token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AdminEndpoint_WithAdminToken_ReturnsOk()
    {
        // Arrange
        var token = await LoginAsAdminAsync();

        // Act
        var response = await GetAsync("/api/v1/admin/reports", token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task StaffEndpoint_WithStaffToken_ReturnsOk()
    {
        // Arrange
        var token = await LoginAsStaffAsync();

        // Act
        var response = await GetAsync("/api/v1/staff/routes", token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

### Staff Attendance Tests

```csharp
using System.Net;
using FluentAssertions;
using TransportationAttendance.ApiTests.TestInfrastructure;
using Xunit;

namespace TransportationAttendance.ApiTests.Staff;

public class StaffAttendanceTests : TestBase
{
    public StaffAttendanceTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task MarkAttendance_WithValidQRCode_ReturnsSuccess()
    {
        // Arrange
        var token = await LoginAsStaffAsync();
        var studentId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890");
        
        // Act
        var response = await PostAsync("/api/v1/attendance/scan", new
        {
            qrCodeHash = "valid-qr-hash-from-test-data",
            checkpointType = "Pickup",
            latitude = 24.7136,
            longitude = 46.6753
        }, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<AttendanceResponse>();
        result.Should().NotBeNull();
        result!.Status.Should().Be("Present");
        result.StudentName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task MarkAttendance_WithExpiredQRCode_ReturnsBadRequest()
    {
        // Arrange
        var token = await LoginAsStaffAsync();
        
        // Act
        var response = await PostAsync("/api/v1/attendance/scan", new
        {
            qrCodeHash = "expired-qr-hash",
            checkpointType = "Pickup",
            latitude = 24.7136,
            longitude = 46.6753
        }, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("expired");
    }

    [Fact]
    public async Task MarkAttendance_Duplicate_ReturnsConflict()
    {
        // Arrange
        var token = await LoginAsStaffAsync();
        var request = new
        {
            qrCodeHash = "valid-qr-hash",
            checkpointType = "Pickup",
            latitude = 24.7136,
            longitude = 46.6753
        };

        // Act - First scan
        var firstResponse = await PostAsync("/api/v1/attendance/scan", request, token);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act - Duplicate scan
        var secondResponse = await PostAsync("/api/v1/attendance/scan", request, token);

        // Assert
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task SyncOfflineAttendance_WithBatchRecords_ProcessesAll()
    {
        // Arrange
        var token = await LoginAsStaffAsync();
        var batchRecords = new[]
        {
            new
            {
                clientRecordId = Guid.NewGuid().ToString(),
                studentId = Guid.NewGuid(),
                checkpointType = "Pickup",
                markedAt = DateTime.UtcNow.AddHours(-2),
                latitude = 24.7136,
                longitude = 46.6753
            },
            new
            {
                clientRecordId = Guid.NewGuid().ToString(),
                studentId = Guid.NewGuid(),
                checkpointType = "Dropoff",
                markedAt = DateTime.UtcNow.AddHours(-1),
                latitude = 24.7140,
                longitude = 46.6755
            }
        };

        // Act
        var response = await PostAsync("/api/v1/attendance/sync", new
        {
            records = batchRecords
        }, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<SyncResponse>();
        result!.SuccessCount.Should().Be(2);
        result.FailedCount.Should().Be(0);
    }

    private record AttendanceResponse(Guid Id, string StudentName, string Status, DateTime MarkedAt);
    private record SyncResponse(int SuccessCount, int FailedCount, List<string> Conflicts);
}
```

### Admin Tests

```csharp
using System.Net;
using FluentAssertions;
using TransportationAttendance.ApiTests.TestInfrastructure;
using Xunit;

namespace TransportationAttendance.ApiTests.Admin;

public class AdminReportsTests : TestBase
{
    public AdminReportsTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task GetDailyReport_WithValidDate_ReturnsReport()
    {
        // Arrange
        var token = await LoginAsAdminAsync();
        var date = DateTime.Today.ToString("yyyy-MM-dd");

        // Act
        var response = await GetAsync($"/api/v1/admin/reports/daily?date={date}", token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var report = await response.Content.ReadFromJsonAsync<DailyReport>();
        report.Should().NotBeNull();
        report!.Date.Should().Be(DateOnly.Parse(date));
        report.TotalStudents.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task GetAbsentStudents_ReturnsStudentList()
    {
        // Arrange
        var token = await LoginAsAdminAsync();

        // Act
        var response = await GetAsync("/api/v1/admin/reports/absent-students", token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var students = await response.Content.ReadFromJsonAsync<List<AbsentStudent>>();
        students.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateRoute_WithValidData_ReturnsCreatedRoute()
    {
        // Arrange
        var token = await LoginAsAdminAsync();
        var newRoute = new
        {
            name = "Test Route",
            code = "TR-001",
            district = "North District",
            pickupTime = "07:00:00",
            dropoffTime = "14:00:00",
            capacity = 40
        };

        // Act
        var response = await PostAsync("/api/v1/admin/routes", newRoute, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var route = await response.Content.ReadFromJsonAsync<RouteResponse>();
        route.Should().NotBeNull();
        route!.Name.Should().Be("Test Route");
        route.Code.Should().Be("TR-001");
    }

    private record DailyReport(DateOnly Date, int TotalStudents, int PresentCount, int AbsentCount);
    private record AbsentStudent(Guid Id, string Name, string Route);
    private record RouteResponse(Guid Id, string Name, string Code, int Capacity);
}
```

## 🚀 Running Tests

### Local Development

```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~LoginTests"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### CI/CD Integration (GitHub Actions)

```yaml
# .github/workflows/api-tests.yml
name: API Integration Tests

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET 9
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '9.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore --configuration Release
    
    - name: Run API Integration Tests
      run: dotnet test tests/TransportationAttendance.ApiTests --no-build --configuration Release --logger "trx;LogFileName=test-results.trx"
    
    - name: Publish Test Results
      uses: dorny/test-reporter@v1
      if: always()
      with:
        name: Test Results
        path: '**/test-results.trx'
        reporter: dotnet-trx
    
    - name: Generate Coverage Report
      run: |
        dotnet test tests/TransportationAttendance.ApiTests \
          /p:CollectCoverage=true \
          /p:CoverletOutputFormat=cobertura \
          /p:CoverletOutput=./coverage/
    
    - name: Upload Coverage
      uses: codecov/codecov-action@v3
      with:
        files: ./coverage/coverage.cobertura.xml
        flags: api-tests
```

### Pre-Commit Hook

```bash
# .git/hooks/pre-commit
#!/bin/sh

echo "Running API integration tests..."

dotnet test tests/TransportationAttendance.ApiTests --configuration Release --no-build

if [ $? -ne 0 ]; then
    echo "❌ Tests failed. Commit aborted."
    exit 1
fi

echo "✅ All tests passed."
exit 0
```

## 📊 What This Strategy Validates

### ✅ Automatically Tested

- **Authentication**: Login, JWT, refresh tokens
- **Authorization**: Role-based access (Staff, Admin, SuperAdmin)
- **Business Logic**: Attendance marking, duplicate detection, QR validation
- **Database Operations**: Real CRUD, transactions, constraints
- **API Contracts**: Request/response models, status codes
- **Error Handling**: Validation errors, exceptions, conflict resolution
- **Offline Sync**: Batch operations, conflict resolution
- **Integration**: Real database, real HTTP, real policies

### ❌ Deliberately NOT Tested

- UI/PWA behavior (manual testing)
- Styling/CSS
- Service worker logic (separate e2e if needed)
- Rare edge cases with minimal business impact

## 🎯 Success Metrics

- **Coverage Target**: ≥ 80% of critical API paths
- **Execution Time**: < 2 minutes for full suite
- **Maintenance Effort**: Low (no brittle mocks, no UI coupling)
- **Confidence**: High (tests mirror real usage)

## 📋 Test Checklist Per Endpoint

For each API endpoint, ensure these tests exist:

1. ✅ **Happy Path**: Valid input → Success response
2. ✅ **Authentication**: No token → 401 Unauthorized
3. ✅ **Authorization**: Wrong role → 403 Forbidden
4. ✅ **Validation**: Invalid input → 400 Bad Request
5. ✅ **Not Found**: Missing resource → 404 Not Found
6. ✅ **Conflict**: Duplicate → 409 Conflict (if applicable)

## 🔧 Configuration

### appsettings.ApiTests.json

```json
{
  "ConnectionStrings": {
    "TransportationDb": "UseTestcontainer",
    "CentralStudentDb": "Server=localhost;Database=CentralStudentDB_Test;..."
  },
  "JwtSettings": {
    "SecretKey": "test-secret-key-for-jwt-signing",
    "Issuer": "TransportationAttendanceAPI",
    "Audience": "TransportationAttendancePWA",
    "ExpirationMinutes": 60
  },
  "TestData": {
    "SeedOnStartup": true,
    "UseFakeData": true
  }
}
```

## 🎯 Summary

**This approach gives you:**

- ✅ Full automation with minimal complexity
- ✅ Real production confidence
- ✅ Low maintenance burden
- ✅ Fast execution (< 2 min)
- ✅ CI/CD ready
- ✅ No BDD overhead
- ✅ No UI coupling
- ✅ Real HTTP + Real JWT + Real DB = **Real Confidence**

**One test project. Real API calls. Maximum value.**
