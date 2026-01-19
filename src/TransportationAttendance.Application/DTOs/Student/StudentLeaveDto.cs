namespace TransportationAttendance.Application.DTOs.Student;

public record StudentLeaveDto
{
    public Guid Id { get; init; }
    public Guid StudentId { get; init; }
    public string? StudentName { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Reason { get; init; } = string.Empty;
    public string? AttachmentUrl { get; init; }
    public string? AttachmentFileName { get; init; }
    public bool IsApproved { get; init; }
    public DateTime? ApprovedAt { get; init; }
    public bool IsCancelled { get; init; }
    public DateTime? CancelledAt { get; init; }
    public string? CancellationReason { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateLeaveDto
{
    public Guid StudentId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Reason { get; init; } = string.Empty;
    public string? AttachmentUrl { get; init; }
    public string? AttachmentFileName { get; init; }
}

public record CancelLeaveDto
{
    public string? Reason { get; init; }
}
