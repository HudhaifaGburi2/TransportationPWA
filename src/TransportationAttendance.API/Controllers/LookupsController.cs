using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportationAttendance.API.Infrastructure;
using TransportationAttendance.Application.DTOs.Common;
using TransportationAttendance.Application.DTOs.Lookups;
using TransportationAttendance.Application.Interfaces;

namespace TransportationAttendance.API.Controllers;

[Authorize]
public class LookupsController : BaseApiController
{
    private readonly ILookupService _lookupService;

    public LookupsController(ILookupService lookupService)
    {
        _lookupService = lookupService;
    }

    [HttpGet("periods")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PeriodDto>>>> GetPeriods(
        [FromQuery] string language = "ar-SA",
        CancellationToken cancellationToken = default)
    {
        var result = await _lookupService.GetPeriodsAsync(language, cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(ApiResponse<IReadOnlyList<PeriodDto>>.FailureResponse(result.Error!));
        }
        
        return Ok(ApiResponse<IReadOnlyList<PeriodDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("periods/{id:int}")]
    public async Task<ActionResult<ApiResponse<PeriodDto>>> GetPeriodById(
        int id,
        [FromQuery] string language = "ar-SA",
        CancellationToken cancellationToken = default)
    {
        var result = await _lookupService.GetPeriodByIdAsync(id, language, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(ApiResponse<PeriodDto>.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse<PeriodDto>.SuccessResponse(result.Value!));
    }

    [HttpGet("age-groups")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AgeGroupDto>>>> GetAgeGroups(
        [FromQuery] string language = "ar-SA",
        CancellationToken cancellationToken = default)
    {
        var result = await _lookupService.GetAgeGroupsAsync(language, cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(ApiResponse<IReadOnlyList<AgeGroupDto>>.FailureResponse(result.Error!));
        }
        
        return Ok(ApiResponse<IReadOnlyList<AgeGroupDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("age-groups/{id:int}")]
    public async Task<ActionResult<ApiResponse<AgeGroupDto>>> GetAgeGroupById(
        int id,
        [FromQuery] string language = "ar-SA",
        CancellationToken cancellationToken = default)
    {
        var result = await _lookupService.GetAgeGroupByIdAsync(id, language, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(ApiResponse<AgeGroupDto>.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse<AgeGroupDto>.SuccessResponse(result.Value!));
    }

    [HttpGet("halaqa-locations")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<HalaqaLocationDto>>>> GetHalaqaLocations(
        CancellationToken cancellationToken = default)
    {
        var result = await _lookupService.GetHalaqaLocationsAsync(cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(ApiResponse<IReadOnlyList<HalaqaLocationDto>>.FailureResponse(result.Error!));
        }
        
        return Ok(ApiResponse<IReadOnlyList<HalaqaLocationDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("halaqa-locations/{id:int}")]
    public async Task<ActionResult<ApiResponse<HalaqaLocationDto>>> GetHalaqaLocationById(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _lookupService.GetHalaqaLocationByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(ApiResponse<HalaqaLocationDto>.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse<HalaqaLocationDto>.SuccessResponse(result.Value!));
    }
}
