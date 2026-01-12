using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Enums;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class StudentRepository : BaseRepository<Student>, IStudentRepository
{
    public StudentRepository(TransportationDbContext context) : base(context)
    {
    }

    public async Task<Student?> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.District)
            .FirstOrDefaultAsync(s => s.StudentId == studentId, cancellationToken);
    }

    public async Task<Student?> GetByUserIdAsync(int studentUserId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.District)
            .FirstOrDefaultAsync(s => s.StudentUserId == studentUserId, cancellationToken);
    }

    public async Task<IReadOnlyList<Student>> GetByDistrictAsync(Guid districtId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(s => s.District)
            .Where(s => s.DistrictId == districtId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Student>> GetActiveStudentsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(s => s.District)
            .Where(s => s.Status == StudentStatus.Active)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByStudentIdAsync(string studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(s => s.StudentId == studentId, cancellationToken);
    }

    public async Task<bool> ExistsByUserIdAsync(int studentUserId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(s => s.StudentUserId == studentUserId, cancellationToken);
    }

    public override async Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.District)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}
