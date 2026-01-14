using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.Registration;

public record CreateRegistrationRequestDto
{
    public Guid? DistrictId { get; init; }

    [Required(ErrorMessage = "National short address is required")]
    [RegularExpression(@"^[A-Za-z]{4}\d{4}$", ErrorMessage = "National short address must be 4 letters followed by 4 digits (e.g., ABCD1234)")]
    public string NationalShortAddress { get; init; } = string.Empty;

    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    public decimal? Latitude { get; init; }

    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    public decimal? Longitude { get; init; }

    [MaxLength(500)]
    public string? HomeAddress { get; init; }
}
