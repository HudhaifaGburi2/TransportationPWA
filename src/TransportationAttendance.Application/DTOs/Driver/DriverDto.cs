namespace TransportationAttendance.Application.DTOs.Driver;

public class DriverDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public bool IsActive { get; set; }
}

public class CreateDriverDto
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
}

public class UpdateDriverDto
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Guid? DepartmentId { get; set; }
    public bool? IsActive { get; set; }
}
