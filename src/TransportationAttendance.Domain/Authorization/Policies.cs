namespace TransportationAttendance.Domain.Authorization;

public static class Policies
{
    // Attendance Policies
    public const string CanMarkAttendance = "CanMarkAttendance";
    public const string CanViewAttendance = "CanViewAttendance";
    public const string CanEditAttendance = "CanEditAttendance";

    // Route Management Policies
    public const string CanViewRoutes = "CanViewRoutes";
    public const string CanManageRoutes = "CanManageRoutes";

    // Registration Policies
    public const string CanSubmitRegistration = "CanSubmitRegistration";
    public const string CanViewOwnRegistration = "CanViewOwnRegistration";
    public const string CanViewRegistrations = "CanViewRegistrations";
    public const string CanApproveRegistrations = "CanApproveRegistrations";

    // Student Management Policies
    public const string CanViewStudents = "CanViewStudents";
    public const string CanManageStudents = "CanManageStudents";

    // Bus Management Policies
    public const string CanViewBuses = "CanViewBuses";
    public const string CanManageBuses = "CanManageBuses";

    // District Management Policies
    public const string CanViewDistricts = "CanViewDistricts";
    public const string CanManageDistricts = "CanManageDistricts";

    // Location Management Policies
    public const string CanViewLocations = "CanViewLocations";
    public const string CanManageLocations = "CanManageLocations";

    // Reports
    public const string CanViewDashboard = "CanViewDashboard";
    public const string CanGenerateReports = "CanGenerateReports";

    // System Admin
    public const string CanAccessSettings = "CanAccessSettings";
    public const string CanViewAuditLogs = "CanViewAuditLogs";
}
