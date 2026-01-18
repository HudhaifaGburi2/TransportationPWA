using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.BusManagement;

namespace TransportationAttendance.Application.Interfaces.Services;

public interface IBusManagementService
{
    // Driver Operations
    Task<Result<IReadOnlyList<DriverManagementDto>>> GetAllDriversAsync(DriverQueryDto? query = null, CancellationToken cancellationToken = default);
    Task<Result<DriverManagementDto>> GetDriverByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<DriverManagementDto>> CreateDriverAsync(CreateDriverManagementDto dto, CancellationToken cancellationToken = default);
    Task<Result<DriverManagementDto>> UpdateDriverAsync(Guid id, UpdateDriverManagementDto dto, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteDriverAsync(Guid id, CancellationToken cancellationToken = default);

    // Route Operations
    Task<Result<IReadOnlyList<RouteManagementDto>>> GetAllRoutesAsync(RouteQueryDto? query = null, CancellationToken cancellationToken = default);
    Task<Result<RouteManagementDto>> GetRouteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<RouteManagementDto>> CreateRouteAsync(CreateRouteManagementDto dto, CancellationToken cancellationToken = default);
    Task<Result<RouteManagementDto>> UpdateRouteAsync(Guid id, UpdateRouteManagementDto dto, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteRouteAsync(Guid id, CancellationToken cancellationToken = default);

    // Bus Operations
    Task<Result<IReadOnlyList<BusManagementDto>>> GetAllBusesAsync(BusManagementQueryDto? query = null, CancellationToken cancellationToken = default);
    Task<Result<BusManagementDto>> GetBusByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<BusManagementDto>> CreateBusAsync(CreateBusManagementDto dto, CancellationToken cancellationToken = default);
    Task<Result<BusManagementDto>> UpdateBusAsync(Guid id, UpdateBusManagementDto dto, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteBusAsync(Guid id, CancellationToken cancellationToken = default);

    // Statistics
    Task<Result<BusManagementStatisticsDto>> GetStatisticsAsync(CancellationToken cancellationToken = default);
}
