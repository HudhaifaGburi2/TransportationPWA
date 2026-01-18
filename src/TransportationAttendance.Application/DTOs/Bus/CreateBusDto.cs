using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.Bus;

public record CreateBusDto
{
    /// <summary>
    /// PlateNumber is the authoritative identifier from official CSV.
    /// Unique within (PlateNumber + PeriodId) - same plate can exist in different periods.
    /// </summary>
    [Required(ErrorMessage = "رقم اللوحة مطلوب")]
    [MaxLength(20)]
    public string PlateNumber { get; init; } = string.Empty;

    [MaxLength(10)]
    public string? BusNumber { get; init; }

    [Required(ErrorMessage = "الفترة مطلوبة")]
    public int PeriodId { get; init; }

    public Guid? RouteId { get; init; }
    public Guid? DriverId { get; init; }
    public Guid? DepartmentId { get; init; }

    [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100")]
    public int Capacity { get; init; } = 30;

    public List<Guid> DistrictIds { get; init; } = new();
}

public record UpdateBusDto
{
    /// <summary>
    /// PlateNumber is the authoritative identifier from official CSV.
    /// Unique within (PlateNumber + PeriodId) - same plate can exist in different periods.
    /// </summary>
    [Required(ErrorMessage = "رقم اللوحة مطلوب")]
    [MaxLength(20)]
    public string PlateNumber { get; init; } = string.Empty;

    [MaxLength(10)]
    public string? BusNumber { get; init; }

    [Required(ErrorMessage = "الفترة مطلوبة")]
    public int PeriodId { get; init; }

    public Guid? RouteId { get; init; }
    public Guid? DriverId { get; init; }
    public Guid? DepartmentId { get; init; }

    [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100")]
    public int Capacity { get; init; } = 30;

    public List<Guid> DistrictIds { get; init; } = new();

    public bool IsActive { get; init; } = true;
}

public record BusQueryDto
{
    public int? PeriodId { get; init; }
    public Guid? RouteId { get; init; }
    public Guid? DistrictId { get; init; }
    public bool? IsActive { get; init; }
    public string? Search { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
