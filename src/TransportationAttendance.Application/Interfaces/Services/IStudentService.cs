using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Student;

namespace TransportationAttendance.Application.Interfaces.Services;

public interface IStudentService
{
    // Search and Retrieval
    Task<Result<PagedResult<StudentDto>>> SearchStudentsAsync(StudentSearchQueryDto query, CancellationToken cancellationToken = default);
    Task<Result<StudentDto>> GetStudentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<StudentDto>> GetStudentByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<StudentDto>>> GetStudentsByDistrictAsync(Guid districtId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<StudentDto>>> GetActiveStudentsAsync(CancellationToken cancellationToken = default);

    // Assignment Operations
    Task<Result<StudentAssignmentDto>> AssignStudentToBusAsync(CreateAssignmentDto dto, Guid assignedBy, CancellationToken cancellationToken = default);
    Task<Result<StudentAssignmentDto>> UpdateStudentAssignmentAsync(Guid studentId, UpdateAssignmentDto dto, CancellationToken cancellationToken = default);
    Task<Result<StudentAssignmentDto>> GetStudentAssignmentAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<StudentAssignmentDto>>> GetStudentsByBusAsync(Guid busId, CancellationToken cancellationToken = default);

    // Suspension Operations
    Task<Result<StudentSuspensionDto>> SuspendStudentAsync(CreateSuspensionDto dto, Guid suspendedBy, CancellationToken cancellationToken = default);
    Task<Result<StudentSuspensionDto>> ReactivateStudentAsync(Guid suspensionId, ReactivateStudentDto dto, Guid reactivatedBy, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<StudentSuspensionDto>>> GetSuspendedStudentsAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<StudentSuspensionDto>>> GetStudentSuspensionHistoryAsync(Guid studentId, CancellationToken cancellationToken = default);

    // Leave Operations
    Task<Result<StudentLeaveDto>> CreateLeaveAsync(CreateLeaveDto dto, Guid createdBy, CancellationToken cancellationToken = default);
    Task<Result<StudentLeaveDto>> ApproveLeaveAsync(Guid leaveId, Guid approvedBy, CancellationToken cancellationToken = default);
    Task<Result<StudentLeaveDto>> CancelLeaveAsync(Guid leaveId, CancelLeaveDto dto, Guid cancelledBy, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<StudentLeaveDto>>> GetPendingLeavesAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<StudentLeaveDto>>> GetStudentLeaveHistoryAsync(Guid studentId, CancellationToken cancellationToken = default);

    // Transfer Operations
    Task<Result<StudentTransferDto>> TransferStudentAsync(CreateTransferDto dto, Guid transferredBy, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<StudentTransferDto>>> GetStudentTransferHistoryAsync(Guid studentId, CancellationToken cancellationToken = default);

    // Timeline
    Task<Result<StudentTimelineDto>> GetStudentTimelineAsync(Guid studentId, CancellationToken cancellationToken = default);
}
