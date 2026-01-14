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
    
    // TUMS-specific policies
    public const string TumsAdminPolicy = "TumsAdminPolicy";
    public const string TumsStaffPolicy = "TumsStaffPolicy";
    public const string TumsDriverPolicy = "TumsDriverPolicy";
    public const string TumsStudentPolicy = "TumsStudentPolicy";

    public static void ConfigurePolicies(AuthorizationOptions options)
    {
        // Admin Only Policy (includes TUMS Admin)
        options.AddPolicy(AdminPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.SystemAdministrator, Roles.SuperAdmin, Roles.Admin, Roles.AdminTums);
        });

        // Staff Policy - Admin and Staff roles (includes TUMS Staff)
        options.AddPolicy(StaffPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.SystemAdministrator, Roles.SuperAdmin, Roles.Admin, Roles.Staff, Roles.AdminTums, Roles.StuffTums);
        });

        // Student Only Policy (includes TUMS Student and common variations)
        options.AddPolicy(StudentPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.Student, Roles.StudentRole, "student", "طالب", "Student_Tums");
        });

        // Driver Only Policy (includes TUMS Driver)
        options.AddPolicy(DriverPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.Driver, Roles.DriverTums);
        });

        // Supervisor Policy
        options.AddPolicy(SupervisorPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.Supervisor, Roles.SystemAdministrator, Roles.SuperAdmin, Roles.AdminTums);
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
                Roles.Driver,
                Roles.AdminTums,
                Roles.StuffTums,
                Roles.DriverTums);
        });

        // Transportation Write Policy - Only specific roles can write
        options.AddPolicy(TransportationWritePolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.SystemAdministrator, Roles.SuperAdmin, Roles.Admin, Roles.AdminTums);
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
                Roles.Driver,
                Roles.AdminTums,
                Roles.StuffTums,
                Roles.DriverTums);
        });
        
        // TUMS Admin Policy - Only Admin_Tums and SYSTEM_ADMINISTRATOR
        options.AddPolicy(TumsAdminPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.AdminTums, Roles.SystemAdministrator);
        });
        
        // TUMS Staff Policy - Admin_Tums, Stuff_Tums, SYSTEM_ADMINISTRATOR
        options.AddPolicy(TumsStaffPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.AdminTums, Roles.StuffTums, Roles.SystemAdministrator);
        });
        
        // TUMS Driver Policy
        options.AddPolicy(TumsDriverPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.AdminTums, Roles.DriverTums, Roles.SystemAdministrator);
        });
        
        // TUMS Student Policy
        options.AddPolicy(TumsStudentPolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Roles.Student, Roles.StudentRole, "student", "طالب", "Student_Tums");
        });
    }
}
