using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IActualRouteRepository
{
    Task<ActualRoute?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActualRoute>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ActualRoute?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<ActualRoute?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActualRoute>> GetActiveRoutesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActualRoute>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task AddAsync(ActualRoute route, CancellationToken cancellationToken = default);
    void Update(ActualRoute route);
    void Remove(ActualRoute route);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
}
