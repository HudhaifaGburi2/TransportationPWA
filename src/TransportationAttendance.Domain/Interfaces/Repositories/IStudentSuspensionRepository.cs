using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IStudentSuspensionRepository : IRepository<StudentSuspension>
{
    Task<StudentSuspension?> GetActiveByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentSuspension>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentSuspension>> GetActiveSuspensionsAsync(CancellationToken cancellationToken = default);
    Task<bool> HasActiveSuspensionAsync(Guid studentId, CancellationToken cancellationToken = default);
}
