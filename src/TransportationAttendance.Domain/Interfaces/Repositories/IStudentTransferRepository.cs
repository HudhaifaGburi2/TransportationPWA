using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IStudentTransferRepository : IRepository<StudentTransfer>
{
    Task<IReadOnlyList<StudentTransfer>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentTransfer>> GetByBusIdAsync(Guid busId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentTransfer>> GetRecentTransfersAsync(int count = 50, CancellationToken cancellationToken = default);
}
