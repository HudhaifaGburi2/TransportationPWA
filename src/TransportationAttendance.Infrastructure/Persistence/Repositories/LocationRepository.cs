using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class LocationRepository : BaseRepository<Location>, ILocationRepository
{
    public LocationRepository(TransportationDbContext context) : base(context)
    {
    }

    public async Task<Location?> GetByCodeAsync(string locationCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.LocationCode == locationCode, cancellationToken);
    }

    public async Task<IReadOnlyList<Location>> GetActiveLocationsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(l => l.IsActive)
            .OrderBy(l => l.LocationCode)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string locationCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(l => l.LocationCode == locationCode, cancellationToken);
    }
}
