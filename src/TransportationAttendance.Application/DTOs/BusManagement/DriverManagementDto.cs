using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.BusManagement;

public record DriverManagementDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateDriverManagementDto
{
    [Required(ErrorMessage = "اسم السائق مطلوب")]
    [MaxLength(200)]
    public string FullName { get; init; } = string.Empty;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [MaxLength(20)]
    public string PhoneNumber { get; init; } = string.Empty;
}

public record UpdateDriverManagementDto
{
    [Required(ErrorMessage = "اسم السائق مطلوب")]
    [MaxLength(200)]
    public string FullName { get; init; } = string.Empty;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [MaxLength(20)]
    public string PhoneNumber { get; init; } = string.Empty;

    public bool IsActive { get; init; } = true;
}

public record DriverQueryDto
{
    public string? Search { get; init; }
    public bool? IsActive { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
