using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.Auth;

public record RefreshTokenRequestDto
{
    [Required]
    public string Token { get; init; } = string.Empty;

    [Required]
    public string RefreshToken { get; init; } = string.Empty;
}
