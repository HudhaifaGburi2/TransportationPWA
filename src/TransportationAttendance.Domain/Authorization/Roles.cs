namespace TransportationAttendance.Domain.Authorization;

public static class Roles
{
    public const string Student = "Student";
    public const string Staff = "Staff";
    public const string Driver = "Driver";
    public const string Supervisor = "Supervisor";
    public const string Admin = "Admin";
    public const string SuperAdmin = "SuperAdmin";
    public const string SystemAdministrator = "SYSTEM_ADMINISTRATOR";

    public static readonly string[] AdminRoles = { Admin, SuperAdmin, SystemAdministrator };
    public static readonly string[] StaffRoles = { Staff, Supervisor, Admin, SuperAdmin, SystemAdministrator };
    public static readonly string[] DriverRoles = { Driver, Supervisor, Admin, SuperAdmin, SystemAdministrator };
    public static readonly string[] SupervisorAndAbove = { Supervisor, Admin, SuperAdmin, SystemAdministrator };
    public static readonly string[] SuperAdminOnly = { SuperAdmin, SystemAdministrator };
    public static readonly string[] AllRoles = { Student, Staff, Driver, Supervisor, Admin, SuperAdmin, SystemAdministrator };
}
