using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IStudentBusAssignmentRepository : IRepository<StudentBusAssignment>
{
    Task<IReadOnlyList<StudentBusAssignment>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentBusAssignment>> GetByBusIdAsync(Guid busId, CancellationToken cancellationToken = default);
    Task<StudentBusAssignment?> GetActiveByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentBusAssignment>> GetActiveByBusIdAsync(Guid busId, CancellationToken cancellationToken = default);
}
