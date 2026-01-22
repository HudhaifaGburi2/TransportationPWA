using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportationAttendance.API.Infrastructure;
using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Common;
using TransportationAttendance.Application.DTOs.Student;
using TransportationAttendance.Application.Interfaces.Services;
using TransportationAttendance.Domain.Authorization;

namespace TransportationAttendance.API.Controllers;

[Authorize]
public class StudentsController : BaseApiController
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(
        IStudentService studentService,
        ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    #region Search and Retrieval

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<PagedResult<StudentDto>>>> SearchStudents(
        [FromQuery] StudentSearchQueryDto query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching students with query: {Query}", query.SearchTerm);
        var result = await _studentService.SearchStudentsAsync(query, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<StudentDto>>.FailureResponse(result.Error!));

        return Ok(ApiResponse<PagedResult<StudentDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<StudentDto>>> GetStudentById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetStudentByIdAsync(id, cancellationToken);
        
        if (result.IsFailure)
            return NotFound(ApiResponse<StudentDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<StudentDto>.SuccessResponse(result.Value!));
    }

    [HttpGet("by-student-id/{studentId}")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<StudentDto>>> GetStudentByStudentId(
        string studentId,
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetStudentByStudentIdAsync(studentId, cancellationToken);
        
        if (result.IsFailure)
            return NotFound(ApiResponse<StudentDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<StudentDto>.SuccessResponse(result.Value!));
    }

    [HttpGet("district/{districtId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<StudentDto>>>> GetStudentsByDistrict(
        Guid districtId,
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetStudentsByDistrictAsync(districtId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<StudentDto>>.FailureResponse(result.Error!));

        return Ok(ApiResponse<IReadOnlyList<StudentDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("active")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<StudentDto>>>> GetActiveStudents(
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetActiveStudentsAsync(cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<StudentDto>>.FailureResponse(result.Error!));

        return Ok(ApiResponse<IReadOnlyList<StudentDto>>.SuccessResponse(result.Value!));
    }

    #endregion

    #region Assignment Operations

    [HttpPost("assignments")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<StudentAssignmentDto>>> AssignStudentToBus(
        [FromBody] CreateAssignmentDto dto,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserGuid() ?? Guid.Empty;
        _logger.LogInformation("Assigning student {StudentId} to bus {BusId}", dto.StudentId, dto.BusId);
        
        var result = await _studentService.AssignStudentToBusAsync(dto, userId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<StudentAssignmentDto>.FailureResponse(result.Error!));

        return CreatedAtAction(nameof(GetStudentAssignment), new { studentId = dto.StudentId }, 
            ApiResponse<StudentAssignmentDto>.SuccessResponse(result.Value!));
    }

    [HttpPut("{studentId:guid}/assignment")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<StudentAssignmentDto>>> UpdateStudentAssignment(
        Guid studentId,
        [FromBody] UpdateAssignmentDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _studentService.UpdateStudentAssignmentAsync(studentId, dto, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<StudentAssignmentDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<StudentAssignmentDto>.SuccessResponse(result.Value!));
    }

    [HttpGet("{studentId:guid}/assignment")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<StudentAssignmentDto>>> GetStudentAssignment(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetStudentAssignmentAsync(studentId, cancellationToken);
        
        if (result.IsFailure)
            return NotFound(ApiResponse<StudentAssignmentDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<StudentAssignmentDto>.SuccessResponse(result.Value!));
    }

    [HttpGet("bus/{busId:guid}/students")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<StudentAssignmentDto>>>> GetStudentsByBus(
        Guid busId,
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetStudentsByBusAsync(busId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<StudentAssignmentDto>>.FailureResponse(result.Error!));

        return Ok(ApiResponse<IReadOnlyList<StudentAssignmentDto>>.SuccessResponse(result.Value!));
    }

    #endregion

    #region Suspension Operations

    [HttpPost("suspensions")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<StudentSuspensionDto>>> SuspendStudent(
        [FromBody] CreateSuspensionDto dto,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserGuid() ?? Guid.Empty;
        _logger.LogInformation("Suspending student {StudentId}", dto.StudentId);
        
        var result = await _studentService.SuspendStudentAsync(dto, userId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<StudentSuspensionDto>.FailureResponse(result.Error!));

        return CreatedAtAction(nameof(GetSuspendedStudents), null, 
            ApiResponse<StudentSuspensionDto>.SuccessResponse(result.Value!));
    }

    [HttpPost("suspensions/{suspensionId:guid}/reactivate")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<StudentSuspensionDto>>> ReactivateStudent(
        Guid suspensionId,
        [FromBody] ReactivateStudentDto dto,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserGuid() ?? Guid.Empty;
        _logger.LogInformation("Reactivating suspension {SuspensionId}", suspensionId);
        
        var result = await _studentService.ReactivateStudentAsync(suspensionId, dto, userId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<StudentSuspensionDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<StudentSuspensionDto>.SuccessResponse(result.Value!));
    }

    [HttpGet("suspended")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<StudentSuspensionDto>>>> GetSuspendedStudents(
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetSuspendedStudentsAsync(cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<StudentSuspensionDto>>.FailureResponse(result.Error!));

        return Ok(ApiResponse<IReadOnlyList<StudentSuspensionDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("{studentId:guid}/suspensions")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<StudentSuspensionDto>>>> GetStudentSuspensionHistory(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetStudentSuspensionHistoryAsync(studentId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<StudentSuspensionDto>>.FailureResponse(result.Error!));

        return Ok(ApiResponse<IReadOnlyList<StudentSuspensionDto>>.SuccessResponse(result.Value!));
    }

    #endregion

    #region Leave Operations

    [HttpPost("leaves")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<StudentLeaveDto>>> CreateLeave(
        [FromBody] CreateLeaveDto dto,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserGuid() ?? Guid.Empty;
        _logger.LogInformation("Creating leave for student {StudentId}", dto.StudentId);
        
        var result = await _studentService.CreateLeaveAsync(dto, userId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<StudentLeaveDto>.FailureResponse(result.Error!));

        return CreatedAtAction(nameof(GetPendingLeaves), null, 
            ApiResponse<StudentLeaveDto>.SuccessResponse(result.Value!));
    }

    [HttpPost("leaves/{leaveId:guid}/approve")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<StudentLeaveDto>>> ApproveLeave(
        Guid leaveId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserGuid() ?? Guid.Empty;
        _logger.LogInformation("Approving leave {LeaveId}", leaveId);
        
        var result = await _studentService.ApproveLeaveAsync(leaveId, userId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<StudentLeaveDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<StudentLeaveDto>.SuccessResponse(result.Value!));
    }

    [HttpPost("leaves/{leaveId:guid}/cancel")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<StudentLeaveDto>>> CancelLeave(
        Guid leaveId,
        [FromBody] CancelLeaveDto dto,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserGuid() ?? Guid.Empty;
        _logger.LogInformation("Cancelling leave {LeaveId}", leaveId);
        
        var result = await _studentService.CancelLeaveAsync(leaveId, dto, userId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<StudentLeaveDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<StudentLeaveDto>.SuccessResponse(result.Value!));
    }

    [HttpGet("leaves/pending")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<StudentLeaveDto>>>> GetPendingLeaves(
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetPendingLeavesAsync(cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<StudentLeaveDto>>.FailureResponse(result.Error!));

        return Ok(ApiResponse<IReadOnlyList<StudentLeaveDto>>.SuccessResponse(result.Value!));
    }

    [HttpGet("{studentId:guid}/leaves")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<StudentLeaveDto>>>> GetStudentLeaveHistory(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetStudentLeaveHistoryAsync(studentId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<StudentLeaveDto>>.FailureResponse(result.Error!));

        return Ok(ApiResponse<IReadOnlyList<StudentLeaveDto>>.SuccessResponse(result.Value!));
    }

    #endregion

    #region Transfer Operations

    [HttpPost("transfers")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<StudentTransferDto>>> TransferStudent(
        [FromBody] CreateTransferDto dto,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserGuid() ?? Guid.Empty;
        _logger.LogInformation("Transferring student {StudentId} to bus {ToBusId}", dto.StudentId, dto.ToBusId);
        
        var result = await _studentService.TransferStudentAsync(dto, userId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<StudentTransferDto>.FailureResponse(result.Error!));

        return CreatedAtAction(nameof(GetStudentTransferHistory), new { studentId = dto.StudentId }, 
            ApiResponse<StudentTransferDto>.SuccessResponse(result.Value!));
    }

    [HttpGet("{studentId:guid}/transfers")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<StudentTransferDto>>>> GetStudentTransferHistory(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetStudentTransferHistoryAsync(studentId, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<StudentTransferDto>>.FailureResponse(result.Error!));

        return Ok(ApiResponse<IReadOnlyList<StudentTransferDto>>.SuccessResponse(result.Value!));
    }

    #endregion

    #region Timeline

    [HttpGet("{studentId:guid}/timeline")]
    [Authorize(Policy = AuthorizationPolicies.TumsStaffPolicy)]
    public async Task<ActionResult<ApiResponse<StudentTimelineDto>>> GetStudentTimeline(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetStudentTimelineAsync(studentId, cancellationToken);
        
        if (result.IsFailure)
            return NotFound(ApiResponse<StudentTimelineDto>.FailureResponse(result.Error!));

        return Ok(ApiResponse<StudentTimelineDto>.SuccessResponse(result.Value!));
    }

    #endregion
}
