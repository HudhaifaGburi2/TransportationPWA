namespace TransportationAttendance.Application.DTOs.Location;

public record LocationDto
{
    public Guid Id { get; init; }
    public string LocationCode { get; init; } = string.Empty;
    public string LocationName { get; init; } = string.Empty;
    public string? LocationType { get; init; }
    public bool IsActive { get; init; }
}
