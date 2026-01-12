using AutoMapper;
using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Lookups;
using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Domain.Interfaces;

namespace TransportationAttendance.Application.Services;

public class LookupService : ILookupService
{
    private readonly ICentralDbRepository _centralDbRepository;
    private readonly IMapper _mapper;

    public LookupService(ICentralDbRepository centralDbRepository, IMapper mapper)
    {
        _centralDbRepository = centralDbRepository;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<PeriodDto>>> GetPeriodsAsync(string language = "ar-SA", CancellationToken cancellationToken = default)
    {
        var periods = await _centralDbRepository.GetAllPeriodsAsync(language, cancellationToken);
        return Result.Success(_mapper.Map<IReadOnlyList<PeriodDto>>(periods));
    }

    public async Task<Result<PeriodDto>> GetPeriodByIdAsync(int periodId, string language = "ar-SA", CancellationToken cancellationToken = default)
    {
        var period = await _centralDbRepository.GetPeriodByIdAsync(periodId, language, cancellationToken);
        if (period == null)
        {
            return Result.Failure<PeriodDto>("Period not found.");
        }
        return _mapper.Map<PeriodDto>(period);
    }

    public async Task<Result<IReadOnlyList<AgeGroupDto>>> GetAgeGroupsAsync(string language = "ar-SA", CancellationToken cancellationToken = default)
    {
        var ageGroups = await _centralDbRepository.GetAllAgeGroupsAsync(language, cancellationToken);
        return Result.Success(_mapper.Map<IReadOnlyList<AgeGroupDto>>(ageGroups));
    }

    public async Task<Result<AgeGroupDto>> GetAgeGroupByIdAsync(int ageGroupId, string language = "ar-SA", CancellationToken cancellationToken = default)
    {
        var ageGroup = await _centralDbRepository.GetAgeGroupByIdAsync(ageGroupId, language, cancellationToken);
        if (ageGroup == null)
        {
            return Result.Failure<AgeGroupDto>("Age group not found.");
        }
        return _mapper.Map<AgeGroupDto>(ageGroup);
    }

    public async Task<Result<IReadOnlyList<HalaqaLocationDto>>> GetHalaqaLocationsAsync(CancellationToken cancellationToken = default)
    {
        var locations = await _centralDbRepository.GetAllHalaqaLocationsAsync(cancellationToken);
        return Result.Success(_mapper.Map<IReadOnlyList<HalaqaLocationDto>>(locations));
    }

    public async Task<Result<HalaqaLocationDto>> GetHalaqaLocationByIdAsync(int locationId, CancellationToken cancellationToken = default)
    {
        var location = await _centralDbRepository.GetHalaqaLocationByIdAsync(locationId, cancellationToken);
        if (location == null)
        {
            return Result.Failure<HalaqaLocationDto>("Halaqa location not found.");
        }
        return _mapper.Map<HalaqaLocationDto>(location);
    }
}
