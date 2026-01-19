using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportationAttendance.API.Infrastructure;
using TransportationAttendance.Application.DTOs.Common;
using TransportationAttendance.Application.DTOs.Registration;
using TransportationAttendance.Application.DTOs.Student;
using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Domain.Authorization;

namespace TransportationAttendance.API.Controllers;

[Authorize]
public class RegistrationController : BaseApiController
{
    private readonly IRegistrationService _registrationService;
    private readonly ILogger<RegistrationController> _logger;

    public RegistrationController(
        IRegistrationService registrationService, 
        ILogger<RegistrationController> logger)
    {
        _registrationService = registrationService;
        _logger = logger;
    }

    [HttpGet("student-info")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<StudentHalaqaInfoDto>>> GetStudentInfoForRegistration(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(ApiResponse<StudentHalaqaInfoDto>.FailureResponse("User not authenticated."));
        }

        var roles = GetCurrentUserRoles();
        _logger.LogInformation("User {UserId} accessing student-info with roles: {Roles}", userId, string.Join(", ", roles));

        var result = await _registrationService.GetStudentInfoForRegistrationAsync(userId.Value, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(ApiResponse<StudentHalaqaInfoDto>.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse<StudentHalaqaInfoDto>.SuccessResponse(result.Value!));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ApiResponse<RegistrationRequestDto>>> SubmitRegistration(
        [FromBody] CreateRegistrationRequestDto dto,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(ApiResponse<RegistrationRequestDto>.FailureResponse("User not authenticated."));
        }

        _logger.LogInformation("Student {UserId} submitting registration request", userId);

        var result = await _registrationService.SubmitRegistrationAsync(userId.Value, dto, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Registration failed for student {UserId}: {Error}", userId, result.Error);
            return BadRequest(ApiResponse<RegistrationRequestDto>.FailureResponse(result.Error!));
        }

        _logger.LogInformation("Registration submitted successfully for student {UserId}", userId);
        return CreatedAtAction(nameof(GetMyRegistration), null,
            ApiResponse<RegistrationRequestDto>.SuccessResponse(result.Value!, "Registration submitted successfully."));
    }

    [HttpGet("my-registration")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<RegistrationRequestDto>>> GetMyRegistration(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(ApiResponse<RegistrationRequestDto>.FailureResponse("User not authenticated."));
        }

        var result = await _registrationService.GetMyRegistrationAsync(userId.Value, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(ApiResponse<RegistrationRequestDto>.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse<RegistrationRequestDto>.SuccessResponse(result.Value!));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<RegistrationRequestDto>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _registrationService.GetRegistrationByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(ApiResponse<RegistrationRequestDto>.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse<RegistrationRequestDto>.SuccessResponse(result.Value!));
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<RegistrationRequestDto>>>> GetAllRegistrations(
        CancellationToken cancellationToken)
    {
        var result = await _registrationService.GetAllRegistrationsAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<RegistrationRequestDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("pending")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<RegistrationRequestDto>>>> GetPendingRegistrations(
        CancellationToken cancellationToken)
    {
        var result = await _registrationService.GetPendingRegistrationsAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<RegistrationRequestDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("by-district/{districtId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<RegistrationRequestDto>>>> GetByDistrict(
        Guid districtId,
        CancellationToken cancellationToken)
    {
        var result = await _registrationService.GetRegistrationsByDistrictAsync(districtId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<RegistrationRequestDto>>.SuccessResponse(result.Value!));
    }

    [HttpPost("{id:guid}/review")]
    [Authorize(Policy = AuthorizationPolicies.TumsAdminPolicy)]
    public async Task<ActionResult<ApiResponse<RegistrationRequestDto>>> ReviewRegistration(
        Guid id,
        [FromBody] ReviewRegistrationRequestDto dto,
        CancellationToken cancellationToken)
    {
        var reviewerId = GetCurrentUserGuid();
        if (reviewerId == null)
        {
            return Unauthorized(ApiResponse<RegistrationRequestDto>.FailureResponse("User not authenticated."));
        }

        _logger.LogInformation("Reviewing registration {RegistrationId} by {ReviewerId}", id, reviewerId);

        var result = await _registrationService.ReviewRegistrationAsync(id, reviewerId.Value, dto, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(ApiResponse<RegistrationRequestDto>.FailureResponse(result.Error!));
        }

        var action = dto.IsApproved ? "approved" : "rejected";
        _logger.LogInformation("Registration {RegistrationId} {Action} by {ReviewerId}", id, action, reviewerId);

        return Ok(ApiResponse<RegistrationRequestDto>.SuccessResponse(result.Value!, $"Registration {action} successfully."));
    }

    [HttpPost("{id:guid}/assign-bus")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<RegistrationRequestDto>>> AssignToBus(
        Guid id,
        [FromBody] AssignBusRequestDto dto,
        CancellationToken cancellationToken)
    {
        var reviewerId = GetCurrentUserGuid();
        if (reviewerId == null)
        {
            return Unauthorized(ApiResponse<RegistrationRequestDto>.FailureResponse("User not authenticated."));
        }

        _logger.LogInformation("Assigning registration {RegistrationId} to bus {BusId} by {ReviewerId}", id, dto.BusId, reviewerId);

        var result = await _registrationService.AssignToBusAsync(id, reviewerId.Value, dto, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(ApiResponse<RegistrationRequestDto>.FailureResponse(result.Error!));
        }

        _logger.LogInformation("Registration {RegistrationId} assigned to bus {BusId} by {ReviewerId}", id, dto.BusId, reviewerId);

        return Ok(ApiResponse<RegistrationRequestDto>.SuccessResponse(result.Value!, "Student assigned to bus successfully."));
    }
}
