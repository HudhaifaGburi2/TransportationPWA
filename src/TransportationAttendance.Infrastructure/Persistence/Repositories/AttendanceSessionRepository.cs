using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Enums;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class AttendanceSessionRepository : BaseRepository<AttendanceSession>, IAttendanceSessionRepository
{
    public AttendanceSessionRepository(TransportationDbContext context) : base(context)
    {
    }

    public async Task<AttendanceSession?> GetByBusAndDateAsync(Guid busId, DateOnly date, AttendanceType type, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Bus)
            .Include(s => s.Location)
            .Include(s => s.AttendanceRecords)
                .ThenInclude(r => r.Student)
            .FirstOrDefaultAsync(s => s.BusId == busId && s.AttendanceDate == date && s.AttendanceType == type, cancellationToken);
    }

    public async Task<IReadOnlyList<AttendanceSession>> GetByDateAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(s => s.Bus)
            .Include(s => s.Location)
            .Where(s => s.AttendanceDate == date)
            .OrderBy(s => s.PeriodId)
            .ThenBy(s => s.Bus!.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AttendanceSession>> GetBySupervisorAsync(Guid supervisorId, DateOnly date, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(s => s.Bus)
            .Include(s => s.Location)
            .Where(s => s.SupervisorId == supervisorId && s.AttendanceDate == date)
            .OrderBy(s => s.PeriodId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AttendanceSession>> GetPendingSyncAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(s => s.AttendanceRecords)
            .Where(s => s.SyncStatus == SyncStatus.Pending || s.SyncStatus == SyncStatus.Failed)
            .OrderBy(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<AttendanceSession?> GetWithRecordsAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Bus)
            .Include(s => s.Location)
            .Include(s => s.AttendanceRecords)
                .ThenInclude(r => r.Student)
            .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);
    }

    public override async Task<AttendanceSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Bus)
            .Include(s => s.Location)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}
