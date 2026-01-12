using TransportationAttendance.Application.DTOs.District;

namespace TransportationAttendance.Application.DTOs.Student;

public record StudentDto
{
    public Guid Id { get; init; }
    
    // Central DB Read-Only Fields
    public int StudentUserId { get; init; }
    public string StudentId { get; init; } = string.Empty;
    public string StudentName { get; init; } = string.Empty;
    public string? HalaqaTypeCode { get; init; }
    public Guid? HalaqaSectionId { get; init; }
    public string? HalaqaGender { get; init; }
    public int? PeriodId { get; init; }
    public int? AgeGroupId { get; init; }
    public int? HalaqaLocationId { get; init; }
    public string? TeacherName { get; init; }
    
    // Transportation fields
    public Guid DistrictId { get; init; }
    public string? HomeAddress { get; init; }
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    public string? PhoneNumber { get; init; }
    public string Status { get; init; } = string.Empty;
    
    // Navigation
    public DistrictDto? District { get; init; }
}
