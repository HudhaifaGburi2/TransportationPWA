using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IBusRepository : IRepository<Bus>
{
    Task<IReadOnlyList<Bus>> GetByPeriodAsync(int periodId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Bus>> GetActiveBusesAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Get bus by PlateNumber within a specific period.
    /// Uniqueness is enforced on (PlateNumber + PeriodId) - same plate can exist in different periods.
    /// </summary>
    Task<Bus?> GetByPlateNumberAsync(string plateNumber, int periodId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Bus>> GetByDistrictAsync(Guid districtId, CancellationToken cancellationToken = default);
    Task<int> GetStudentCountAsync(Guid busId, CancellationToken cancellationToken = default);
}
