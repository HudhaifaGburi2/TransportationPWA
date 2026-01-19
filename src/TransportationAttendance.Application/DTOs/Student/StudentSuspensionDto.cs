namespace TransportationAttendance.Application.DTOs.Student;

public record StudentSuspensionDto
{
    public Guid Id { get; init; }
    public Guid StudentId { get; init; }
    public string? StudentName { get; init; }
    public Guid? BusId { get; init; }
    public string? BusNumber { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTime SuspendedAt { get; init; }
    public bool IsReactivated { get; init; }
    public DateTime? ReactivatedAt { get; init; }
    public Guid? NewBusIdAfterReactivation { get; init; }
    public string? NewBusNumberAfterReactivation { get; init; }
    public string? ReactivationNotes { get; init; }
}

public record CreateSuspensionDto
{
    public Guid StudentId { get; init; }
    public string? Reason { get; init; }
}

public record ReactivateStudentDto
{
    public Guid? NewBusId { get; init; }
    public string? Notes { get; init; }
}
