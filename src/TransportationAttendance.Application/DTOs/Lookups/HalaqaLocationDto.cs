namespace TransportationAttendance.Application.DTOs.Lookups;

public record HalaqaLocationDto
{
    public int LocationId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Gender { get; init; }
}
