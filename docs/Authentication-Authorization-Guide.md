# Authentication & Authorization Implementation Guide

This document describes the complete authentication and authorization implementation used in HaramAPI-Core. Use this as a reference for implementing similar patterns in other projects.

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Project Structure](#project-structure)
3. [Configuration](#configuration)
4. [JWT Token Service](#jwt-token-service)
5. [JWT Authentication Setup](#jwt-authentication-setup)
6. [Identity Service](#identity-service)
7. [Custom User Store (Legacy DB Support)](#custom-user-store)
8. [Password Hashing (MD5 Legacy)](#password-hashing)
9. [Current User Service](#current-user-service)
10. [Authorization Policies](#authorization-policies)
11. [Role Service](#role-service)
12. [Login Endpoint](#login-endpoint)
13. [Claims Transformation (Test/Dev)](#claims-transformation)
14. [Dependency Injection Setup](#dependency-injection-setup)
15. [Usage in Controllers](#usage-in-controllers)

---

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                         API Layer                                │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐  │
│  │ UsersController │  │ [Authorize]     │  │ CurrentUser     │  │
│  │ (Login)         │  │ Controllers     │  │ Service         │  │
│  └────────┬────────┘  └────────┬────────┘  └────────┬────────┘  │
└───────────┼────────────────────┼────────────────────┼───────────┘
            │                    │                    │
┌───────────┼────────────────────┼────────────────────┼───────────┐
│           ▼                    ▼                    ▼           │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │              JWT Bearer Authentication Middleware            ││
│  │         (Validates token, extracts claims)                   ││
│  └─────────────────────────────────────────────────────────────┘│
│                      Infrastructure Layer                        │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐  │
│  │ JwtTokenService │  │ IdentityService │  │ CustomUserStore │  │
│  │ (Generate/      │  │ (Auth with MD5) │  │ (Legacy DB)     │  │
│  │  Validate)      │  │                 │  │                 │  │
│  └─────────────────┘  └─────────────────┘  └─────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

---

## Project Structure

```
src/
├── HaramAPI.API/
│   ├── Infrastructure/
│   │   ├── JwtConfiguration.cs          # JWT auth middleware setup
│   │   ├── AuthorizationPolicies.cs     # Policy definitions
│   │   └── TestUserRoleProvider.cs      # Dev/test claims transform
│   ├── Services/
│   │   └── CurrentUser.cs               # Current user accessor
│   └── Controllers/
│       └── UsersController.cs           # Login endpoint
│
├── HaramAPI.Application/
│   └── Interfaces/
│       ├── IJwtTokenService.cs          # Token service interface
│       ├── IIdentityService.cs          # Identity service interface
│       └── IUser.cs                     # Current user interface
│
├── HaramAPI.Infrastructure/
│   ├── Identity/
│   │   ├── JwtTokenService.cs           # JWT generation/validation
│   │   ├── IdentityService.cs           # User authentication
│   │   ├── EnhancedIdentityService.cs   # Extended identity service
│   │   ├── ApplicationUser.cs           # User entity
│   │   ├── CustomUserStore.cs           # Legacy DB user store
│   │   ├── CustomRoleStore.cs           # Legacy DB role store
│   │   ├── MD5PasswordHasher.cs         # Legacy password hasher
│   │   └── RoleService.cs               # Role management
│   └── DependencyInjection.cs           # Service registration
│
└── HaramAPI.Core/
    └── Constants/
        ├── Roles.cs                     # Role constants
        └── Policies.cs                  # Policy constants
```

---

## Configuration

### appsettings.json

```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "HaramAPI",
    "Audience": "HaramAPIClients"
  },
  "LoginGateway": {
    "BearerToken": "your-gateway-token-for-login-endpoint"
  }
}
```

---

## JWT Token Service

### Interface (Application Layer)

```csharp
// src/HaramAPI.Application/Interfaces/IJwtTokenService.cs
namespace HaramAPI.Application.Interfaces;

public interface IJwtTokenService
{
    /// <summary>
    /// Generates a JWT token for the specified user
    /// </summary>
    string GenerateToken(string userId, string userName, IEnumerable<string> roles);
    
    /// <summary>
    /// Validates a JWT token
    /// </summary>
    bool ValidateToken(string token);
    
    /// <summary>
    /// Gets user ID from JWT token
    /// </summary>
    string? GetUserIdFromToken(string token);
}
```

### Implementation (Infrastructure Layer)

```csharp
// src/HaramAPI.Infrastructure/Identity/JwtTokenService.cs
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HaramAPI.Infrastructure.Identity;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        var jwtSection = _configuration.GetSection("JwtSettings");
        _secretKey = jwtSection["SecretKey"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
        _issuer = jwtSection["Issuer"] ?? string.Empty;
        _audience = jwtSection["Audience"] ?? string.Empty;
    }

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
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = string.IsNullOrWhiteSpace(_issuer) ? null : _issuer,
            Audience = string.IsNullOrWhiteSpace(_audience) ? null : _audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = !string.IsNullOrWhiteSpace(_issuer),
                ValidateAudience = !string.IsNullOrWhiteSpace(_audience),
                ValidIssuer = string.IsNullOrWhiteSpace(_issuer) ? null : _issuer,
                ValidAudience = string.IsNullOrWhiteSpace(_audience) ? null : _audience,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public string? GetUserIdFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                         ?? jwt.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            return userId;
        }
        catch
        {
            return null;
        }
    }
}
```

---

## JWT Authentication Setup

### API Configuration

```csharp
// src/HaramAPI.API/Infrastructure/JwtConfiguration.cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HaramAPI.API.Infrastructure;

public static class JwtConfiguration
{
    public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is required");
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is required");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience is required");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false; // Set to true in production
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = "role",
                NameClaimType = "unique_name"
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Append("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var result = System.Text.Json.JsonSerializer.Serialize(new { error = "You are not authorized" });
                    return context.Response.WriteAsync(result);
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    var result = System.Text.Json.JsonSerializer.Serialize(new { error = "You do not have permission to access this resource" });
                    return context.Response.WriteAsync(result);
                }
            };
        });
    }
}
```

---

## Identity Service

### Interface

```csharp
// src/HaramAPI.Application/Interfaces/IIdentityService.cs
namespace HaramAPI.Application.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);
    Task<string?> GetUserIdAsync(string userName);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<bool> AuthorizeAsync(string userId, string policyName);
    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);
    Task<Result> DeleteUserAsync(string userId);
    
    // Legacy MD5 authentication
    Task<(Result Result, Regt_User User, List<Regt_User_Role> Roles)> AuthenticateWithMD5Async(string username, string password);
    Task<(Result Result, Regt_User User, List<Regt_User_Role> Roles)> AuthenticateWithMD5AndRolesAsync(string username, string password);
}
```

### Implementation (with MD5 Legacy Support)

```csharp
// src/HaramAPI.Infrastructure/Identity/IdentityService.cs (partial)
public async Task<(Result Result, Regt_User User, List<Regt_User_Role> Roles)> AuthenticateWithMD5Async(
    string username, string password)
{
    var user = await _haramContext.RegtUsers
        .FirstOrDefaultAsync(u => u.Username == username);

    if (user?.UserPassword == null)
    {
        return (Result.Failure(new[] { "Invalid username or password." }), null!, new List<Regt_User_Role>());
    }

    var encryptedInputPassword = GetMD5EncryptedPassword(password);
    if (!user.UserPassword.Equals(encryptedInputPassword))
    {
        return (Result.Failure(new[] { "Invalid username or password." }), null!, new List<Regt_User_Role>());
    }

    // Update last login date
    user.LastLoginDt = DateTime.Now;
    await _haramContext.SaveChangesAsync();

    // Parse roles from comma-separated RoleIdCommaSep
    var roles = new List<Regt_User_Role>();
    if (!string.IsNullOrWhiteSpace(user.RoleIdCommaSep))
    {
        var roleIds = user.RoleIdCommaSep
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => int.TryParse(s, out var id) ? id : (int?)null)
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .ToList();

        if (roleIds.Count > 0)
        {
            roles = await _haramContext.RegtUserRoles
                .Where(r => roleIds.Contains(r.RoleId))
                .ToListAsync();
        }
    }

    return (Result.Success(), user, roles);
}

private static string GetMD5EncryptedPassword(string password)
{
    var enc = new UTF8Encoding();
    using var md5Hasher = MD5.Create();
    var hashBytes = md5Hasher.ComputeHash(enc.GetBytes(password));
    return enc.GetString(hashBytes);
}
```

---

## Custom User Store

For working with legacy databases that don't follow ASP.NET Identity schema:

```csharp
// src/HaramAPI.Infrastructure/Identity/CustomUserStore.cs
public class CustomUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser>
{
    private readonly HaramDbContext _context;
    private readonly ILogger<CustomUserStore> _logger;

    public CustomUserStore(HaramDbContext context, ILogger<CustomUserStore> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(userId))
            return null;

        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == normalizedUserName, cancellationToken);
        
        if (user == null)
        {
            // Try case-insensitive search
            user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName!.ToUpper() == normalizedUserName.ToUpper(), 
                    cancellationToken);
        }
        
        return user;
    }

    // IUserRoleStore implementation
    public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var roleIds = user.GetRoleIds();
        
        if (roleIds.Count == 0)
            return new List<string>();

        var roleNames = await _context.RegtUserRoles
            .Where(r => roleIds.Contains(r.RoleId))
            .Select(r => r.RoleName!)
            .ToListAsync(cancellationToken);

        return roleNames;
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        var userRoles = await GetRolesAsync(user, cancellationToken);
        return userRoles.Any(r => r.Equals(roleName, StringComparison.OrdinalIgnoreCase));
    }

    // ... other interface implementations
}
```

---

## Password Hashing

For legacy systems using MD5 hashing:

```csharp
// src/HaramAPI.Infrastructure/Identity/MD5PasswordHasher.cs
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace HaramAPI.Infrastructure.Identity;

public class MD5PasswordHasher : IPasswordHasher<ApplicationUser>
{
    public string HashPassword(ApplicationUser user, string password)
    {
        return GetMD5Hash(password);
    }

    public PasswordVerificationResult VerifyHashedPassword(
        ApplicationUser user, string hashedPassword, string providedPassword)
    {
        var providedHash = GetMD5Hash(providedPassword);
        
        if (hashedPassword == providedHash)
        {
            return PasswordVerificationResult.Success;
        }
        
        return PasswordVerificationResult.Failed;
    }

    private static string GetMD5Hash(string input)
    {
        var enc = new UTF8Encoding();
        using var md5Hasher = MD5.Create();
        var hashBytes = md5Hasher.ComputeHash(enc.GetBytes(input));
        return enc.GetString(hashBytes);
    }
}
```

> **Note**: MD5 is not secure for new systems. Use this only for backward compatibility with legacy systems. For new projects, use the default ASP.NET Identity password hasher (bcrypt/PBKDF2).

---

## Current User Service

### Interface

```csharp
// src/HaramAPI.Application/Interfaces/IUser.cs
namespace HaramAPI.Application.Interfaces;

public interface IUser
{
    string? Id { get; }
    string? UserName { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    IEnumerable<string> Roles { get; }
    bool IsInRole(string role);
}
```

### Implementation

```csharp
// src/HaramAPI.API/Services/CurrentUser.cs
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace HaramAPI.API.Services;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public IEnumerable<string> Roles => _httpContextAccessor.HttpContext?.User?
        .FindAll(ClaimTypes.Role)?.Select(c => c.Value) ?? Enumerable.Empty<string>();

    public bool IsInRole(string role) => _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
}
```

---

## Authorization Policies

```csharp
// src/HaramAPI.API/Infrastructure/AuthorizationPolicies.cs
using Microsoft.AspNetCore.Authorization;

namespace HaramAPI.API.Infrastructure;

public static class AuthorizationPolicies
{
    // Policy Names
    public const string StudentPlanReadPolicy = "StudentPlanReadPolicy";
    public const string StudentPlanWritePolicy = "StudentPlanWritePolicy";
    public const string StudentEvaluationReadPolicy = "StudentEvaluationReadPolicy";
    public const string StudentEvaluationWritePolicy = "StudentEvaluationWritePolicy";
    public const string TeacherPolicy = "TeacherPolicy";
    public const string AdminPolicy = "AdminPolicy";

    public static void ConfigurePolicies(AuthorizationOptions options)
    {
        // Read Policy - Multiple roles can read
        options.AddPolicy(StudentPlanReadPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole("SYSTEM_ADMINISTRATOR", "MANAGER", "TEACHER", "STUDENT");
        });

        // Write Policy - Only specific roles can write
        options.AddPolicy(StudentPlanWritePolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole("SYSTEM_ADMINISTRATOR", "MANAGER", "TEACHER");
        });

        // Teacher Only Policy
        options.AddPolicy(TeacherPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole("TEACHER");
        });

        // Admin Only Policy
        options.AddPolicy(AdminPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole("SYSTEM_ADMINISTRATOR");
        });
    }
}
```

---

## Role Service

```csharp
// src/HaramAPI.Infrastructure/Identity/RoleService.cs
namespace HaramAPI.Infrastructure.Identity;

public interface IRoleService
{
    Task<List<string>> GetUserRoleNamesAsync(string userId);
    Task<List<string>> GetUserRoleNamesByRoleIdsAsync(List<int> roleIds);
    Task<bool> IsUserInRoleAsync(string userId, string roleName);
    Task<Dictionary<int, string>> GetAllRolesAsync();
}

public class RoleService : IRoleService
{
    private readonly HaramDbContext _context;

    public RoleService(HaramDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> GetUserRoleNamesAsync(string userId)
    {
        if (!int.TryParse(userId, out int userIdInt))
            return new List<string>();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null || string.IsNullOrEmpty(user.RoleIDsCommaSep))
            return new List<string>();

        var roleIds = user.GetRoleIds();
        return await GetUserRoleNamesByRoleIdsAsync(roleIds);
    }

    public async Task<List<string>> GetUserRoleNamesByRoleIdsAsync(List<int> roleIds)
    {
        if (roleIds.Count == 0)
            return new List<string>();

        return await _context.RegtUserRoles
            .Where(r => roleIds.Contains(r.RoleId))
            .Select(r => r.RoleName!)
            .ToListAsync();
    }

    public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
    {
        var userRoles = await GetUserRoleNamesAsync(userId);
        return userRoles.Any(r => r.Equals(roleName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Dictionary<int, string>> GetAllRolesAsync()
    {
        return await _context.RegtUserRoles
            .ToDictionaryAsync(r => r.RoleId, r => r.RoleName!);
    }
}
```

---

## Login Endpoint

```csharp
// src/HaramAPI.API/Controllers/UsersController.cs
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<UsersController> _logger;
    private readonly IConfiguration _configuration;

    public UsersController(
        IIdentityService identityService,
        IJwtTokenService jwtTokenService,
        ILogger<UsersController> logger,
        IConfiguration configuration)
    {
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login(
        [FromBody] LoginRequest request, 
        [FromHeader(Name = "X-Login-Gateway-Token")] string gatewayToken)
    {
        // Require fixed gateway token for login (extra security layer)
        var expectedGatewayToken = _configuration["LoginGateway:BearerToken"];
        if (string.IsNullOrWhiteSpace(expectedGatewayToken) || 
            !string.Equals(expectedGatewayToken, gatewayToken, StringComparison.Ordinal))
        {
            return Unauthorized(ApiResponse<LoginResponse>.ErrorResponse("Unauthorized login gateway."));
        }

        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(ApiResponse<LoginResponse>.ErrorResponse("Username and password are required."));
        }

        // Authenticate with MD5 (legacy)
        var (result, user, roles) = await _identityService.AuthenticateWithMD5AndRolesAsync(
            request.Username, request.Password);
            
        if (!result.Succeeded || user == null)
        {
            _logger.LogWarning("Failed login attempt for username: {Username}", request.Username);
            return Unauthorized(ApiResponse<LoginResponse>.ErrorResponse("Invalid username or password."));
        }

        // Generate JWT token
        var userId = user.UserId.ToString();
        var roleNames = roles.Select(r => r.RoleName).ToList();
        var token = _jwtTokenService.GenerateToken(userId, request.Username, roleNames);

        _logger.LogInformation("Successful login for user: {Username} (ID: {UserId})", request.Username, userId);

        var payload = new
        {
            token = token,
            user_id = userId,
            user_name = request.Username,
            roles = roleNames,
            expires_at = DateTime.UtcNow.AddHours(24)
        };
        
        return Ok(ApiResponse<dynamic>.SuccessResponse(payload, "Login successful."));
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
```

---

## Claims Transformation

For development/testing to assign roles without database:

```csharp
// src/HaramAPI.API/Infrastructure/TestUserRoleProvider.cs
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace HaramAPI.API.Infrastructure;

/// <summary>
/// Test user role provider for development/testing purposes
/// WARNING: Remove in production!
/// </summary>
public class TestUserRoleProvider : IClaimsTransformation
{
    private static readonly Dictionary<string, string[]> TestUserRoles = new()
    {
        { "97185", new[] { "SYSTEM_ADMINISTRATOR", "REGT_USER" } },
        { "2209", new[] { "MANAGER", "REGT_USER" } },
        { "2210", new[] { "TEACHER", "REGT_USER" } },
        { "2211", new[] { "STUDENT", "REGT_USER" } }
    };

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                     ?? principal.FindFirst("nameid")?.Value
                     ?? principal.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Task.FromResult(principal);

        if (TestUserRoles.TryGetValue(userId, out var roles))
        {
            var identity = (ClaimsIdentity)principal.Identity!;
            
            // Remove existing role claims
            var existingRoleClaims = identity.FindAll(ClaimTypes.Role).ToList();
            foreach (var claim in existingRoleClaims)
            {
                identity.RemoveClaim(claim);
            }
            
            // Add hard-coded test roles
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
                if (!identity.HasClaim("role", role))
                {
                    identity.AddClaim(new Claim("role", role));
                }
            }
        }

        return Task.FromResult(principal);
    }
}
```

---

## Dependency Injection Setup

```csharp
// src/HaramAPI.Infrastructure/DependencyInjection.cs
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDatabase(builder.Configuration);
        builder.Services.AddAuthentication(builder.Configuration);
        builder.Services.AddAuthorization();
        builder.Services.AddRepositories();
        builder.Services.AddInfrastructureServices();

        return builder.Services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // JWT Configuration
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
        var key = Encoding.ASCII.GetBytes(secretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        })
        .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddSignInManager()
            .AddApiEndpoints();

        // Custom stores for legacy database
        services.AddScoped<IUserStore<ApplicationUser>, CustomUserStore>();
        services.AddScoped<IUserPasswordStore<ApplicationUser>, CustomUserStore>();
        services.AddScoped<IUserRoleStore<ApplicationUser>, CustomUserStore>();
        services.AddScoped<IRoleStore<IdentityRole>, CustomRoleStore>();

        // MD5 password hasher for legacy compatibility
        services.AddScoped<IPasswordHasher<ApplicationUser>, MD5PasswordHasher>();

        return services;
    }

    public static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator));
        });

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IRoleService, RoleService>();
        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, EnhancedIdentityService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddMemoryCache();

        return services;
    }
}
```

### Program.cs Registration

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add infrastructure services (includes auth)
builder.AddInfrastructureServices();

// Add API-specific JWT configuration
builder.Services.ConfigureJwtAuthentication(builder.Configuration);

// Add authorization policies
builder.Services.AddAuthorization(AuthorizationPolicies.ConfigurePolicies);

// Register current user service
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUser, CurrentUser>();

var app = builder.Build();

// Middleware order matters!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
```

---

## Usage in Controllers

### Basic Authentication Requirement

```csharp
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]  // Requires any authenticated user
public class MyController : ControllerBase
{
    // All endpoints require authentication
}
```

### Role-Based Authorization

```csharp
[Authorize(Roles = "SYSTEM_ADMINISTRATOR,MANAGER")]
[HttpPost]
public async Task<IActionResult> CreateItem([FromBody] CreateItemRequest request)
{
    // Only SYSTEM_ADMINISTRATOR or MANAGER can access
}
```

### Policy-Based Authorization

```csharp
[Authorize(Policy = AuthorizationPolicies.StudentPlanWritePolicy)]
[HttpPut("{id}")]
public async Task<IActionResult> UpdatePlan(int id, [FromBody] UpdatePlanRequest request)
{
    // Policy determines allowed roles
}
```

### Accessing Current User

```csharp
public class MyController : ControllerBase
{
    private readonly IUser _currentUser;

    public MyController(IUser currentUser)
    {
        _currentUser = currentUser;
    }

    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            id = _currentUser.Id,
            username = _currentUser.UserName,
            roles = _currentUser.Roles,
            isAdmin = _currentUser.IsInRole("SYSTEM_ADMINISTRATOR")
        });
    }
}
```

---

## Required NuGet Packages

```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.0.0" />
```

---

## Security Checklist

- [ ] Use HTTPS in production (`RequireHttpsMetadata = true`)
- [ ] Use strong secret keys (32+ characters)
- [ ] Store secrets in environment variables or Azure Key Vault
- [ ] Remove `TestUserRoleProvider` in production
- [ ] Set appropriate token expiration times
- [ ] Implement refresh tokens for long-lived sessions
- [ ] Use modern password hashing (not MD5) for new systems
- [ ] Validate all inputs
- [ ] Log authentication failures
- [ ] Implement rate limiting on login endpoints

---

## Summary

This implementation provides:

1. **JWT-based authentication** with configurable issuer/audience
2. **Role-based authorization** with policy support
3. **Legacy database compatibility** via custom user/role stores
4. **MD5 password support** for backward compatibility
5. **Current user accessor** for easy user info retrieval in services
6. **Claims transformation** for development/testing scenarios

Adapt this implementation to your specific needs, ensuring you follow security best practices for production deployments.
