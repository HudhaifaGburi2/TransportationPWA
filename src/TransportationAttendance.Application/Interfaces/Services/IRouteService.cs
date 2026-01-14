using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Route;

namespace TransportationAttendance.Application.Interfaces.Services;

public interface IRouteService
{
    Task<Result<IReadOnlyList<RouteDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<RouteDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<RouteDto>> CreateAsync(CreateRouteDto dto, CancellationToken cancellationToken = default);
    Task<Result<RouteDto>> UpdateAsync(Guid id, UpdateRouteDto dto, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
