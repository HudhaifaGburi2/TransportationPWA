using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.BusManagement;

public record BusManagementDto
{
    public Guid Id { get; init; }
    public string BusNumber { get; init; } = string.Empty;
    public string LicensePlate { get; init; } = string.Empty;
    public int Capacity { get; init; }
    public string? Model { get; init; }
    public int? Year { get; init; }
    public bool IsActive { get; init; }
    public DateTime? LastMaintenanceDate { get; init; }
    public DateTime? NextMaintenanceDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool NeedsMaintenance => NextMaintenanceDate.HasValue && NextMaintenanceDate.Value < DateTime.UtcNow;
}

public record CreateBusManagementDto
{
    [Required(ErrorMessage = "رقم الباص مطلوب")]
    [MaxLength(20)]
    public string BusNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "رقم اللوحة مطلوب")]
    [MaxLength(20)]
    public string LicensePlate { get; init; } = string.Empty;

    [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100")]
    public int Capacity { get; init; } = 30;

    [MaxLength(100)]
    public string? Model { get; init; }

    public int? Year { get; init; }
}

public record UpdateBusManagementDto
{
    [Required(ErrorMessage = "رقم الباص مطلوب")]
    [MaxLength(20)]
    public string BusNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "رقم اللوحة مطلوب")]
    [MaxLength(20)]
    public string LicensePlate { get; init; } = string.Empty;

    [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100")]
    public int Capacity { get; init; } = 30;

    [MaxLength(100)]
    public string? Model { get; init; }

    public int? Year { get; init; }

    public bool IsActive { get; init; } = true;

    public DateTime? LastMaintenanceDate { get; init; }
    public DateTime? NextMaintenanceDate { get; init; }
}

public record BusManagementQueryDto
{
    public string? Search { get; init; }
    public bool? IsActive { get; init; }
    public bool? NeedsMaintenance { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

public record BusManagementStatisticsDto
{
    public int TotalBuses { get; init; }
    public int ActiveBuses { get; init; }
    public int InactiveBuses { get; init; }
    public int TotalCapacity { get; init; }
    public int BusesNeedingMaintenance { get; init; }
    public int TotalDrivers { get; init; }
    public int ActiveDrivers { get; init; }
    public int DriversWithExpiringLicense { get; init; }
    public int TotalRoutes { get; init; }
    public int ActiveRoutes { get; init; }
}
