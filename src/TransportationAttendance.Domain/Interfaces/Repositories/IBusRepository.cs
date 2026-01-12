using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IBusRepository : IRepository<Bus>
{
    Task<IReadOnlyList<Bus>> GetByPeriodAsync(int periodId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Bus>> GetActiveBusesAsync(CancellationToken cancellationToken = default);
    Task<Bus?> GetByBusNumberAsync(string busNumber, int periodId, CancellationToken cancellationToken = default);
}
