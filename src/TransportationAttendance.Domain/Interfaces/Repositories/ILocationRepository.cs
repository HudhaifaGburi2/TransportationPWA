using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface ILocationRepository : IRepository<Location>
{
    Task<Location?> GetByCodeAsync(string locationCode, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Location>> GetActiveLocationsAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string locationCode, CancellationToken cancellationToken = default);
}
