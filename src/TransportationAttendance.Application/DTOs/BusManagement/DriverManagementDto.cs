using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.BusManagement;

public record DriverManagementDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string LicenseNumber { get; init; } = string.Empty;
    public DateTime LicenseExpiryDate { get; init; }
    public string? EmployeeId { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool IsLicenseExpired => LicenseExpiryDate < DateTime.UtcNow;
    public int DaysUntilLicenseExpiry => (int)(LicenseExpiryDate - DateTime.UtcNow).TotalDays;
}

public record CreateDriverManagementDto
{
    [Required(ErrorMessage = "اسم السائق مطلوب")]
    [MaxLength(200)]
    public string FullName { get; init; } = string.Empty;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [MaxLength(20)]
    public string PhoneNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "رقم الرخصة مطلوب")]
    [MaxLength(50)]
    public string LicenseNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "تاريخ انتهاء الرخصة مطلوب")]
    public DateTime LicenseExpiryDate { get; init; }

    [MaxLength(50)]
    public string? EmployeeId { get; init; }
}

public record UpdateDriverManagementDto
{
    [Required(ErrorMessage = "اسم السائق مطلوب")]
    [MaxLength(200)]
    public string FullName { get; init; } = string.Empty;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [MaxLength(20)]
    public string PhoneNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "رقم الرخصة مطلوب")]
    [MaxLength(50)]
    public string LicenseNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "تاريخ انتهاء الرخصة مطلوب")]
    public DateTime LicenseExpiryDate { get; init; }

    [MaxLength(50)]
    public string? EmployeeId { get; init; }

    public bool IsActive { get; init; } = true;
}

public record DriverQueryDto
{
    public string? Search { get; init; }
    public bool? IsActive { get; init; }
    public bool? LicenseExpiringSoon { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
