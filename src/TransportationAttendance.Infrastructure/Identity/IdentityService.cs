using Microsoft.Extensions.Logging;
using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Domain.Entities.Central;
using TransportationAttendance.Domain.Interfaces;

namespace TransportationAttendance.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly ICentralDbRepository _centralDbRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        ICentralDbRepository centralDbRepository, 
        IJwtTokenService jwtTokenService,
        ILogger<IdentityService> logger)
    {
        _centralDbRepository = centralDbRepository;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    public async Task<AuthenticationResult> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        // Direct authentication against CentralStudentDb (fast - no external API call)
        var user = await _centralDbRepository.GetUserByUsernameAsync(username, cancellationToken);

        if (user == null || string.IsNullOrEmpty(user.UserPassword))
        {
            _logger.LogWarning("User not found: {Username}", username);
            return new AuthenticationResult(false, ErrorMessage: "Invalid username or password.");
        }

        if (user.IsActive == false)
        {
            _logger.LogWarning("User account inactive: {Username}", username);
            return new AuthenticationResult(false, ErrorMessage: "User account is inactive.");
        }

        // Verify password using MD5 hash (legacy compatibility)
        if (!MD5PasswordHasher.VerifyPassword(password, user.UserPassword))
        {
            _logger.LogWarning("Invalid password for user: {Username}", username);
            return new AuthenticationResult(false, ErrorMessage: "Invalid username or password.");
        }

        // Resolve roles from comma-separated Role_IDs
        var roles = await ResolveRolesAsync(user.RoleIdCommaSep, cancellationToken);
        var roleNames = roles.Select(r => r.RoleName ?? string.Empty).Where(r => !string.IsNullOrEmpty(r)).ToList();

        // Generate JWT token
        var token = _jwtTokenService.GenerateToken(
            user.UserId.ToString(),
            user.Username ?? user.UserId.ToString(),
            roleNames,
            user.FullNameOfficialAr ?? user.FullNameOfficialEn
        );

        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddHours(24);

        _logger.LogInformation("User {Username} authenticated successfully", username);

        return new AuthenticationResult(
            Succeeded: true,
            Token: token,
            RefreshToken: refreshToken,
            UserId: user.UserId.ToString(),
            Username: user.Username,
            FullName: user.FullNameOfficialAr ?? user.FullNameOfficialEn,
            Roles: roleNames,
            ExpiresAt: expiresAt
        );
    }

    public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        // Validate the expired token to get user info (allow expired tokens for refresh)
        var principal = _jwtTokenService.ValidateTokenIgnoreExpiry(token);
        if (principal == null)
        {
            return new AuthenticationResult(false, ErrorMessage: "Invalid token.");
        }

        var userIdClaim = principal.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return new AuthenticationResult(false, ErrorMessage: "Invalid token claims.");
        }

        // Get user from database
        var user = await _centralDbRepository.GetUserByUserIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return new AuthenticationResult(false, ErrorMessage: "User not found.");
        }

        if (user.IsActive == false)
        {
            return new AuthenticationResult(false, ErrorMessage: "User account is inactive.");
        }

        // Resolve roles
        var roles = await ResolveRolesAsync(user.RoleIdCommaSep, cancellationToken);
        var roleNames = roles.Select(r => r.RoleName).ToList();

        // Generate new tokens
        var newToken = _jwtTokenService.GenerateToken(
            user.UserId.ToString(),
            user.Username ?? user.UserId.ToString(),
            roleNames,
            user.FullNameOfficialAr ?? user.FullNameOfficialEn
        );

        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddHours(24);

        return new AuthenticationResult(
            Succeeded: true,
            Token: newToken,
            RefreshToken: newRefreshToken,
            UserId: user.UserId.ToString(),
            Username: user.Username,
            FullName: user.FullNameOfficialAr ?? user.FullNameOfficialEn,
            Roles: roleNames,
            ExpiresAt: expiresAt
        );
    }

    public async Task<UserWithRolesResult?> GetUserWithRolesAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _centralDbRepository.GetUserByUserIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return null;
        }

        var roles = await ResolveRolesAsync(user.RoleIdCommaSep, cancellationToken);
        var roleNames = roles.Select(r => r.RoleName).ToList();
        
        return new UserWithRolesResult(
            user.UserId,
            user.Username,
            user.FullNameOfficialAr,
            user.FullNameOfficialEn,
            roleNames
        );
    }

    private async Task<IReadOnlyList<RegtUserRole>> ResolveRolesAsync(string? roleIdCommaSep, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(roleIdCommaSep))
        {
            return Array.Empty<RegtUserRole>();
        }

        var roleIds = roleIdCommaSep
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => int.TryParse(s, out var id) ? id : (int?)null)
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .ToList();

        if (roleIds.Count == 0)
        {
            return Array.Empty<RegtUserRole>();
        }

        return await _centralDbRepository.GetRolesByIdsAsync(roleIds, cancellationToken);
    }
}
