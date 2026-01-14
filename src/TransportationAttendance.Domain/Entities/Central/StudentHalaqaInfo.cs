namespace TransportationAttendance.Domain.Entities.Central;

public class StudentHalaqaInfo
{
    // Student Information
    public int StudentUserId { get; init; }
    public string StudentId { get; init; } = string.Empty;
    public string StudentName { get; init; } = string.Empty;
    public string? StudentLanguage { get; init; }

    // Education Type Information
    public int? EducationTypeId { get; init; }
    public string? EducationTypeName { get; init; }
    public string? EducationTypeCode { get; init; }

    // Halaqa Type Information
    public int? HalaqaTypeId { get; init; }
    public string? HalaqaTypeName { get; init; }
    public string? HalaqaTypeCode { get; init; }

    // Memorization Level Information
    public int? MemorizationLevelId { get; init; }
    public string? MemorizationLevelName { get; init; }

    // Teacher Information
    public int? TeacherDbId { get; init; }
    public string? TeacherId { get; init; }
    public string? TeacherName { get; init; }
    public int? TeacherUserId { get; init; }

    // Period Information
    public int? PeriodId { get; init; }
    public string? PeriodName { get; init; }
    public string? PeriodCode { get; init; }
    public string? PeriodStartTime { get; init; }
    public string? PeriodEndTime { get; init; }

    // Age Group Information
    public int? AgeGroupId { get; init; }
    public string? AgeGroupName { get; init; }
    public int? MinAgeLimit { get; init; }
    public int? MaxAgeLimit { get; init; }

    // Halaqa Location Information
    public int? HalaqaLocationId { get; init; }
    public string? HalaqaLocationName { get; init; }
    public string? HalaqaGender { get; init; }

    // MAP_Std_Edu_period Information
    public int? MapStdEduPeriodId { get; init; }
    public int? MapStatus { get; init; }

    // Halaqa Section Information
    public int? HalaqaSectionId { get; init; }
    public int? HalaqaSectionStatus { get; init; }

    // Teacher-Halaqa Mapping
    public int? TeacherHalaqaSecId { get; init; }

    // Student-Halaqa Mapping
    public int? StudentHalaqaSecId { get; init; }
}
