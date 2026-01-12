using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Domain.Interfaces.Repositories;

public interface IDistrictRepository : IRepository<District>
{
    Task<IReadOnlyList<District>> GetActiveDistrictsAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string districtNameAr, CancellationToken cancellationToken = default);
}
