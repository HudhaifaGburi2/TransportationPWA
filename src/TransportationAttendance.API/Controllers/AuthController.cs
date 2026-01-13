using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportationAttendance.Application.DTOs.Auth;
using TransportationAttendance.Application.DTOs.Common;
using TransportationAttendance.Application.Interfaces;

namespace TransportationAttendance.API.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;

    public AuthController(
        IAuthService authService, 
        ILogger<AuthController> logger,
        IConfiguration configuration)
    {
        _authService = authService;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(
        [FromBody] LoginRequestDto request,
        [FromHeader(Name = "X-Login-Gateway-Token")] string? gatewayToken,
        CancellationToken cancellationToken)
    {
        // Validate gateway token (extra security layer)
        var expectedGatewayToken = _configuration["LoginGateway:BearerToken"];
        if (!string.IsNullOrWhiteSpace(expectedGatewayToken) && 
            !string.Equals(expectedGatewayToken, gatewayToken, StringComparison.Ordinal))
        {
            _logger.LogWarning("Unauthorized login gateway attempt for user: {Username}", request.Username);
            return Unauthorized(ApiResponse<LoginResponseDto>.FailureResponse("Unauthorized login gateway."));
        }

        _logger.LogInformation("Login attempt for user: {Username}", request.Username);

        var result = await _authService.LoginAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Login failed for user: {Username}. Error: {Error}", request.Username, result.Error);
            return Unauthorized(ApiResponse<LoginResponseDto>.FailureResponse(result.Error!));
        }

        _logger.LogInformation("Login successful for user: {Username}", request.Username);
        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(result.Value!, "Login successful."));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserInfoDto>>> GetCurrentUser(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(ApiResponse<UserInfoDto>.FailureResponse("User not authenticated."));
        }

        var result = await _authService.GetCurrentUserAsync(userId.Value, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(ApiResponse<UserInfoDto>.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse<UserInfoDto>.SuccessResponse(result.Value!));
    }

    [HttpPost("validate-token")]
    [Authorize]
    public ActionResult<ApiResponse<object>> ValidateToken()
    {
        var userId = GetCurrentUserId();
        var roles = GetCurrentUserRoles();

        return Ok(ApiResponse<object>.SuccessResponse(new
        {
            IsValid = true,
            UserId = userId,
            Roles = roles
        }, "Token is valid."));
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> RefreshToken(
        [FromBody] RefreshTokenRequestDto request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Token refresh attempt");

        var result = await _authService.RefreshTokenAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Token refresh failed: {Error}", result.Error);
            return Unauthorized(ApiResponse<LoginResponseDto>.FailureResponse(result.Error!));
        }

        _logger.LogInformation("Token refresh successful for user: {UserId}", result.Value!.UserId);
        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(result.Value!, "Token refreshed successfully."));
    }

    [HttpPost("logout")]
    [Authorize]
    public ActionResult<ApiResponse> Logout()
    {
        _logger.LogInformation("User {UserId} logged out", GetCurrentUserId());
        return Ok(ApiResponse.SuccessResponse("Logged out successfully."));
    }
}
