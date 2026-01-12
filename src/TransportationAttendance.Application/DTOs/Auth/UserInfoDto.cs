namespace TransportationAttendance.Application.DTOs.Auth;

public record UserInfoDto
{
    public string UserId { get; init; } = string.Empty;
    public string? Username { get; init; }
    public string? FullName { get; init; }
    public IReadOnlyList<string> Roles { get; init; } = Array.Empty<string>();
}
