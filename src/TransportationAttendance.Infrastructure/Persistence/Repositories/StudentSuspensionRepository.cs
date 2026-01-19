using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class StudentSuspensionRepository : BaseRepository<StudentSuspension>, IStudentSuspensionRepository
{
    public StudentSuspensionRepository(TransportationDbContext context) : base(context)
    {
    }

    public async Task<StudentSuspension?> GetActiveByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Student)
            .Include(s => s.Bus)
            .FirstOrDefaultAsync(s => s.StudentId == studentId && !s.IsReactivated, cancellationToken);
    }

    public async Task<IReadOnlyList<StudentSuspension>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(s => s.Bus)
            .Where(s => s.StudentId == studentId)
            .OrderByDescending(s => s.SuspendedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentSuspension>> GetActiveSuspensionsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(s => s.Student)
            .Include(s => s.Bus)
            .Where(s => !s.IsReactivated)
            .OrderByDescending(s => s.SuspendedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasActiveSuspensionAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(s => s.StudentId == studentId && !s.IsReactivated, cancellationToken);
    }
}
