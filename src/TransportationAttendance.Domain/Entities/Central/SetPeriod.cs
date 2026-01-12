namespace TransportationAttendance.Domain.Entities.Central;

public class SetPeriod
{
    public int PeriodId { get; init; }
    public string PeriodDesc { get; init; } = string.Empty;
    public string? PeriodCode { get; init; }
    public TimeSpan? StartTime { get; init; }
    public TimeSpan? EndTime { get; init; }
    public string? LanguageP { get; init; }
}
