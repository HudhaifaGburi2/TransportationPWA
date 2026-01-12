using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Lookups;

namespace TransportationAttendance.Application.Interfaces;

public interface ILookupService
{
    Task<Result<IReadOnlyList<PeriodDto>>> GetPeriodsAsync(string language = "ar-SA", CancellationToken cancellationToken = default);
    Task<Result<PeriodDto>> GetPeriodByIdAsync(int periodId, string language = "ar-SA", CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<AgeGroupDto>>> GetAgeGroupsAsync(string language = "ar-SA", CancellationToken cancellationToken = default);
    Task<Result<AgeGroupDto>> GetAgeGroupByIdAsync(int ageGroupId, string language = "ar-SA", CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<HalaqaLocationDto>>> GetHalaqaLocationsAsync(CancellationToken cancellationToken = default);
    Task<Result<HalaqaLocationDto>> GetHalaqaLocationByIdAsync(int locationId, CancellationToken cancellationToken = default);
}
