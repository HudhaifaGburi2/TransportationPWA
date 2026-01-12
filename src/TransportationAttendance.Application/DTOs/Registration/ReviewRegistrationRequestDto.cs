using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.Registration;

public record ReviewRegistrationRequestDto
{
    [Required(ErrorMessage = "Approved status is required")]
    public bool IsApproved { get; init; }

    [MaxLength(1000)]
    public string? Notes { get; init; }
}
