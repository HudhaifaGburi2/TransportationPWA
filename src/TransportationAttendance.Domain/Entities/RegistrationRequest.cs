using TransportationAttendance.Domain.Enums;

namespace TransportationAttendance.Domain.Entities;

public class RegistrationRequest : BaseEntity
{
    // Central DB Read-Only Fields (auto-filled from view)
    public int StudentUserId { get; private set; }
    public string StudentId { get; private set; } = string.Empty;
    public string StudentName { get; private set; } = string.Empty;
    public string? HalaqaTypeCode { get; private set; }
    public Guid? HalaqaSectionId { get; private set; }
    public string? HalaqaGender { get; private set; }
    public int? PeriodId { get; private set; }
    public int? AgeGroupId { get; private set; }
    public int? HalaqaLocationId { get; private set; }
    public string? TeacherName { get; private set; }
    
    // Student-provided fields
    public Guid DistrictId { get; private set; }
    public string NationalShortAddress { get; private set; } = string.Empty;
    public string? FullNationalAddress { get; private set; }
    public string? HomeAddress { get; private set; }
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }
    
    /// <summary>
    /// JSON array of period IDs the student is registering for (e.g., ["1","3","5"])
    /// </summary>
    public string? Periods { get; private set; }
    
    // Request status
    public RegistrationStatus Status { get; private set; }
    public DateTime RequestedAt { get; private set; }
    
    // Review information
    public DateTime? ReviewedAt { get; private set; }
    public Guid? ReviewedBy { get; private set; }
    public string? ReviewNotes { get; private set; }
    
    // Navigation
    public District? District { get; private set; }

    private RegistrationRequest() { }

    public static RegistrationRequest CreateFromCentralDb(
        int studentUserId,
        string studentId,
        string studentName,
        string? halaqaTypeCode,
        Guid? halaqaSectionId,
        string? halaqaGender,
        int? periodId,
        int? ageGroupId,
        int? halaqaLocationId,
        string? teacherName,
        Guid districtId,
        string nationalShortAddress,
        string? fullNationalAddress = null,
        string? periods = null,
        decimal? latitude = null,
        decimal? longitude = null,
        string? homeAddress = null)
    {
        return new RegistrationRequest
        {
            StudentUserId = studentUserId,
            StudentId = studentId,
            StudentName = studentName,
            HalaqaTypeCode = halaqaTypeCode,
            HalaqaSectionId = halaqaSectionId,
            HalaqaGender = halaqaGender,
            PeriodId = periodId,
            AgeGroupId = ageGroupId,
            HalaqaLocationId = halaqaLocationId,
            TeacherName = teacherName,
            DistrictId = districtId,
            NationalShortAddress = nationalShortAddress,
            FullNationalAddress = fullNationalAddress,
            Periods = periods,
            Latitude = latitude,
            Longitude = longitude,
            HomeAddress = homeAddress,
            Status = RegistrationStatus.Pending,
            RequestedAt = DateTime.UtcNow
        };
    }

    public void Approve(Guid reviewedBy, string? notes = null)
    {
        Status = RegistrationStatus.Approved;
        ReviewedAt = DateTime.UtcNow;
        ReviewedBy = reviewedBy;
        ReviewNotes = notes;
    }

    public void Reject(Guid reviewedBy, string notes)
    {
        Status = RegistrationStatus.Rejected;
        ReviewedAt = DateTime.UtcNow;
        ReviewedBy = reviewedBy;
        ReviewNotes = notes;
    }

    public bool IsPending() => Status == RegistrationStatus.Pending;
    public bool IsApproved() => Status == RegistrationStatus.Approved;
    public bool IsRejected() => Status == RegistrationStatus.Rejected;
}
