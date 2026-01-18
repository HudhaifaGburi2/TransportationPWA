using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.BusManagement;

public record RouteManagementDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateRouteManagementDto
{
    [Required(ErrorMessage = "اسم المسار مطلوب")]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}

public record UpdateRouteManagementDto
{
    [Required(ErrorMessage = "اسم المسار مطلوب")]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; } = true;
}

public record RouteQueryDto
{
    public string? Search { get; init; }
    public bool? IsActive { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
