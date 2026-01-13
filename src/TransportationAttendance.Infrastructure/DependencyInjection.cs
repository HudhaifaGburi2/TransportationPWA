using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Domain.Interfaces;
using TransportationAttendance.Infrastructure.Identity;
using TransportationAttendance.Infrastructure.Persistence;
using TransportationAttendance.Infrastructure.Persistence.Repositories;

namespace TransportationAttendance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Contexts
        services.AddDbContext<TransportationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("TumsDb"),
                b => b.MigrationsAssembly(typeof(TransportationDbContext).Assembly.FullName)));

        services.AddDbContext<CentralStudentDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CentralStudentDb")));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Central DB Repository
        services.AddScoped<ICentralDbRepository, CentralDbRepository>();

        // JWT Settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // LoginGateway Settings
        var loginGatewaySettings = configuration.GetSection("LoginGateway").Get<LoginGatewaySettings>() ?? new LoginGatewaySettings();
        services.Configure<LoginGatewaySettings>(configuration.GetSection("LoginGateway"));

        // LoginGateway HttpClient
        services.AddHttpClient<ILoginGatewayService, LoginGatewayService>(client =>
        {
            if (!string.IsNullOrEmpty(loginGatewaySettings.BaseUrl))
            {
                client.BaseAddress = new Uri(loginGatewaySettings.BaseUrl);
            }
            if (!string.IsNullOrEmpty(loginGatewaySettings.BearerToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {loginGatewaySettings.BearerToken}");
            }
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(loginGatewaySettings.TimeoutSeconds > 0 ? loginGatewaySettings.TimeoutSeconds : 30);
        });

        // JWT Services
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
