using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Enums;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IRegistrationRequestRepository : IRepository<RegistrationRequest>
{
    Task<IReadOnlyList<RegistrationRequest>> GetByStatusAsync(RegistrationStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RegistrationRequest>> GetByDistrictAsync(Guid districtId, CancellationToken cancellationToken = default);
    Task<RegistrationRequest?> GetByStudentUserIdAsync(int studentUserId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RegistrationRequest>> GetPendingRequestsAsync(CancellationToken cancellationToken = default);
    Task<bool> HasPendingRequestAsync(int studentUserId, CancellationToken cancellationToken = default);
}
