namespace TransportationAttendance.Domain.Entities.Central;

public class SetAgeGroup
{
    public int AgeGroupId { get; init; }
    public string AgName { get; init; } = string.Empty;
    public int? MinAgeLimit { get; init; }
    public int? MaxAgeLimit { get; init; }
    public string? AgLanguage { get; init; }
}
