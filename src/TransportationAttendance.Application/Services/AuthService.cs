using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Auth;
using TransportationAttendance.Application.Interfaces;

namespace TransportationAttendance.Application.Services;

public class AuthService : IAuthService
{
    private readonly IIdentityService _identityService;

    public AuthService(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _identityService.AuthenticateAsync(request.Username, request.Password, cancellationToken);

        if (!result.Succeeded)
        {
            return Result.Failure<LoginResponseDto>(result.ErrorMessage ?? "Authentication failed.");
        }

        return new LoginResponseDto
        {
            Token = result.Token!,
            RefreshToken = result.RefreshToken,
            UserId = result.UserId!,
            Username = result.Username,
            FullName = result.FullName,
            Roles = result.Roles ?? Array.Empty<string>(),
            ExpiresAt = result.ExpiresAt ?? DateTime.UtcNow.AddHours(24)
        };
    }

    public async Task<Result<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        if (!result.Succeeded)
        {
            return Result.Failure<LoginResponseDto>(result.ErrorMessage ?? "Token refresh failed.");
        }

        return new LoginResponseDto
        {
            Token = result.Token!,
            RefreshToken = result.RefreshToken,
            UserId = result.UserId!,
            Username = result.Username,
            FullName = result.FullName,
            Roles = result.Roles ?? Array.Empty<string>(),
            ExpiresAt = result.ExpiresAt ?? DateTime.UtcNow.AddHours(24)
        };
    }

    public async Task<Result<UserInfoDto>> GetCurrentUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var userResult = await _identityService.GetUserWithRolesAsync(userId, cancellationToken);

        if (userResult == null)
        {
            return Result.Failure<UserInfoDto>("User not found.");
        }

        return new UserInfoDto
        {
            UserId = userResult.UserId.ToString(),
            Username = userResult.Username,
            FullName = userResult.FullNameAr ?? userResult.FullNameEn,
            Roles = userResult.Roles.ToList()
        };
    }
}
