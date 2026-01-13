using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportationAttendance.API.Infrastructure;
using TransportationAttendance.Application.DTOs.Common;
using TransportationAttendance.Application.DTOs.District;
using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Domain.Authorization;

namespace TransportationAttendance.API.Controllers;

[Authorize]
public class DistrictsController : BaseApiController
{
    private readonly IDistrictService _districtService;

    public DistrictsController(IDistrictService districtService)
    {
        _districtService = districtService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<DistrictDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _districtService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<DistrictDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<DistrictDto>>>> GetActive(CancellationToken cancellationToken)
    {
        var result = await _districtService.GetActiveAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<DistrictDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DistrictDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _districtService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(ApiResponse<DistrictDto>.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse<DistrictDto>.SuccessResponse(result.Value!));
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<ActionResult<ApiResponse<DistrictDto>>> Create(
        [FromBody] CreateDistrictDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _districtService.CreateAsync(dto, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(ApiResponse<DistrictDto>.FailureResponse(result.Error!));
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, 
            ApiResponse<DistrictDto>.SuccessResponse(result.Value!, "District created successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<ActionResult<ApiResponse<DistrictDto>>> Update(
        Guid id,
        [FromBody] CreateDistrictDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _districtService.UpdateAsync(id, dto, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error!.Contains("not found") 
                ? NotFound(ApiResponse<DistrictDto>.FailureResponse(result.Error!))
                : BadRequest(ApiResponse<DistrictDto>.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse<DistrictDto>.SuccessResponse(result.Value!, "District updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _districtService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(ApiResponse.FailureResponse(result.Error!));
        }

        return Ok(ApiResponse.SuccessResponse("District deleted successfully."));
    }
}
