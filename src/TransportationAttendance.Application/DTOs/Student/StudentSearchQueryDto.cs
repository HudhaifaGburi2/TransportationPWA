namespace TransportationAttendance.Application.DTOs.Student;

public record StudentSearchQueryDto
{
    public string? SearchTerm { get; init; }
    public string? Status { get; init; }
    public Guid? DistrictId { get; init; }
    public Guid? BusId { get; init; }
    public int? PeriodId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}
