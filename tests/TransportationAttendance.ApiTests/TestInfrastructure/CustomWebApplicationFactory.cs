using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransportationAttendance.Infrastructure.Persistence;

namespace TransportationAttendance.ApiTests.TestInfrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registrations
            var descriptorTums = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<TransportationDbContext>));
            if (descriptorTums != null)
            {
                services.Remove(descriptorTums);
            }

            var descriptorCentral = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<CentralStudentDbContext>));
            if (descriptorCentral != null)
            {
                services.Remove(descriptorCentral);
            }

            // Add in-memory database for TumsDb
            services.AddDbContext<TransportationDbContext>(options =>
            {
                options.UseInMemoryDatabase("TumsDb_Test");
            });

            // Add in-memory database for CentralStudentDb
            services.AddDbContext<CentralStudentDbContext>(options =>
            {
                options.UseInMemoryDatabase("CentralStudentDb_Test");
            });

            // Build service provider and ensure database is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var tumsDb = scopedServices.GetRequiredService<TransportationDbContext>();
            tumsDb.Database.EnsureCreated();

            var centralDb = scopedServices.GetRequiredService<CentralStudentDbContext>();
            centralDb.Database.EnsureCreated();

            // Seed test data
            SeedTestData(tumsDb, centralDb);
        });

        builder.UseEnvironment("Testing");
    }

    private static void SeedTestData(TransportationDbContext tumsDb, CentralStudentDbContext centralDb)
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
