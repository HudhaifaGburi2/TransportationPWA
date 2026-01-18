using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class BusRepository : IBusRepository
{
    private readonly TransportationDbContext _context;

    public BusRepository(TransportationDbContext context)
    {
        _context = context;
    }

    public async Task<Bus?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Buses
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Bus>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Buses
            .AsNoTracking()
            .OrderBy(b => b.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<Bus?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default)
    {
        return await _context.Buses
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.LicensePlate == licensePlate, cancellationToken);
    }

    public async Task<Bus?> GetByBusNumberAsync(string busNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Buses
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.BusNumber == busNumber, cancellationToken);
    }

    public async Task<IReadOnlyList<Bus>> GetActiveBusesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Buses
            .AsNoTracking()
            .Where(b => b.IsActive)
            .OrderBy(b => b.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Bus>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Buses
            .AsNoTracking()
            .Where(b => b.BusNumber.Contains(searchTerm) || b.LicensePlate.Contains(searchTerm))
            .OrderBy(b => b.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Bus bus, CancellationToken cancellationToken = default)
    {
        await _context.Buses.AddAsync(bus, cancellationToken);
    }

    public void Update(Bus bus)
    {
        _context.Buses.Update(bus);
    }

    public void Remove(Bus bus)
    {
        _context.Buses.Remove(bus);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Buses.CountAsync(cancellationToken);
    }

    public async Task<int> GetActiveCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Buses.CountAsync(b => b.IsActive, cancellationToken);
    }

    public async Task<int> GetTotalCapacityAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Buses
            .Where(b => b.IsActive)
            .SumAsync(b => b.Capacity, cancellationToken);
    }
}
