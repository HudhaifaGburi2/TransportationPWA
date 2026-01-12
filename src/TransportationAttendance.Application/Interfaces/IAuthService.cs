using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Auth;

namespace TransportationAttendance.Application.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<UserInfoDto>> GetCurrentUserAsync(int userId, CancellationToken cancellationToken = default);
}
