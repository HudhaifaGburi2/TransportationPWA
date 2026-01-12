using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportationAttendance.Application.DTOs.Common;
using TransportationAttendance.Application.DTOs.Location;
using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Domain.Authorization;

namespace TransportationAttendance.API.Controllers;

public class LocationsController : BaseApiController
{
    private readonly ILocationService _locationService;

    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LocationDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _locationService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<LocationDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("active")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LocationDto>>>> GetActive(CancellationToken cancellationToken)
    {
        var result = await _locationService.GetActiveAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<LocationDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<LocationDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _locationService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(ApiResponse<LocationDto>.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse<LocationDto>.SuccessResponse(result.Value!));
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.SuperAdmin},{Roles.SystemAdministrator}")]
    public async Task<ActionResult<ApiResponse<LocationDto>>> Create(
        [FromBody] CreateLocationDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _locationService.CreateAsync(dto, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(ApiResponse<LocationDto>.FailureResponse(result.Error!));
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id },
            ApiResponse<LocationDto>.SuccessResponse(result.Value!, "Location created successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.SuperAdmin},{Roles.SystemAdministrator}")]
    public async Task<ActionResult<ApiResponse<LocationDto>>> Update(
        Guid id,
        [FromBody] CreateLocationDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _locationService.UpdateAsync(id, dto, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error!.Contains("not found")
                ? NotFound(ApiResponse<LocationDto>.FailureResponse(result.Error!))
                : BadRequest(ApiResponse<LocationDto>.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse<LocationDto>.SuccessResponse(result.Value!, "Location updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.SuperAdmin},{Roles.SystemAdministrator}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _locationService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(ApiResponse.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse.SuccessResponse("Location deleted successfully."));
    }
}
