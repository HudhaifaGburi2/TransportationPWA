using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class ActualRouteRepository : IActualRouteRepository
{
    private readonly TransportationDbContext _context;

    public ActualRouteRepository(TransportationDbContext context)
    {
        _context = context;
    }

    public async Task<ActualRoute?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ActualRoutes
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ActualRoute>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActualRoutes
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<ActualRoute?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.ActualRoutes
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Code == code, cancellationToken);
    }

    public async Task<ActualRoute?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.ActualRoutes
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyList<ActualRoute>> GetActiveRoutesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActualRoutes
            .AsNoTracking()
            .Where(r => r.Status == 1)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ActualRoute>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.ActualRoutes
            .AsNoTracking()
            .Where(r => r.Name.Contains(searchTerm) || r.Code.Contains(searchTerm) || r.District.Contains(searchTerm))
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ActualRoute route, CancellationToken cancellationToken = default)
    {
        await _context.ActualRoutes.AddAsync(route, cancellationToken);
    }

    public void Update(ActualRoute route)
    {
        _context.ActualRoutes.Update(route);
    }

    public void Remove(ActualRoute route)
    {
        _context.ActualRoutes.Remove(route);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ActualRoutes.CountAsync(cancellationToken);
    }
}
