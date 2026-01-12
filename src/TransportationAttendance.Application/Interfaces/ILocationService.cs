using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Location;

namespace TransportationAttendance.Application.Interfaces;

public interface ILocationService
{
    Task<Result<IReadOnlyList<LocationDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<LocationDto>>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<Result<LocationDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<LocationDto>> CreateAsync(CreateLocationDto dto, CancellationToken cancellationToken = default);
    Task<Result<LocationDto>> UpdateAsync(Guid id, CreateLocationDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
