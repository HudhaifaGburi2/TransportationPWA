using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // JWT Services
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IIdentityService, IdentityService>();

        // Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers["Token-Expired"] = "true";
                    }
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}
