using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class StudentTransferRepository : BaseRepository<StudentTransfer>, IStudentTransferRepository
{
    public StudentTransferRepository(TransportationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<StudentTransfer>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(t => t.FromBus)
            .Include(t => t.ToBus)
            .Where(t => t.StudentId == studentId)
            .OrderByDescending(t => t.TransferredAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentTransfer>> GetByBusIdAsync(Guid busId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(t => t.Student)
            .Include(t => t.FromBus)
            .Include(t => t.ToBus)
            .Where(t => t.FromBusId == busId || t.ToBusId == busId)
            .OrderByDescending(t => t.TransferredAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StudentTransfer>> GetRecentTransfersAsync(int count = 50, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(t => t.Student)
            .Include(t => t.FromBus)
            .Include(t => t.ToBus)
            .OrderByDescending(t => t.TransferredAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}
