using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IStudentLeaveRepository : IRepository<StudentLeave>
{
    Task<StudentLeave?> GetActiveByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentLeave>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentLeave>> GetPendingLeavesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentLeave>> GetActiveLeavesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentLeave>> GetLeavesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<bool> HasOverlappingLeaveAsync(Guid studentId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
