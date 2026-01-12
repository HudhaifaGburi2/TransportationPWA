using AutoMapper;
using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Registration;
using TransportationAttendance.Application.DTOs.Student;
using TransportationAttendance.Application.Interfaces;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces;

namespace TransportationAttendance.Application.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICentralDbRepository _centralDbRepository;
    private readonly IMapper _mapper;

    public RegistrationService(IUnitOfWork unitOfWork, ICentralDbRepository centralDbRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _centralDbRepository = centralDbRepository;
        _mapper = mapper;
    }

    public async Task<Result<StudentHalaqaInfoDto>> GetStudentInfoForRegistrationAsync(int studentUserId, CancellationToken cancellationToken = default)
    {
        var studentInfo = await _centralDbRepository.GetStudentHalaqaInfoByUserIdAsync(studentUserId, cancellationToken);
        if (studentInfo == null)
        {
            return Result.Failure<StudentHalaqaInfoDto>("Student not found in Central Database.");
        }

        return _mapper.Map<StudentHalaqaInfoDto>(studentInfo);
    }

    public async Task<Result<RegistrationRequestDto>> SubmitRegistrationAsync(int studentUserId, CreateRegistrationRequestDto dto, CancellationToken cancellationToken = default)
    {
        // Check if student already has a pending registration
        var hasPending = await _unitOfWork.RegistrationRequests.HasPendingRequestAsync(studentUserId, cancellationToken);
        if (hasPending)
        {
            return Result.Failure<RegistrationRequestDto>("You already have a pending registration request.");
        }

        // Check if student is already registered
        var existingStudent = await _unitOfWork.Students.ExistsByUserIdAsync(studentUserId, cancellationToken);
        if (existingStudent)
        {
            return Result.Failure<RegistrationRequestDto>("You are already registered for transportation.");
        }

        // Verify district exists
        var district = await _unitOfWork.Districts.GetByIdAsync(dto.DistrictId, cancellationToken);
        if (district == null)
        {
            return Result.Failure<RegistrationRequestDto>("Selected district not found.");
        }

        // Get student info from Central DB (auto-fill read-only fields)
        var studentInfo = await _centralDbRepository.GetStudentHalaqaInfoByUserIdAsync(studentUserId, cancellationToken);
        if (studentInfo == null)
        {
            return Result.Failure<RegistrationRequestDto>("Student not found in Central Database.");
        }

        // Create registration request with auto-filled Central DB data
        var registrationRequest = RegistrationRequest.CreateFromCentralDb(
            studentUserId: studentInfo.StudentUserId,
            studentId: studentInfo.StudentId,
            studentName: studentInfo.StudentName,
            halaqaTypeCode: studentInfo.HalaqaTypeCode,
            halaqaSectionId: studentInfo.HalaqaSectionId,
            halaqaGender: studentInfo.HalaqaGender,
            periodId: studentInfo.PeriodId,
            ageGroupId: studentInfo.AgeGroupId,
            halaqaLocationId: studentInfo.HalaqaLocationId,
            teacherName: studentInfo.TeacherName,
            districtId: dto.DistrictId,
            latitude: dto.Latitude,
            longitude: dto.Longitude,
            homeAddress: dto.HomeAddress
        );

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _unitOfWork.RegistrationRequests.AddAsync(registrationRequest, cancellationToken);

            // Create audit log
            var auditLog = AuditLog.Create(
                action: "RegistrationRequestSubmitted",
                entityType: "RegistrationRequest",
                entityId: registrationRequest.Id,
                newValues: System.Text.Json.JsonSerializer.Serialize(new { dto.DistrictId, dto.Latitude, dto.Longitude })
            );
            await _unitOfWork.AuditLogs.AddAsync(auditLog, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }

        return _mapper.Map<RegistrationRequestDto>(registrationRequest);
    }

    public async Task<Result<RegistrationRequestDto>> GetRegistrationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = await _unitOfWork.RegistrationRequests.GetByIdAsync(id, cancellationToken);
        if (request == null)
        {
            return Result.Failure<RegistrationRequestDto>("Registration request not found.");
        }

        return _mapper.Map<RegistrationRequestDto>(request);
    }

    public async Task<Result<RegistrationRequestDto>> GetMyRegistrationAsync(int studentUserId, CancellationToken cancellationToken = default)
    {
        var request = await _unitOfWork.RegistrationRequests.GetByStudentUserIdAsync(studentUserId, cancellationToken);
        if (request == null)
        {
            return Result.Failure<RegistrationRequestDto>("No registration request found.");
        }

        return _mapper.Map<RegistrationRequestDto>(request);
    }

    public async Task<Result<IReadOnlyList<RegistrationRequestDto>>> GetPendingRegistrationsAsync(CancellationToken cancellationToken = default)
    {
        var requests = await _unitOfWork.RegistrationRequests.GetPendingRequestsAsync(cancellationToken);
        return Result.Success(_mapper.Map<IReadOnlyList<RegistrationRequestDto>>(requests));
    }

    public async Task<Result<IReadOnlyList<RegistrationRequestDto>>> GetRegistrationsByDistrictAsync(Guid districtId, CancellationToken cancellationToken = default)
    {
        var requests = await _unitOfWork.RegistrationRequests.GetByDistrictAsync(districtId, cancellationToken);
        return Result.Success(_mapper.Map<IReadOnlyList<RegistrationRequestDto>>(requests));
    }

    public async Task<Result<RegistrationRequestDto>> ReviewRegistrationAsync(Guid id, Guid reviewerId, ReviewRegistrationRequestDto dto, CancellationToken cancellationToken = default)
    {
        var request = await _unitOfWork.RegistrationRequests.GetByIdAsync(id, cancellationToken);
        if (request == null)
        {
            return Result.Failure<RegistrationRequestDto>("Registration request not found.");
        }

        if (!request.IsPending())
        {
            return Result.Failure<RegistrationRequestDto>("This registration request has already been reviewed.");
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            if (dto.IsApproved)
            {
                request.Approve(reviewerId, dto.Notes);

                // Create student record from approved registration
                var student = Student.CreateFromCentralDb(
                    studentUserId: request.StudentUserId,
                    studentId: request.StudentId,
                    studentName: request.StudentName,
                    halaqaTypeCode: request.HalaqaTypeCode,
                    halaqaSectionId: request.HalaqaSectionId,
                    halaqaGender: request.HalaqaGender,
                    periodId: request.PeriodId,
                    ageGroupId: request.AgeGroupId,
                    halaqaLocationId: request.HalaqaLocationId,
                    teacherName: request.TeacherName,
                    districtId: request.DistrictId,
                    latitude: request.Latitude,
                    longitude: request.Longitude,
                    homeAddress: request.HomeAddress
                );

                await _unitOfWork.Students.AddAsync(student, cancellationToken);

                // Create audit log for approval
                var approvalLog = AuditLog.Create(
                    action: "RegistrationRequestApproved",
                    userId: reviewerId,
                    entityType: "RegistrationRequest",
                    entityId: request.Id,
                    newValues: System.Text.Json.JsonSerializer.Serialize(new { StudentCreated = student.Id })
                );
                await _unitOfWork.AuditLogs.AddAsync(approvalLog, cancellationToken);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(dto.Notes))
                {
                    await _unitOfWork.RollbackAsync(cancellationToken);
                    return Result.Failure<RegistrationRequestDto>("Rejection reason is required.");
                }

                request.Reject(reviewerId, dto.Notes);

                // Create audit log for rejection
                var rejectionLog = AuditLog.Create(
                    action: "RegistrationRequestRejected",
                    userId: reviewerId,
                    entityType: "RegistrationRequest",
                    entityId: request.Id,
                    newValues: System.Text.Json.JsonSerializer.Serialize(new { Reason = dto.Notes })
                );
                await _unitOfWork.AuditLogs.AddAsync(rejectionLog, cancellationToken);
            }

            _unitOfWork.RegistrationRequests.Update(request);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }

        return _mapper.Map<RegistrationRequestDto>(request);
    }
}
