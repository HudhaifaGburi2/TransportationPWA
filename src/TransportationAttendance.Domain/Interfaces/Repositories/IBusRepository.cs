using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IBusRepository
{
    Task<Bus?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Bus>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Bus?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default);
    Task<Bus?> GetByBusNumberAsync(string busNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Bus>> GetActiveBusesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Bus>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task AddAsync(Bus bus, CancellationToken cancellationToken = default);
    void Update(Bus bus);
    void Remove(Bus bus);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetActiveCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetTotalCapacityAsync(CancellationToken cancellationToken = default);
}
