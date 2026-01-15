using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class BusRepository : BaseRepository<Bus>, IBusRepository
{
    public BusRepository(TransportationDbContext context) : base(context)
    {
    }

    public override async Task<IReadOnlyList<Bus>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(b => b.Route)
            .Include(b => b.BusDistricts)
                .ThenInclude(bd => bd.District)
            .Include(b => b.StudentAssignments)
            .OrderBy(b => b.PeriodId)
            .ThenBy(b => b.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Bus>> GetByPeriodAsync(int periodId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(b => b.Route)
            .Include(b => b.BusDistricts)
                .ThenInclude(bd => bd.District)
            .Include(b => b.StudentAssignments)
            .Where(b => b.PeriodId == periodId && b.IsActive)
            .OrderBy(b => b.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Bus>> GetActiveBusesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(b => b.Route)
            .Include(b => b.BusDistricts)
                .ThenInclude(bd => bd.District)
            .Include(b => b.StudentAssignments)
            .Where(b => b.IsActive)
            .OrderBy(b => b.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<Bus?> GetByBusNumberAsync(string busNumber, int periodId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Route)
            .Include(b => b.BusDistricts)
                .ThenInclude(bd => bd.District)
            .FirstOrDefaultAsync(b => b.BusNumber == busNumber && b.PeriodId == periodId, cancellationToken);
    }

    public override async Task<Bus?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Route)
            .Include(b => b.BusDistricts)
                .ThenInclude(bd => bd.District)
            .Include(b => b.StudentAssignments)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Bus>> GetByDistrictAsync(Guid districtId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(b => b.Route)
            .Include(b => b.BusDistricts)
                .ThenInclude(bd => bd.District)
            .Include(b => b.StudentAssignments)
            .Where(b => b.IsActive && b.BusDistricts.Any(bd => bd.DistrictId == districtId))
            .OrderBy(b => b.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetStudentCountAsync(Guid busId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<StudentBusAssignment>()
            .CountAsync(s => s.BusId == busId && s.IsActive, cancellationToken);
    }
}
