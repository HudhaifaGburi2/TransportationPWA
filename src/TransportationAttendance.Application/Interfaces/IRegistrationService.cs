using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Registration;
using TransportationAttendance.Application.DTOs.Student;

namespace TransportationAttendance.Application.Interfaces;

public interface IRegistrationService
{
    Task<Result<StudentHalaqaInfoDto>> GetStudentInfoForRegistrationAsync(int studentUserId, CancellationToken cancellationToken = default);
    Task<Result<RegistrationRequestDto>> SubmitRegistrationAsync(int studentUserId, CreateRegistrationRequestDto dto, CancellationToken cancellationToken = default);
    Task<Result<RegistrationRequestDto>> GetRegistrationByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<RegistrationRequestDto>> GetMyRegistrationAsync(int studentUserId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<RegistrationRequestDto>>> GetAllRegistrationsAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<RegistrationRequestDto>>> GetPendingRegistrationsAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<RegistrationRequestDto>>> GetRegistrationsByDistrictAsync(Guid districtId, CancellationToken cancellationToken = default);
    Task<Result<RegistrationRequestDto>> AssignToBusAsync(Guid requestId, Guid reviewerId, AssignBusRequestDto dto, CancellationToken cancellationToken = default);
    Task<Result<RegistrationRequestDto>> ReviewRegistrationAsync(Guid id, Guid reviewerId, ReviewRegistrationRequestDto dto, CancellationToken cancellationToken = default);
}
