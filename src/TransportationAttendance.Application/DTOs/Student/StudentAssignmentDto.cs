namespace TransportationAttendance.Application.DTOs.Student;

public record StudentAssignmentDto
{
    public Guid Id { get; init; }
    public Guid StudentId { get; init; }
    public string? StudentName { get; init; }
    public Guid BusId { get; init; }
    public string? BusNumber { get; init; }
    public string TransportType { get; init; } = "Both";
    public Guid? ArrivalBusId { get; init; }
    public string? ArrivalBusNumber { get; init; }
    public Guid? ReturnBusId { get; init; }
    public string? ReturnBusNumber { get; init; }
    public bool IsActive { get; init; }
    public DateTime AssignedAt { get; init; }
}

public record CreateAssignmentDto
{
    public Guid StudentId { get; init; }
    public Guid BusId { get; init; }
    public string TransportType { get; init; } = "Both";
    public Guid? ArrivalBusId { get; init; }
    public Guid? ReturnBusId { get; init; }
}

public record UpdateAssignmentDto
{
    public Guid BusId { get; init; }
    public string TransportType { get; init; } = "Both";
    public Guid? ArrivalBusId { get; init; }
    public Guid? ReturnBusId { get; init; }
}
