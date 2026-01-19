using System.ComponentModel.DataAnnotations;

namespace TransportationAttendance.Application.DTOs.Registration;

public record AssignBusRequestDto
{
    [Required(ErrorMessage = "Bus ID is required")]
    public Guid BusId { get; init; }

    [MaxLength(500)]
    public string? Notes { get; init; }
}
