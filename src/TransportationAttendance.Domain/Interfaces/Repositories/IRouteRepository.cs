using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IRouteRepository : IRepository<Route>
{
    Task<IReadOnlyList<Route>> GetActiveRoutesAsync(CancellationToken cancellationToken = default);
    Task<Route?> GetByNameAsync(string routeName, CancellationToken cancellationToken = default);
    Task<Route?> GetWithBusesAsync(Guid id, CancellationToken cancellationToken = default);
}
