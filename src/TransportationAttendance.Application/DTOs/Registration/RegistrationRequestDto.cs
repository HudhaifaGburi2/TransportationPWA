using TransportationAttendance.Application.DTOs.District;

namespace TransportationAttendance.Application.DTOs.Registration;

public record RegistrationRequestDto
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
    
    // Student-provided fields
    public Guid DistrictId { get; init; }
    public string NationalShortAddress { get; init; } = string.Empty;
    public string? FullNationalAddress { get; init; }
    public string? HomeAddress { get; init; }
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    
    /// <summary>
    /// JSON array of period IDs the student registered for
    /// </summary>
    public List<string>? Periods { get; init; }
    
    // Status
    public string Status { get; init; } = string.Empty;
    public DateTime RequestedAt { get; init; }
    
    // Review information
    public DateTime? ReviewedAt { get; init; }
    public Guid? ReviewedBy { get; init; }
    public string? ReviewNotes { get; init; }
    
    // Navigation
    public DistrictDto? District { get; init; }
}
