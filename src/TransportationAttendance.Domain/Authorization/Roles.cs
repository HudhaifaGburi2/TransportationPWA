namespace TransportationAttendance.Domain.Authorization;

public static class Roles
{
    // Legacy roles
    public const string Student = "Student";
    public const string Staff = "Staff";
    public const string Driver = "Driver";
    public const string Supervisor = "Supervisor";
    public const string Admin = "Admin";
    public const string SuperAdmin = "SuperAdmin";
    public const string SystemAdministrator = "SYSTEM_ADMINISTRATOR";
    
    // TUMS-specific roles (from CentralDB Regt_User_Role)
    public const string AdminTums = "Admin_Tums";
    public const string StuffTums = "Stuff_Tums";
    public const string DriverTums = "Driver_Tums";
    public const string StudentRole = "STUDENT";

    // Role groups for authorization
    public static readonly string[] AdminRoles = { Admin, SuperAdmin, SystemAdministrator, AdminTums };
    public static readonly string[] StaffRoles = { Staff, Supervisor, Admin, SuperAdmin, SystemAdministrator, AdminTums, StuffTums };
    public static readonly string[] DriverRoles = { Driver, Supervisor, Admin, SuperAdmin, SystemAdministrator, AdminTums, DriverTums };
    public static readonly string[] SupervisorAndAbove = { Supervisor, Admin, SuperAdmin, SystemAdministrator, AdminTums };
    public static readonly string[] SuperAdminOnly = { SuperAdmin, SystemAdministrator };
    
    // TUMS-specific role groups
    public static readonly string[] TumsAdminRoles = { AdminTums, SystemAdministrator };
    public static readonly string[] TumsStaffRoles = { AdminTums, StuffTums, SystemAdministrator };
    public static readonly string[] TumsDriverRoles = { AdminTums, DriverTums, SystemAdministrator };
    public static readonly string[] TumsStudentRoles = { StudentRole };
    
    public static readonly string[] AllRoles = { Student, Staff, Driver, Supervisor, Admin, SuperAdmin, SystemAdministrator, AdminTums, StuffTums, DriverTums, StudentRole };
}
