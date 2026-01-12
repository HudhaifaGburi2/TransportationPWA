using AutoMapper;
using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.District;
using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces;

namespace TransportationAttendance.Application.Services;

public class DistrictService : IDistrictService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DistrictService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<DistrictDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var districts = await _unitOfWork.Districts.GetAllAsync(cancellationToken);
        return Result.Success(_mapper.Map<IReadOnlyList<DistrictDto>>(districts));
    }

    public async Task<Result<IReadOnlyList<DistrictDto>>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var districts = await _unitOfWork.Districts.GetActiveDistrictsAsync(cancellationToken);
        return Result.Success(_mapper.Map<IReadOnlyList<DistrictDto>>(districts));
    }

    public async Task<Result<DistrictDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var district = await _unitOfWork.Districts.GetByIdAsync(id, cancellationToken);
        if (district == null)
        {
            return Result.Failure<DistrictDto>("District not found.");
        }
        return _mapper.Map<DistrictDto>(district);
    }

    public async Task<Result<DistrictDto>> CreateAsync(CreateDistrictDto dto, CancellationToken cancellationToken = default)
    {
        var exists = await _unitOfWork.Districts.ExistsByNameAsync(dto.DistrictNameAr, cancellationToken);
        if (exists)
        {
            return Result.Failure<DistrictDto>("District with this name already exists.");
        }

        var district = District.Create(dto.DistrictNameAr, dto.DistrictNameEn);

        await _unitOfWork.Districts.AddAsync(district, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<DistrictDto>(district);
    }

    public async Task<Result<DistrictDto>> UpdateAsync(Guid id, CreateDistrictDto dto, CancellationToken cancellationToken = default)
    {
        var district = await _unitOfWork.Districts.GetByIdAsync(id, cancellationToken);
        if (district == null)
        {
            return Result.Failure<DistrictDto>("District not found.");
        }

        district.Update(dto.DistrictNameAr, dto.DistrictNameEn);
        _unitOfWork.Districts.Update(district);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<DistrictDto>(district);
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var district = await _unitOfWork.Districts.GetByIdAsync(id, cancellationToken);
        if (district == null)
        {
            return Result.Failure("District not found.");
        }

        district.SoftDelete(DateTime.UtcNow, null);
        _unitOfWork.Districts.Update(district);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
