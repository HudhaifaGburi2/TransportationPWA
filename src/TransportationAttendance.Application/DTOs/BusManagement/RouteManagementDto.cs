using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.BusManagement;

public record RouteManagementDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string District { get; init; } = string.Empty;
    public string MeetingPoint { get; init; } = string.Empty;
    public decimal? MeetingPointLatitude { get; init; }
    public decimal? MeetingPointLongitude { get; init; }
    public TimeSpan PickupTime { get; init; }
    public TimeSpan DropoffTime { get; init; }
    public int Capacity { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateRouteManagementDto
{
    [Required(ErrorMessage = "اسم المسار مطلوب")]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "رمز المسار مطلوب")]
    [MaxLength(20)]
    public string Code { get; init; } = string.Empty;

    [Required(ErrorMessage = "الحي مطلوب")]
    [MaxLength(200)]
    public string District { get; init; } = string.Empty;

    [Required(ErrorMessage = "نقطة التجمع مطلوبة")]
    [MaxLength(500)]
    public string MeetingPoint { get; init; } = string.Empty;

    public decimal? MeetingPointLatitude { get; init; }
    public decimal? MeetingPointLongitude { get; init; }

    [Required(ErrorMessage = "وقت الذهاب مطلوب")]
    public TimeSpan PickupTime { get; init; }

    [Required(ErrorMessage = "وقت العودة مطلوب")]
    public TimeSpan DropoffTime { get; init; }

    [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100")]
    public int Capacity { get; init; } = 30;
}

public record UpdateRouteManagementDto
{
    [Required(ErrorMessage = "اسم المسار مطلوب")]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "رمز المسار مطلوب")]
    [MaxLength(20)]
    public string Code { get; init; } = string.Empty;

    [Required(ErrorMessage = "الحي مطلوب")]
    [MaxLength(200)]
    public string District { get; init; } = string.Empty;

    [Required(ErrorMessage = "نقطة التجمع مطلوبة")]
    [MaxLength(500)]
    public string MeetingPoint { get; init; } = string.Empty;

    public decimal? MeetingPointLatitude { get; init; }
    public decimal? MeetingPointLongitude { get; init; }

    [Required(ErrorMessage = "وقت الذهاب مطلوب")]
    public TimeSpan PickupTime { get; init; }

    [Required(ErrorMessage = "وقت العودة مطلوب")]
    public TimeSpan DropoffTime { get; init; }

    [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100")]
    public int Capacity { get; init; } = 30;

    public bool IsActive { get; init; } = true;
}

public record RouteQueryDto
{
    public string? Search { get; init; }
    public bool? IsActive { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
