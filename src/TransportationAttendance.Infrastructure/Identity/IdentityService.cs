using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Domain.Entities.Central;
using TransportationAttendance.Domain.Interfaces;

namespace TransportationAttendance.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly ICentralDbRepository _centralDbRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public IdentityService(ICentralDbRepository centralDbRepository, IJwtTokenService jwtTokenService)
    {
        _centralDbRepository = centralDbRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthenticationResult> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        // Find user by username or user_id
        var user = await _centralDbRepository.GetUserByUsernameAsync(username, cancellationToken);

        if (user == null || string.IsNullOrEmpty(user.UserPassword))
        {
            return new AuthenticationResult(false, ErrorMessage: "Invalid username or password.");
        }

        if (!user.IsActive)
        {
            return new AuthenticationResult(false, ErrorMessage: "User account is inactive.");
        }

        // Verify password using MD5 hash
        if (!MD5PasswordHasher.VerifyPassword(password, user.UserPassword))
        {
            return new AuthenticationResult(false, ErrorMessage: "Invalid username or password.");
        }

        // Resolve roles from comma-separated Role_IDs
        var roles = await ResolveRolesAsync(user.RoleIdCommaSep, cancellationToken);
        var roleNames = roles.Select(r => r.RoleName).ToList();

        // Generate JWT token
        var token = _jwtTokenService.GenerateToken(
            user.UserId.ToString(),
            user.Username ?? user.UserId.ToString(),
            roleNames,
            user.FullNameOfficialAr ?? user.FullNameOfficialEn
        );

        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddHours(24);

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
