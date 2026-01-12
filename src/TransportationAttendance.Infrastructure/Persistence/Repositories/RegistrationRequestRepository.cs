using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Enums;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class RegistrationRequestRepository : BaseRepository<RegistrationRequest>, IRegistrationRequestRepository
{
    public RegistrationRequestRepository(TransportationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<RegistrationRequest>> GetByStatusAsync(RegistrationStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(r => r.District)
            .Where(r => r.Status == status)
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RegistrationRequest>> GetByDistrictAsync(Guid districtId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(r => r.District)
            .Where(r => r.DistrictId == districtId)
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<RegistrationRequest?> GetByStudentUserIdAsync(int studentUserId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.District)
            .FirstOrDefaultAsync(r => r.StudentUserId == studentUserId, cancellationToken);
    }

    public async Task<IReadOnlyList<RegistrationRequest>> GetPendingRequestsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(r => r.District)
            .Where(r => r.Status == RegistrationStatus.Pending)
            .OrderBy(r => r.RequestedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasPendingRequestAsync(int studentUserId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(r => r.StudentUserId == studentUserId && r.Status == RegistrationStatus.Pending, cancellationToken);
    }

    public override async Task<RegistrationRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.District)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}
