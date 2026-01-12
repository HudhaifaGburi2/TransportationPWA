using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.Location;

public record CreateLocationDto
{
    [Required(ErrorMessage = "Location code is required")]
    [MaxLength(20)]
    public string LocationCode { get; init; } = string.Empty;

    [Required(ErrorMessage = "Location name is required")]
    [MaxLength(100)]
    public string LocationName { get; init; } = string.Empty;

    [MaxLength(50)]
    public string? LocationType { get; init; }
}
