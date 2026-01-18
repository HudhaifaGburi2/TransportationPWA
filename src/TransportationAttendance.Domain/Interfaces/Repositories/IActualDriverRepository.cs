using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IActualDriverRepository
{
    Task<ActualDriver?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActualDriver>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ActualDriver?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<ActualDriver?> GetByLicenseNumberAsync(string licenseNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActualDriver>> GetActiveDriversAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActualDriver>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task AddAsync(ActualDriver driver, CancellationToken cancellationToken = default);
    void Update(ActualDriver driver);
    void Remove(ActualDriver driver);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
}
