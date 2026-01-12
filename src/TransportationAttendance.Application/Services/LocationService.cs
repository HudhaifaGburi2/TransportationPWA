using AutoMapper;
using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Location;
using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces;

namespace TransportationAttendance.Application.Services;

public class LocationService : ILocationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LocationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<LocationDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var locations = await _unitOfWork.Locations.GetAllAsync(cancellationToken);
        return Result.Success(_mapper.Map<IReadOnlyList<LocationDto>>(locations));
    }

    public async Task<Result<IReadOnlyList<LocationDto>>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var locations = await _unitOfWork.Locations.GetActiveLocationsAsync(cancellationToken);
        return Result.Success(_mapper.Map<IReadOnlyList<LocationDto>>(locations));
    }

    public async Task<Result<LocationDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var location = await _unitOfWork.Locations.GetByIdAsync(id, cancellationToken);
        if (location == null)
        {
            return Result.Failure<LocationDto>("Location not found.");
        }
        return _mapper.Map<LocationDto>(location);
    }

    public async Task<Result<LocationDto>> CreateAsync(CreateLocationDto dto, CancellationToken cancellationToken = default)
    {
        var exists = await _unitOfWork.Locations.ExistsByCodeAsync(dto.LocationCode, cancellationToken);
        if (exists)
        {
            return Result.Failure<LocationDto>("Location with this code already exists.");
        }

        var location = Location.Create(dto.LocationCode, dto.LocationName, dto.LocationType);

        await _unitOfWork.Locations.AddAsync(location, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LocationDto>(location);
    }

    public async Task<Result<LocationDto>> UpdateAsync(Guid id, CreateLocationDto dto, CancellationToken cancellationToken = default)
    {
        var location = await _unitOfWork.Locations.GetByIdAsync(id, cancellationToken);
        if (location == null)
        {
            return Result.Failure<LocationDto>("Location not found.");
        }

        location.Update(dto.LocationCode, dto.LocationName, dto.LocationType);
        _unitOfWork.Locations.Update(location);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LocationDto>(location);
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var location = await _unitOfWork.Locations.GetByIdAsync(id, cancellationToken);
        if (location == null)
        {
            return Result.Failure("Location not found.");
        }

        location.SoftDelete(DateTime.UtcNow, null);
        _unitOfWork.Locations.Update(location);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
