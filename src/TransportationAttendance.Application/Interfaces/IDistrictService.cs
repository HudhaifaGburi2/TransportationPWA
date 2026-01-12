using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.District;

namespace TransportationAttendance.Application.Interfaces;

public interface IDistrictService
{
    Task<Result<IReadOnlyList<DistrictDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<DistrictDto>>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<Result<DistrictDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<DistrictDto>> CreateAsync(CreateDistrictDto dto, CancellationToken cancellationToken = default);
    Task<Result<DistrictDto>> UpdateAsync(Guid id, CreateDistrictDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
