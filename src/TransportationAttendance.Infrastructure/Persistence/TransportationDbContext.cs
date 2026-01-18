using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence;

public class TransportationDbContext : DbContext
{
    public TransportationDbContext(DbContextOptions<TransportationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<District> Districts => Set<District>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<Bus> Buses => Set<Bus>();
    public DbSet<BusDistrict> BusDistricts => Set<BusDistrict>();
    public DbSet<StudentBusAssignment> StudentBusAssignments => Set<StudentBusAssignment>();
    public DbSet<AttendanceSession> AttendanceSessions => Set<AttendanceSession>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
    public DbSet<RegistrationRequest> RegistrationRequests => Set<RegistrationRequest>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<Department> Departments => Set<Department>();
    
    // Actual schema entities (matching existing database)
    public DbSet<ActualDriver> ActualDrivers => Set<ActualDriver>();
    public DbSet<ActualRoute> ActualRoutes => Set<ActualRoute>();
    public DbSet<ActualBus> ActualBuses => Set<ActualBus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransportationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.SetCreated(DateTime.UtcNow, null);
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.SetUpdated(DateTime.UtcNow, null);
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
