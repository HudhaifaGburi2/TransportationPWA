using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class ActualDriverRepository : IActualDriverRepository
{
    private readonly TransportationDbContext _context;

    public ActualDriverRepository(TransportationDbContext context)
    {
        _context = context;
    }

    public async Task<ActualDriver?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ActualDrivers
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ActualDriver>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActualDrivers
            .AsNoTracking()
            .OrderBy(d => d.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<ActualDriver?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await _context.ActualDrivers
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<ActualDriver?> GetByLicenseNumberAsync(string licenseNumber, CancellationToken cancellationToken = default)
    {
        return await _context.ActualDrivers
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.LicenseNumber == licenseNumber, cancellationToken);
    }

    public async Task<IReadOnlyList<ActualDriver>> GetActiveDriversAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActualDrivers
            .AsNoTracking()
            .Where(d => d.IsActive)
            .OrderBy(d => d.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ActualDriver>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.ActualDrivers
            .AsNoTracking()
            .Where(d => d.FullName.Contains(searchTerm) || d.PhoneNumber.Contains(searchTerm))
            .OrderBy(d => d.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ActualDriver driver, CancellationToken cancellationToken = default)
    {
        await _context.ActualDrivers.AddAsync(driver, cancellationToken);
    }

    public void Update(ActualDriver driver)
    {
        _context.ActualDrivers.Update(driver);
    }

    public void Remove(ActualDriver driver)
    {
        _context.ActualDrivers.Remove(driver);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActualDrivers.CountAsync(cancellationToken);
    }
}
