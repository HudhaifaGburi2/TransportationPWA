namespace TransportationAttendance.Application.DTOs.BusManagement;

public record BusStudentsDto
{
    public Guid BusId { get; init; }
    public string BusNumber { get; init; } = string.Empty;
    public int Capacity { get; init; }
    public IReadOnlyList<BusStudentDto> Students { get; init; } = new List<BusStudentDto>();
}

public record BusStudentDto
{
    public Guid Id { get; init; }
    public string StudentId { get; init; } = string.Empty;
    public string StudentName { get; init; } = string.Empty;
    public string? DistrictName { get; init; }
    public string? PeriodName { get; init; }
    public DateTime AssignedAt { get; init; }
}
