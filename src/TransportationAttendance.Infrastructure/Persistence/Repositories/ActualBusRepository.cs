using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class ActualBusRepository : IActualBusRepository
{
    private readonly TransportationDbContext _context;

    public ActualBusRepository(TransportationDbContext context)
    {
        _context = context;
    }

    public async Task<ActualBus?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ActualBuses
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ActualBus>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActualBuses
            .AsNoTracking()
            .OrderBy(b => b.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<ActualBus?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default)
    {
        return await _context.ActualBuses
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.LicensePlate == licensePlate, cancellationToken);
    }

    public async Task<ActualBus?> GetByBusNumberAsync(string busNumber, CancellationToken cancellationToken = default)
    {
        return await _context.ActualBuses
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.BusNumber == busNumber, cancellationToken);
    }

    public async Task<IReadOnlyList<ActualBus>> GetActiveBusesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActualBuses
            .AsNoTracking()
            .Where(b => b.Status == 1)
            .OrderBy(b => b.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ActualBus>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.ActualBuses
            .AsNoTracking()
            .Where(b => b.BusNumber.Contains(searchTerm) || b.LicensePlate.Contains(searchTerm))
            .OrderBy(b => b.BusNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ActualBus bus, CancellationToken cancellationToken = default)
    {
        await _context.ActualBuses.AddAsync(bus, cancellationToken);
    }

    public void Update(ActualBus bus)
    {
        _context.ActualBuses.Update(bus);
    }

    public void Remove(ActualBus bus)
    {
        _context.ActualBuses.Remove(bus);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActualBuses.CountAsync(cancellationToken);
    }

    public async Task<int> GetActiveCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActualBuses.CountAsync(b => b.Status == 1, cancellationToken);
    }

    public async Task<int> GetTotalCapacityAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActualBuses.Where(b => b.Status == 1).SumAsync(b => b.Capacity, cancellationToken);
    }
}
