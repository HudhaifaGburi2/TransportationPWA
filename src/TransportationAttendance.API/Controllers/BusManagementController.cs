using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportationAttendance.Application.DTOs.BusManagement;
using TransportationAttendance.Application.DTOs.Common;
using TransportationAttendance.Application.Interfaces.Services;

namespace TransportationAttendance.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BusManagementController : ControllerBase
{
    private readonly IBusManagementService _busManagementService;
    private readonly ILogger<BusManagementController> _logger;

    public BusManagementController(IBusManagementService busManagementService, ILogger<BusManagementController> logger)
    {
        _busManagementService = busManagementService;
        _logger = logger;
    }

    #region Driver Endpoints

    [HttpGet("drivers")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<DriverManagementDto>>>> GetAllDrivers(
        [FromQuery] DriverQueryDto? query,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.GetAllDriversAsync(query, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<DriverManagementDto>>.FailureResponse(result.Error!));

        return Ok(ApiResponse<IReadOnlyList<DriverManagementDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("drivers/{id:guid}")]
    public async Task<ActionResult<ApiResponse<DriverManagementDto>>> GetDriverById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.GetDriverByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<DriverManagementDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<DriverManagementDto>.SuccessResponse(result.Value!));
    }

    [HttpPost("drivers")]
    public async Task<ActionResult<ApiResponse<DriverManagementDto>>> CreateDriver(
        [FromBody] CreateDriverManagementDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.CreateDriverAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<DriverManagementDto>.FailureResponse(result.Error!));

        return CreatedAtAction(nameof(GetDriverById), new { id = result.Value!.Id },
            ApiResponse<DriverManagementDto>.SuccessResponse(result.Value!, "تم إضافة السائق بنجاح"));
    }

    [HttpPut("drivers/{id:guid}")]
    public async Task<ActionResult<ApiResponse<DriverManagementDto>>> UpdateDriver(
        Guid id,
        [FromBody] UpdateDriverManagementDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.UpdateDriverAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<DriverManagementDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<DriverManagementDto>.SuccessResponse(result.Value!, "تم تحديث بيانات السائق بنجاح"));
    }

    [HttpDelete("drivers/{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteDriver(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.DeleteDriverAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<bool>.FailureResponse(result.Error!));

        return Ok(ApiResponse<bool>.SuccessResponse(true, "تم حذف السائق بنجاح"));
    }

    #endregion

    #region Route Endpoints

    [HttpGet("routes")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<RouteManagementDto>>>> GetAllRoutes(
        [FromQuery] RouteQueryDto? query,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.GetAllRoutesAsync(query, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<RouteManagementDto>>.FailureResponse(result.Error!));

        return Ok(ApiResponse<IReadOnlyList<RouteManagementDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("routes/{id:guid}")]
    public async Task<ActionResult<ApiResponse<RouteManagementDto>>> GetRouteById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.GetRouteByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<RouteManagementDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<RouteManagementDto>.SuccessResponse(result.Value!));
    }

    [HttpPost("routes")]
    public async Task<ActionResult<ApiResponse<RouteManagementDto>>> CreateRoute(
        [FromBody] CreateRouteManagementDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.CreateRouteAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<RouteManagementDto>.FailureResponse(result.Error!));

        return CreatedAtAction(nameof(GetRouteById), new { id = result.Value!.Id },
            ApiResponse<RouteManagementDto>.SuccessResponse(result.Value!, "تم إضافة المسار بنجاح"));
    }

    [HttpPut("routes/{id:guid}")]
    public async Task<ActionResult<ApiResponse<RouteManagementDto>>> UpdateRoute(
        Guid id,
        [FromBody] UpdateRouteManagementDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.UpdateRouteAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<RouteManagementDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<RouteManagementDto>.SuccessResponse(result.Value!, "تم تحديث بيانات المسار بنجاح"));
    }

    [HttpDelete("routes/{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteRoute(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.DeleteRouteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<bool>.FailureResponse(result.Error!));

        return Ok(ApiResponse<bool>.SuccessResponse(true, "تم حذف المسار بنجاح"));
    }

    #endregion

    #region Bus Endpoints

    [HttpGet("buses")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BusManagementDto>>>> GetAllBuses(
        [FromQuery] BusManagementQueryDto? query,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.GetAllBusesAsync(query, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<BusManagementDto>>.FailureResponse(result.Error!));

        return Ok(ApiResponse<IReadOnlyList<BusManagementDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("buses/{id:guid}")]
    public async Task<ActionResult<ApiResponse<BusManagementDto>>> GetBusById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.GetBusByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<BusManagementDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<BusManagementDto>.SuccessResponse(result.Value!));
    }

    [HttpPost("buses")]
    public async Task<ActionResult<ApiResponse<BusManagementDto>>> CreateBus(
        [FromBody] CreateBusManagementDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.CreateBusAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<BusManagementDto>.FailureResponse(result.Error!));

        return CreatedAtAction(nameof(GetBusById), new { id = result.Value!.Id },
            ApiResponse<BusManagementDto>.SuccessResponse(result.Value!, "تم إضافة الباص بنجاح"));
    }

    [HttpPut("buses/{id:guid}")]
    public async Task<ActionResult<ApiResponse<BusManagementDto>>> UpdateBus(
        Guid id,
        [FromBody] UpdateBusManagementDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.UpdateBusAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<BusManagementDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<BusManagementDto>.SuccessResponse(result.Value!, "تم تحديث بيانات الباص بنجاح"));
    }

    [HttpDelete("buses/{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteBus(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.DeleteBusAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<bool>.FailureResponse(result.Error!));

        return Ok(ApiResponse<bool>.SuccessResponse(true, "تم حذف الباص بنجاح"));
    }

    #endregion

    #region Statistics

    [HttpGet("statistics")]
    public async Task<ActionResult<ApiResponse<BusManagementStatisticsDto>>> GetStatistics(
        CancellationToken cancellationToken)
    {
        var result = await _busManagementService.GetStatisticsAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<BusManagementStatisticsDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<BusManagementStatisticsDto>.SuccessResponse(result.Value!));
    }

    #endregion
}
