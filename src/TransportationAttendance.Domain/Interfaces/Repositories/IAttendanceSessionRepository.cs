using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Enums;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IAttendanceSessionRepository : IRepository<AttendanceSession>
{
    Task<AttendanceSession?> GetByBusAndDateAsync(Guid busId, DateOnly date, AttendanceType type, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AttendanceSession>> GetByDateAsync(DateOnly date, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AttendanceSession>> GetBySupervisorAsync(Guid supervisorId, DateOnly date, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AttendanceSession>> GetPendingSyncAsync(CancellationToken cancellationToken = default);
    Task<AttendanceSession?> GetWithRecordsAsync(Guid sessionId, CancellationToken cancellationToken = default);
}
