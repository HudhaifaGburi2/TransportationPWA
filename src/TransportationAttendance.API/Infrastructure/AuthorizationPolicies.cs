using Microsoft.AspNetCore.Authorization;
using TransportationAttendance.Domain.Authorization;

namespace TransportationAttendance.API.Infrastructure;

public static class AuthorizationPolicies
{
    public const string AdminPolicy = "AdminPolicy";
    public const string StaffPolicy = "StaffPolicy";
    public const string StudentPolicy = "StudentPolicy";
    public const string DriverPolicy = "DriverPolicy";
    public const string SupervisorPolicy = "SupervisorPolicy";
    public const string TransportationReadPolicy = "TransportationReadPolicy";
    public const string TransportationWritePolicy = "TransportationWritePolicy";
    public const string AttendanceManagePolicy = "AttendanceManagePolicy";

    public static void ConfigurePolicies(AuthorizationOptions options)
    {
        // Admin Only Policy
        options.AddPolicy(AdminPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.SystemAdministrator, Roles.SuperAdmin, Roles.Admin);
        });

        // Staff Policy - Admin and Staff roles
        options.AddPolicy(StaffPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.SystemAdministrator, Roles.SuperAdmin, Roles.Admin, Roles.Staff);
        });

        // Student Only Policy
        options.AddPolicy(StudentPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.Student);
        });

        // Driver Only Policy
        options.AddPolicy(DriverPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.Driver);
        });

        // Supervisor Policy
        options.AddPolicy(SupervisorPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.Supervisor, Roles.SystemAdministrator, Roles.SuperAdmin);
        });

        // Transportation Read Policy - Multiple roles can read
        options.AddPolicy(TransportationReadPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(
                Roles.SystemAdministrator, 
                Roles.SuperAdmin, 
                Roles.Admin, 
                Roles.Staff,
                Roles.Supervisor,
                Roles.Driver);
        });

        // Transportation Write Policy - Only specific roles can write
        options.AddPolicy(TransportationWritePolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.SystemAdministrator, Roles.SuperAdmin, Roles.Admin);
        });

        // Attendance Management Policy
        options.AddPolicy(AttendanceManagePolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(
                Roles.SystemAdministrator, 
                Roles.SuperAdmin, 
                Roles.Admin, 
                Roles.Staff,
                Roles.Supervisor,
                Roles.Driver);
        });
    }
}
