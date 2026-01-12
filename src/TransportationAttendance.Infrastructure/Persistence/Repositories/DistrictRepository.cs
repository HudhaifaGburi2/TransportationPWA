using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class DistrictRepository : BaseRepository<District>, IDistrictRepository
{
    public DistrictRepository(TransportationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<District>> GetActiveDistrictsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(d => d.IsActive)
            .OrderBy(d => d.DistrictNameAr)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string districtNameAr, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(d => d.DistrictNameAr == districtNameAr, cancellationToken);
    }
}
