namespace TransportationAttendance.Application.DTOs.Lookups;

public record PeriodDto
{
    public int PeriodId { get; init; }
    public string PeriodName { get; init; } = string.Empty;
    public string? PeriodCode { get; init; }
    public TimeSpan? StartTime { get; init; }
    public TimeSpan? EndTime { get; init; }
}
