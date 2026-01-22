using Microsoft.Extensions.DependencyInjection;
using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Application.Interfaces.Services;
using TransportationAttendance.Application.Mappings;
using TransportationAttendance.Application.Services;

namespace TransportationAttendance.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDistrictService, DistrictService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<ILookupService, LookupService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IBusManagementService, BusManagementService>();
        services.AddScoped<IStudentService, StudentService>();

        return services;
    }
}
