using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class StudentBusAssignmentRepository : BaseRepository<StudentBusAssignment>, IStudentBusAssignmentRepository
{
    public StudentBusAssignmentRepository(TransportationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<StudentBusAssignment>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(a => a.Bus)
            .Include(a => a.Student)
            .Where(a => a.StudentId == studentId)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentBusAssignment>> GetByBusIdAsync(Guid busId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(a => a.Bus)
            .Include(a => a.Student)
            .Where(a => a.BusId == busId)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<StudentBusAssignment?> GetActiveByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Bus)
            .Include(a => a.Student)
            .FirstOrDefaultAsync(a => a.StudentId == studentId && a.IsActive, cancellationToken);
    }

    public async Task<IReadOnlyList<StudentBusAssignment>> GetActiveByBusIdAsync(Guid busId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(a => a.Bus)
            .Include(a => a.Student)
            .Where(a => a.BusId == busId && a.IsActive)
            .OrderBy(a => a.Student!.StudentName)
            .ToListAsync(cancellationToken);
    }
}
