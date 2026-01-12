namespace TransportationAttendance.Application.DTOs.Lookups;

public record AgeGroupDto
{
    public int AgeGroupId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int? MinAge { get; init; }
    public int? MaxAge { get; init; }
}
