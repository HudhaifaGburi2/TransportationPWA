using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class BusRepository : BaseRepository<Bus>, IBusRepository
{
    public BusRepository(TransportationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Bus>> GetByPeriodAsync(int periodId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(b => b.Route)
            .Where(b => b.PeriodId == periodId && b.IsActive)
            .OrderBy(b => b.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Bus>> GetActiveBusesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(b => b.Route)
            .Where(b => b.IsActive)
            .OrderBy(b => b.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<Bus?> GetByBusNumberAsync(string busNumber, int periodId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Route)
            .FirstOrDefaultAsync(b => b.BusNumber == busNumber && b.PeriodId == periodId, cancellationToken);
    }

    public override async Task<Bus?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Route)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }
}
