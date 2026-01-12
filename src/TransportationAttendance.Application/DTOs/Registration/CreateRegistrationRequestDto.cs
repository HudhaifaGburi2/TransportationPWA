using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.Registration;

public record CreateRegistrationRequestDto
{
    [Required(ErrorMessage = "District is required")]
    public Guid DistrictId { get; init; }

    [Required(ErrorMessage = "Latitude is required")]
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    public decimal Latitude { get; init; }

    [Required(ErrorMessage = "Longitude is required")]
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    public decimal Longitude { get; init; }

    [MaxLength(500)]
    public string? HomeAddress { get; init; }
}
