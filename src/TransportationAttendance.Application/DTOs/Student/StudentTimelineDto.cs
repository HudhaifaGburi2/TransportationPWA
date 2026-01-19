namespace TransportationAttendance.Application.DTOs.Student;

public record StudentTimelineDto
{
    public Guid StudentId { get; init; }
    public string StudentName { get; init; } = string.Empty;
    public IReadOnlyList<TimelineEventDto> Events { get; init; } = new List<TimelineEventDto>();
}

public record TimelineEventDto
{
    public Guid Id { get; init; }
    public string EventType { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime OccurredAt { get; init; }
    public Dictionary<string, object?> Details { get; init; } = new();
}
