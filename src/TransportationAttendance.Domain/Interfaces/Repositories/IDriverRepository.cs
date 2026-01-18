using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IDriverRepository
{
    Task<Driver?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Driver>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Driver?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<Driver?> GetByLicenseNumberAsync(string licenseNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Driver>> GetActiveDriversAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Driver>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task AddAsync(Driver driver, CancellationToken cancellationToken = default);
    void Update(Driver driver);
    void Remove(Driver driver);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
}
