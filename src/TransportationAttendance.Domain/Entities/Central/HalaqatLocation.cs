namespace TransportationAttendance.Domain.Entities.Central;

public class HalaqatLocation
{
    public int LocationId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Gender { get; init; }
}
