using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Domain.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IStudentRepository Students { get; }
    IDistrictRepository Districts { get; }
    ILocationRepository Locations { get; }
    IRegistrationRequestRepository RegistrationRequests { get; }
    IBusRepository Buses { get; }
    IRouteRepository Routes { get; }
    IDriverRepository Drivers { get; }
    IStudentBusAssignmentRepository StudentBusAssignments { get; }
    IStudentSuspensionRepository StudentSuspensions { get; }
    IStudentLeaveRepository StudentLeaves { get; }
    IStudentTransferRepository StudentTransfers { get; }
    IAttendanceSessionRepository AttendanceSessions { get; }
    IAuditLogRepository AuditLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
