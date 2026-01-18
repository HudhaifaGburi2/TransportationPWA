using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IRouteRepository
{
    Task<Route?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Route>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Route?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Route>> GetActiveRoutesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Route>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task AddAsync(Route route, CancellationToken cancellationToken = default);
    void Update(Route route);
    void Remove(Route route);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
}
