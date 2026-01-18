using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IActualBusRepository
{
    Task<ActualBus?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActualBus>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ActualBus?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default);
    Task<ActualBus?> GetByBusNumberAsync(string busNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActualBus>> GetActiveBusesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActualBus>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task AddAsync(ActualBus bus, CancellationToken cancellationToken = default);
    void Update(ActualBus bus);
    void Remove(ActualBus bus);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetActiveCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetTotalCapacityAsync(CancellationToken cancellationToken = default);
}
