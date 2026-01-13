using Microsoft.Extensions.Logging;
using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Domain.Entities.Central;
using TransportationAttendance.Domain.Interfaces;

namespace TransportationAttendance.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly ICentralDbRepository _centralDbRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILoginGatewayService _loginGatewayService;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        ICentralDbRepository centralDbRepository, 
        IJwtTokenService jwtTokenService,
        ILoginGatewayService loginGatewayService,
        ILogger<IdentityService> logger)
    {
        _centralDbRepository = centralDbRepository;
        _jwtTokenService = jwtTokenService;
        _loginGatewayService = loginGatewayService;
        _logger = logger;
    }

    public async Task<AuthenticationResult> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        // Call external LoginGateway API for authentication
        var gatewayResponse = await _loginGatewayService.AuthenticateAsync(username, password, cancellationToken);

        if (gatewayResponse == null)
        {
            _logger.LogError("LoginGateway service returned null response");
            return new AuthenticationResult(false, ErrorMessage: "Authentication service unavailable.");
        }

        if (!gatewayResponse.Success || gatewayResponse.Data == null)
        {
            var errorMessage = gatewayResponse.Message ?? "Invalid username or password.";
            _logger.LogWarning("LoginGateway authentication failed: {Message}", errorMessage);
            return new AuthenticationResult(false, ErrorMessage: errorMessage);
        }

        var data = gatewayResponse.Data;
        
        // Generate our own JWT token for internal use
        var token = _jwtTokenService.GenerateToken(
            data.UserId ?? username,
            data.UserName ?? username,
            data.Roles ?? new List<string>(),
            data.FullNameOfficialAr ?? data.FullNameOfficialEn
        );

        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var expiresAt = data.ExpiresAt ?? DateTime.UtcNow.AddHours(24);

        _logger.LogInformation("User {Username} authenticated successfully via LoginGateway", username);

        return new AuthenticationResult(
            Succeeded: true,
            Token: token,
            RefreshToken: refreshToken,
            UserId: data.UserId ?? username,
            Username: data.UserName ?? username,
            FullName: data.FullNameOfficialAr ?? data.FullNameOfficialEn,
            Roles: data.Roles ?? new List<string>(),
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
