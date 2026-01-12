using Microsoft.EntityFrameworkCore;
using TransportationAttendance.Domain.Entities.Central;
using TransportationAttendance.Domain.Interfaces;

namespace TransportationAttendance.Infrastructure.Persistence.Repositories;

public class CentralDbRepository : ICentralDbRepository
{
    private readonly CentralStudentDbContext _context;

    public CentralDbRepository(CentralStudentDbContext context)
    {
        _context = context;
    }

    // User authentication
    public async Task<RegtUser?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.RegtUsers
            .FirstOrDefaultAsync(u => u.Username == username || u.UserId.ToString() == username, cancellationToken);
    }

    public async Task<RegtUser?> GetUserByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.RegtUsers
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
    }

    public async Task<IReadOnlyList<RegtUserRole>> GetRolesByIdsAsync(IEnumerable<int> roleIds, CancellationToken cancellationToken = default)
    {
        return await _context.RegtUserRoles
            .Where(r => roleIds.Contains(r.RoleId))
            .ToListAsync(cancellationToken);
    }

    // Student Halaqa Info (from view)
    public async Task<StudentHalaqaInfo?> GetStudentHalaqaInfoByUserIdAsync(int studentUserId, CancellationToken cancellationToken = default)
    {
        return await _context.StudentHalaqaInfos
            .FirstOrDefaultAsync(s => s.StudentUserId == studentUserId, cancellationToken);
    }

    public async Task<StudentHalaqaInfo?> GetStudentHalaqaInfoByStudentIdAsync(string studentId, CancellationToken cancellationToken = default)
    {
        return await _context.StudentHalaqaInfos
            .FirstOrDefaultAsync(s => s.StudentId == studentId, cancellationToken);
    }

    public async Task<IReadOnlyList<StudentHalaqaInfo>> SearchStudentHalaqaInfoAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.StudentHalaqaInfos
            .Where(s => s.StudentId.Contains(searchTerm) || 
                       s.StudentName.Contains(searchTerm) ||
                       s.StudentUserId.ToString() == searchTerm)
            .Take(50)
            .ToListAsync(cancellationToken);
    }

    // Lookups
    public async Task<IReadOnlyList<SetPeriod>> GetAllPeriodsAsync(string language = "ar-SA", CancellationToken cancellationToken = default)
    {
        return await _context.SetPeriods
            .Where(p => p.LanguageP == language)
            .OrderBy(p => p.PeriodId)
            .ToListAsync(cancellationToken);
    }

    public async Task<SetPeriod?> GetPeriodByIdAsync(int periodId, string language = "ar-SA", CancellationToken cancellationToken = default)
    {
        return await _context.SetPeriods
            .FirstOrDefaultAsync(p => p.PeriodId == periodId && p.LanguageP == language, cancellationToken);
    }

    public async Task<IReadOnlyList<SetAgeGroup>> GetAllAgeGroupsAsync(string language = "ar-SA", CancellationToken cancellationToken = default)
    {
        return await _context.SetAgeGroups
            .Where(a => a.AgLanguage == language)
            .OrderBy(a => a.MinAgeLimit)
            .ToListAsync(cancellationToken);
    }

    public async Task<SetAgeGroup?> GetAgeGroupByIdAsync(int ageGroupId, string language = "ar-SA", CancellationToken cancellationToken = default)
    {
        return await _context.SetAgeGroups
            .FirstOrDefaultAsync(a => a.AgeGroupId == ageGroupId && a.AgLanguage == language, cancellationToken);
    }

    public async Task<IReadOnlyList<HalaqatLocation>> GetAllHalaqaLocationsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.HalaqatLocations
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<HalaqatLocation?> GetHalaqaLocationByIdAsync(int locationId, CancellationToken cancellationToken = default)
    {
        return await _context.HalaqatLocations
            .FirstOrDefaultAsync(l => l.LocationId == locationId, cancellationToken);
    }
}
