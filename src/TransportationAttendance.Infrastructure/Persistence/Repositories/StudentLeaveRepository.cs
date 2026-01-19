using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class StudentLeaveRepository : BaseRepository<StudentLeave>, IStudentLeaveRepository
{
    public StudentLeaveRepository(TransportationDbContext context) : base(context)
    {
    }

    public async Task<StudentLeave?> GetActiveByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow.Date;
        return await _dbSet
            .Include(l => l.Student)
            .FirstOrDefaultAsync(l => 
                l.StudentId == studentId && 
                l.IsApproved && 
                !l.IsCancelled && 
                l.StartDate.Date <= now && 
                l.EndDate.Date >= now, 
                cancellationToken);
    }

    public async Task<IReadOnlyList<StudentLeave>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(l => l.StudentId == studentId)
            .OrderByDescending(l => l.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentLeave>> GetPendingLeavesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(l => l.Student)
            .Where(l => !l.IsApproved && !l.IsCancelled)
            .OrderBy(l => l.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentLeave>> GetActiveLeavesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow.Date;
        return await _dbSet
            .AsNoTracking()
            .Include(l => l.Student)
            .Where(l => l.IsApproved && !l.IsCancelled && l.StartDate.Date <= now && l.EndDate.Date >= now)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentLeave>> GetLeavesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(l => l.Student)
            .Where(l => l.StartDate.Date <= endDate.Date && l.EndDate.Date >= startDate.Date)
            .OrderBy(l => l.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasOverlappingLeaveAsync(Guid studentId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(l => 
            l.StudentId == studentId && 
            !l.IsCancelled &&
            l.StartDate.Date <= endDate.Date && 
            l.EndDate.Date >= startDate.Date, 
            cancellationToken);
    }
}
