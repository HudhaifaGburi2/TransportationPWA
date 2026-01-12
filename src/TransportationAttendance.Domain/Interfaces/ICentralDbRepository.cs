using TransportationAttendance.Domain.Entities.Central;

namespace TransportationAttendance.Domain.Interfaces;

public interface ICentralDbRepository
{
    // User authentication
    Task<RegtUser?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<RegtUser?> GetUserByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RegtUserRole>> GetRolesByIdsAsync(IEnumerable<int> roleIds, CancellationToken cancellationToken = default);
    
    // Student Halaqa Info (from view)
    Task<StudentHalaqaInfo?> GetStudentHalaqaInfoByUserIdAsync(int studentUserId, CancellationToken cancellationToken = default);
    Task<StudentHalaqaInfo?> GetStudentHalaqaInfoByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentHalaqaInfo>> SearchStudentHalaqaInfoAsync(string searchTerm, CancellationToken cancellationToken = default);
    
    // Lookups
    Task<IReadOnlyList<SetPeriod>> GetAllPeriodsAsync(string language = "ar-SA", CancellationToken cancellationToken = default);
    Task<SetPeriod?> GetPeriodByIdAsync(int periodId, string language = "ar-SA", CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<SetAgeGroup>> GetAllAgeGroupsAsync(string language = "ar-SA", CancellationToken cancellationToken = default);
    Task<SetAgeGroup?> GetAgeGroupByIdAsync(int ageGroupId, string language = "ar-SA", CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<HalaqatLocation>> GetAllHalaqaLocationsAsync(CancellationToken cancellationToken = default);
    Task<HalaqatLocation?> GetHalaqaLocationByIdAsync(int locationId, CancellationToken cancellationToken = default);
}
