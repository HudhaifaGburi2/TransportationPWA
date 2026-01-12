namespace TransportationAttendance.Application.Interfaces;

public record AuthenticationResult(
    bool Succeeded,
    string? Token = null,
    string? RefreshToken = null,
    string? UserId = null,
    string? Username = null,
    string? FullName = null,
    IReadOnlyList<string>? Roles = null,
    DateTime? ExpiresAt = null,
    string? ErrorMessage = null);

public record UserWithRolesResult(
    int UserId,
    string? Username,
    string? FullNameAr,
    string? FullNameEn,
    IReadOnlyList<string> Roles);

public interface IIdentityService
{
    Task<AuthenticationResult> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
    Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<UserWithRolesResult?> GetUserWithRolesAsync(int userId, CancellationToken cancellationToken = default);
}
