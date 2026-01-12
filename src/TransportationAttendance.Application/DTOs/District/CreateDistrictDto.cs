using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.District;

public record CreateDistrictDto
{
    [Required(ErrorMessage = "District name in Arabic is required")]
    [MaxLength(200)]
    public string DistrictNameAr { get; init; } = string.Empty;

    [MaxLength(200)]
    public string? DistrictNameEn { get; init; }
}
