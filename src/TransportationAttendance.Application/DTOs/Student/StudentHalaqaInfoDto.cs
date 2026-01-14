namespace TransportationAttendance.Application.DTOs.Student;

public record StudentHalaqaInfoDto
{
    // Student Information (Read-Only from Central DB)
    public int StudentUserId { get; init; }
    public string StudentId { get; init; } = string.Empty;
    public string StudentName { get; init; } = string.Empty;
    
    // Halaqa Type Information
    public string? HalaqaTypeCode { get; init; }
    public string? HalaqaTypeName { get; init; }
    
    // Halaqa Section
    public int? HalaqaSectionId { get; init; }
    public string? HalaqaGender { get; init; }
    
    // Period Information
    public int? PeriodId { get; init; }
    public string? PeriodName { get; init; }
    public string? PeriodCode { get; init; }
    
    // Age Group Information
    public int? AgeGroupId { get; init; }
    public string? AgeGroupName { get; init; }
    
    // Halaqa Location
    public int? HalaqaLocationId { get; init; }
    public string? HalaqaLocationName { get; init; }
    
    // Teacher Information
    public string? TeacherName { get; init; }
}
