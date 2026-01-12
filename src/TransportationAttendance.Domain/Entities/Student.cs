using TransportationAttendance.Domain.Enums;

namespace TransportationAttendance.Domain.Entities;

public class Student : BaseEntity
{
    // Central DB Read-Only Fields (auto-filled from vw_Student_Halaqa_Teacher_information_Transportation_Dep)
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
    
    // Transportation-specific fields (editable)
    public Guid DistrictId { get; private set; }
    public string? HomeAddress { get; private set; }
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }
    public string? PhoneNumber { get; private set; }
    
    // Status
    public StudentStatus Status { get; private set; }
    
    // Navigation
    public District? District { get; private set; }

    private Student() { }

    public static Student CreateFromCentralDb(
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
        decimal? latitude,
        decimal? longitude,
        string? homeAddress = null,
        string? phoneNumber = null)
    {
        return new Student
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
            Latitude = latitude,
            Longitude = longitude,
            HomeAddress = homeAddress,
            PhoneNumber = phoneNumber,
            Status = StudentStatus.Active
        };
    }

    public void UpdateTransportInfo(Guid districtId, decimal? latitude, decimal? longitude, string? homeAddress, string? phoneNumber)
    {
        DistrictId = districtId;
        Latitude = latitude;
        Longitude = longitude;
        HomeAddress = homeAddress;
        PhoneNumber = phoneNumber;
    }

    public void UpdateLocation(decimal latitude, decimal longitude, string? homeAddress)
    {
        Latitude = latitude;
        Longitude = longitude;
        HomeAddress = homeAddress;
    }

    public void Suspend() => Status = StudentStatus.Suspended;
    public void Activate() => Status = StudentStatus.Active;
    public void SetOnLeave() => Status = StudentStatus.OnLeave;
    public void Deactivate() => Status = StudentStatus.Inactive;
}
