using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportationAttendance.API.Infrastructure;
using TransportationAttendance.Application.DTOs.Bus;
using TransportationAttendance.Application.DTOs.Common;
using TransportationAttendance.Application.Interfaces.Services;

namespace TransportationAttendance.API.Controllers;

[Authorize]
public class BusesController : BaseApiController
{
    private readonly IBusService _busService;

    public BusesController(IBusService busService)
    {
        _busService = busService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BusDto>>>> GetAll(
        [FromQuery] BusQueryDto query,
        CancellationToken cancellationToken)
    {
        var result = await _busService.GetAllAsync(query, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<BusDto>>.FailureResponse(result.Error!));
        
        return Ok(ApiResponse<IReadOnlyList<BusDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<BusDto>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _busService.GetByIdAsync(id, cancellationToken);
        
        if (result.IsFailure)
            return NotFound(ApiResponse<BusDto>.FailureResponse(result.Error!));
        
        return Ok(ApiResponse<BusDto>.SuccessResponse(result.Value!));
    }

    [HttpGet("by-period/{periodId:int}")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BusDto>>>> GetByPeriod(
        int periodId,
        CancellationToken cancellationToken)
    {
        var result = await _busService.GetByPeriodAsync(periodId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<BusDto>>.FailureResponse(result.Error!));
        
        return Ok(ApiResponse<IReadOnlyList<BusDto>>.SuccessResponse(result.Value!));
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<ActionResult<ApiResponse<BusDto>>> Create(
        [FromBody] CreateBusDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _busService.CreateAsync(dto, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<BusDto>.FailureResponse(result.Error!));
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value!.BusId },
            ApiResponse<BusDto>.SuccessResponse(result.Value!));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<ActionResult<ApiResponse<BusDto>>> Update(
        Guid id,
        [FromBody] UpdateBusDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _busService.UpdateAsync(id, dto, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<BusDto>.FailureResponse(result.Error!));
        
        return Ok(ApiResponse<BusDto>.SuccessResponse(result.Value!));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _busService.DeleteAsync(id, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<bool>.FailureResponse(result.Error!));
        
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }

    [HttpGet("{id:guid}/statistics")]
    public async Task<ActionResult<ApiResponse<BusStatisticsDto>>> GetStatistics(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _busService.GetStatisticsAsync(id, cancellationToken);
        
        if (result.IsFailure)
            return NotFound(ApiResponse<BusStatisticsDto>.FailureResponse(result.Error!));
        
        return Ok(ApiResponse<BusStatisticsDto>.SuccessResponse(result.Value!));
    }

    [HttpGet("summary")]
    public async Task<ActionResult<ApiResponse<BusSummaryDto>>> GetSummary(
        CancellationToken cancellationToken)
    {
        var result = await _busService.GetSummaryAsync(cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<BusSummaryDto>.FailureResponse(result.Error!));
        
        return Ok(ApiResponse<BusSummaryDto>.SuccessResponse(result.Value!));
    }

    [HttpPost("{id:guid}/districts")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<ActionResult<ApiResponse<bool>>> AssignDistricts(
        Guid id,
        [FromBody] List<Guid> districtIds,
        CancellationToken cancellationToken)
    {
        var result = await _busService.AssignDistrictsAsync(id, districtIds, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<bool>.FailureResponse(result.Error!));
        
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }
}
