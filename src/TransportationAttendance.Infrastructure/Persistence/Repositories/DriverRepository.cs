using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly TransportationDbContext _context;

    public DriverRepository(TransportationDbContext context)
    {
        _context = context;
    }

    public async Task<Driver?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Drivers
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Driver>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Drivers
            .AsNoTracking()
            .OrderBy(d => d.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Driver?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Drivers
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<Driver?> GetByLicenseNumberAsync(string licenseNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Drivers
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.LicenseNumber == licenseNumber, cancellationToken);
    }

    public async Task<IReadOnlyList<Driver>> GetActiveDriversAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Drivers
            .AsNoTracking()
            .Where(d => d.IsActive)
            .OrderBy(d => d.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Driver>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Drivers
            .AsNoTracking()
            .Where(d => d.FullName.Contains(searchTerm) || d.PhoneNumber.Contains(searchTerm))
            .OrderBy(d => d.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Driver driver, CancellationToken cancellationToken = default)
    {
        await _context.Drivers.AddAsync(driver, cancellationToken);
    }

    public void Update(Driver driver)
    {
        _context.Drivers.Update(driver);
    }

    public void Remove(Driver driver)
    {
        _context.Drivers.Remove(driver);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Drivers.CountAsync(cancellationToken);
    }
}
