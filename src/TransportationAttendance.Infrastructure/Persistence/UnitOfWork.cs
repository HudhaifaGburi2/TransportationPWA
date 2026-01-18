using Microsoft.EntityFrameworkCore.Storage;
using TransportationAttendance.Domain.Interfaces;
using TransportationAttendance.Domain.Interfaces.Repositories;
using TransportationAttendance.Infrastructure.Persistence.Repositories;

namespace TransportationAttendance.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly TransportationDbContext _context;
    private IDbContextTransaction? _transaction;

    private IStudentRepository? _students;
    private IDistrictRepository? _districts;
    private ILocationRepository? _locations;
    private IRegistrationRequestRepository? _registrationRequests;
    private IBusRepository? _buses;
    private IRouteRepository? _routes;
    private IDriverRepository? _drivers;
    private IAttendanceSessionRepository? _attendanceSessions;
    private IAuditLogRepository? _auditLogs;

    public UnitOfWork(TransportationDbContext context)
    {
        _context = context;
    }

    public IStudentRepository Students => _students ??= new StudentRepository(_context);
    public IDistrictRepository Districts => _districts ??= new DistrictRepository(_context);
    public ILocationRepository Locations => _locations ??= new LocationRepository(_context);
    public IRegistrationRequestRepository RegistrationRequests => _registrationRequests ??= new RegistrationRequestRepository(_context);
    public IBusRepository Buses => _buses ??= new BusRepository(_context);
    public IRouteRepository Routes => _routes ??= new RouteRepository(_context);
    public IDriverRepository Drivers => _drivers ??= new DriverRepository(_context);
    public IAttendanceSessionRepository AttendanceSessions => _attendanceSessions ??= new AttendanceSessionRepository(_context);
    public IAuditLogRepository AuditLogs => _auditLogs ??= new AuditLogRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
        await _context.DisposeAsync();
    }
}
