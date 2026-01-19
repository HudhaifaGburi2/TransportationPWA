using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.BusManagement;

public record BusManagementDto
{
    public Guid Id { get; init; }
    public string? BusNumber { get; init; }
    public int PeriodId { get; init; }
    public Guid? RouteId { get; init; }
    public string? DriverName { get; init; }
    public string? DriverPhoneNumber { get; init; }
    public int Capacity { get; init; }
    public bool IsActive { get; init; }
    public bool IsMerged { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateBusManagementDto
{
    [Required(ErrorMessage = "رقم الباص مطلوب")]
    [MaxLength(20)]
    public string BusNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "الفترة مطلوبة")]
    public int PeriodId { get; init; } = 1;

    public Guid? RouteId { get; init; }

    [MaxLength(200)]
    public string? DriverName { get; init; }

    [MaxLength(20)]
    public string? DriverPhoneNumber { get; init; }

    [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100")]
    public int Capacity { get; init; } = 30;
}

public record UpdateBusManagementDto
{
    [Required(ErrorMessage = "رقم الباص مطلوب")]
    [MaxLength(20)]
    public string BusNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "الفترة مطلوبة")]
    public int PeriodId { get; init; } = 1;

    public Guid? RouteId { get; init; }

    [MaxLength(200)]
    public string? DriverName { get; init; }

    [MaxLength(20)]
    public string? DriverPhoneNumber { get; init; }

    [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100")]
    public int Capacity { get; init; } = 30;

    public bool IsActive { get; init; } = true;
}

public record BusManagementQueryDto
{
    public string? Search { get; init; }
    public bool? IsActive { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

public record BusManagementStatisticsDto
{
    public int TotalBuses { get; init; }
    public int ActiveBuses { get; init; }
    public int InactiveBuses { get; init; }
    public int TotalCapacity { get; init; }
    public int TotalDrivers { get; init; }
    public int ActiveDrivers { get; init; }
    public int TotalRoutes { get; init; }
    public int ActiveRoutes { get; init; }
}
