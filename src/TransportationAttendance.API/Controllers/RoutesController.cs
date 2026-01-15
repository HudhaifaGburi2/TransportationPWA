using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportationAttendance.API.Infrastructure;
using TransportationAttendance.Application.DTOs.Common;
using TransportationAttendance.Application.DTOs.Route;
using TransportationAttendance.Application.Interfaces.Services;

namespace TransportationAttendance.API.Controllers;

[Authorize]
public class RoutesController : BaseApiController
{
    private readonly IRouteService _routeService;

    public RoutesController(IRouteService routeService)
    {
        _routeService = routeService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<RouteDto>>>> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _routeService.GetAllAsync(cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<RouteDto>>.FailureResponse(result.Error!));
        
        return Ok(ApiResponse<IReadOnlyList<RouteDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<RouteDto>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _routeService.GetByIdAsync(id, cancellationToken);
        
        if (result.IsFailure)
            return NotFound(ApiResponse<RouteDto>.FailureResponse(result.Error!));
        
        return Ok(ApiResponse<RouteDto>.SuccessResponse(result.Value!));
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.TumsAdminPolicy)]
    public async Task<ActionResult<ApiResponse<RouteDto>>> Create(
        [FromBody] CreateRouteDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _routeService.CreateAsync(dto, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<RouteDto>.FailureResponse(result.Error!));
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value!.RouteId },
            ApiResponse<RouteDto>.SuccessResponse(result.Value!));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.TumsAdminPolicy)]
    public async Task<ActionResult<ApiResponse<RouteDto>>> Update(
        Guid id,
        [FromBody] UpdateRouteDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _routeService.UpdateAsync(id, dto, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<RouteDto>.FailureResponse(result.Error!));
        
        return Ok(ApiResponse<RouteDto>.SuccessResponse(result.Value!));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.TumsAdminPolicy)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _routeService.DeleteAsync(id, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<bool>.FailureResponse(result.Error!));
        
        return Ok(ApiResponse<bool>.SuccessResponse(true));
    }
}
