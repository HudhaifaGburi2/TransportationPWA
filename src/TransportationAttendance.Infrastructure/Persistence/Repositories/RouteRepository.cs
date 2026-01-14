using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class RouteRepository : BaseRepository<Route>, IRouteRepository
{
    public RouteRepository(TransportationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Route>> GetActiveRoutesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(r => r.IsActive)
            .OrderBy(r => r.RouteName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Route?> GetByNameAsync(string routeName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.RouteName == routeName, cancellationToken);
    }

    public async Task<Route?> GetWithBusesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}
