using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.Bus;

public record CreateBusDto
{
    [Required(ErrorMessage = "رقم الباص مطلوب")]
    [MaxLength(20)]
    public string BusNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "الفترة مطلوبة")]
    public int PeriodId { get; init; }

    public Guid? RouteId { get; init; }

    [MaxLength(200)]
    public string? DriverName { get; init; }

    [MaxLength(20)]
    public string? DriverPhoneNumber { get; init; }

    [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100")]
    public int Capacity { get; init; } = 30;

    public List<Guid> DistrictIds { get; init; } = new();
}

public record UpdateBusDto
{
    [Required(ErrorMessage = "رقم الباص مطلوب")]
    [MaxLength(20)]
    public string BusNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "الفترة مطلوبة")]
    public int PeriodId { get; init; }

    public Guid? RouteId { get; init; }

    [MaxLength(200)]
    public string? DriverName { get; init; }

    [MaxLength(20)]
    public string? DriverPhoneNumber { get; init; }

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
