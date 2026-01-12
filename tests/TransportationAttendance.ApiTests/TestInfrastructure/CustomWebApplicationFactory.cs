using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransportationAttendance.Infrastructure.Persistence;

namespace TransportationAttendance.ApiTests.TestInfrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = $"TumsDb_Test_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove all DbContext-related registrations
            var descriptorsToRemove = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<TransportationDbContext>) ||
                           d.ServiceType == typeof(DbContextOptions<CentralStudentDbContext>) ||
                           d.ServiceType.FullName?.Contains("DbContextOptions") == true)
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database for TumsDb
            services.AddDbContext<TransportationDbContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });

            // Add in-memory database for CentralStudentDb  
            services.AddDbContext<CentralStudentDbContext>(options =>
            {
                options.UseInMemoryDatabase($"CentralDb_{_dbName}");
            });
        });
    }

    public void EnsureDatabaseSeeded()
    {
        using var scope = Services.CreateScope();
        var tumsDb = scope.ServiceProvider.GetRequiredService<TransportationDbContext>();
        tumsDb.Database.EnsureCreated();
        SeedTestData(tumsDb);
    }

    private static void SeedTestData(TransportationDbContext tumsDb)
    {
        // Seed test districts
        if (!tumsDb.Districts.Any())
        {
            tumsDb.Districts.AddRange(
                Domain.Entities.District.Create("حي الملك فهد", "King Fahd District"),
                Domain.Entities.District.Create("حي النسيم", "Al Naseem District"),
                Domain.Entities.District.Create("حي الروضة", "Al Rawdah District")
            );
            tumsDb.SaveChanges();
        }

        // Seed test locations
        if (!tumsDb.Locations.Any())
        {
            tumsDb.Locations.AddRange(
                Domain.Entities.Location.Create("A8", "موقف A8", "Parking"),
                Domain.Entities.Location.Create("A9", "موقف A9", "Parking"),
                Domain.Entities.Location.Create("B8", "موقف B8", "Parking")
            );
            tumsDb.SaveChanges();
        }

        // Seed test buses
        if (!tumsDb.Buses.Any())
        {
            tumsDb.Buses.AddRange(
                Domain.Entities.Bus.Create("101", 1, 40),
                Domain.Entities.Bus.Create("102", 1, 40),
                Domain.Entities.Bus.Create("201", 2, 35)
            );
            tumsDb.SaveChanges();
        }
    }
}
