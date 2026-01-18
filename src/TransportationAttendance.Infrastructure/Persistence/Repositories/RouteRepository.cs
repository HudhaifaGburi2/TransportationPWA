using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class RouteRepository : IRouteRepository
{
    private readonly TransportationDbContext _context;

    public RouteRepository(TransportationDbContext context)
    {
        _context = context;
    }

    public async Task<Route?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Routes
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Route>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Routes
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Route?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Routes
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyList<Route>> GetActiveRoutesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Routes
            .AsNoTracking()
            .Where(r => r.IsActive)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Route>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Routes
            .AsNoTracking()
            .Where(r => r.Name.Contains(searchTerm) || 
                       (r.Description != null && r.Description.Contains(searchTerm)))
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Route route, CancellationToken cancellationToken = default)
    {
        await _context.Routes.AddAsync(route, cancellationToken);
    }

    public void Update(Route route)
    {
        _context.Routes.Update(route);
    }

    public void Remove(Route route)
    {
        _context.Routes.Remove(route);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Routes.CountAsync(cancellationToken);
    }
}
