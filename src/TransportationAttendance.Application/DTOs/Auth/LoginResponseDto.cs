namespace TransportationAttendance.Application.DTOs.Auth;

public record LoginResponseDto
{
    public string Token { get; init; } = string.Empty;
    public string? RefreshToken { get; init; }
    public string UserId { get; init; } = string.Empty;
    public string? Username { get; init; }
    public string? FullName { get; init; }
    public IReadOnlyList<string> Roles { get; init; } = Array.Empty<string>();
    public DateTime ExpiresAt { get; init; }
}
