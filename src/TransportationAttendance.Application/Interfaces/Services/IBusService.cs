using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Bus;

namespace TransportationAttendance.Application.Interfaces.Services;

public interface IBusService
{
    Task<Result<IReadOnlyList<BusDto>>> GetAllAsync(BusQueryDto? query = null, CancellationToken cancellationToken = default);
    Task<Result<BusDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<BusDto>>> GetByPeriodAsync(int periodId, CancellationToken cancellationToken = default);
    Task<Result<BusDto>> CreateAsync(CreateBusDto dto, CancellationToken cancellationToken = default);
    Task<Result<BusDto>> UpdateAsync(Guid id, UpdateBusDto dto, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<BusStatisticsDto>> GetStatisticsAsync(Guid busId, CancellationToken cancellationToken = default);
    Task<Result<BusSummaryDto>> GetSummaryAsync(CancellationToken cancellationToken = default);
    Task<Result<bool>> AssignDistrictsAsync(Guid busId, List<Guid> districtIds, CancellationToken cancellationToken = default);
}
