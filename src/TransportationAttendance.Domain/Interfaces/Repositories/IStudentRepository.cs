using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
    Task<Student?> GetByUserIdAsync(int studentUserId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Student>> GetByDistrictAsync(Guid districtId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Student>> GetActiveStudentsAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUserIdAsync(int studentUserId, CancellationToken cancellationToken = default);
}
