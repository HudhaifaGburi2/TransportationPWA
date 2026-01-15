using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.NationalAddress;

namespace TransportationAttendance.Application.Interfaces;

/// <summary>
/// Service for looking up Saudi National Addresses
/// </summary>
public interface INationalAddressService
{
    /// <summary>
    /// Lookup a full address from a short national address code
    /// </summary>
    /// <param name="shortAddress">Short address code (e.g., RRRD2929)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Full address details or error</returns>
    Task<Result<NationalAddressDto>> LookupAddressAsync(string shortAddress, CancellationToken cancellationToken = default);
}
