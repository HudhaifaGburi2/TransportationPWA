using AutoMapper;
using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Student;
using TransportationAttendance.Application.Interfaces.Services;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Enums;
using TransportationAttendance.Domain.Interfaces;

namespace TransportationAttendance.Application.Services;

public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Search and Retrieval

    public async Task<Result<PagedResult<StudentDto>>> SearchStudentsAsync(StudentSearchQueryDto query, CancellationToken cancellationToken = default)
    {
        var students = await _unitOfWork.Students.GetActiveStudentsAsync(cancellationToken);
        var filtered = students.AsEnumerable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.ToLower();
            filtered = filtered.Where(s => 
                s.StudentName.ToLower().Contains(term) ||
                s.StudentId.ToLower().Contains(term) ||
                (s.PhoneNumber?.Contains(term) ?? false));
        }

        if (!string.IsNullOrWhiteSpace(query.Status) && Enum.TryParse<StudentStatus>(query.Status, true, out var status))
        {
            filtered = filtered.Where(s => s.Status == status);
        }

        if (query.DistrictId.HasValue)
        {
            filtered = filtered.Where(s => s.DistrictId == query.DistrictId.Value);
        }

        if (query.PeriodId.HasValue)
        {
            filtered = filtered.Where(s => s.PeriodId == query.PeriodId.Value);
        }

        var totalCount = filtered.Count();
        var items = filtered
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        return Result.Success(PagedResult<StudentDto>.Create(
            _mapper.Map<IReadOnlyList<StudentDto>>(items),
            totalCount,
            query.PageNumber,
            query.PageSize
        ));
    }

    public async Task<Result<StudentDto>> GetStudentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id, cancellationToken);
        if (student == null)
            return Result.Failure<StudentDto>("الطالب غير موجود");

        return _mapper.Map<StudentDto>(student);
    }

    public async Task<Result<StudentDto>> GetStudentByStudentIdAsync(string studentId, CancellationToken cancellationToken = default)
    {
        var student = await _unitOfWork.Students.GetByStudentIdAsync(studentId, cancellationToken);
        if (student == null)
            return Result.Failure<StudentDto>("الطالب غير موجود");

        return _mapper.Map<StudentDto>(student);
    }

    public async Task<Result<IReadOnlyList<StudentDto>>> GetStudentsByDistrictAsync(Guid districtId, CancellationToken cancellationToken = default)
    {
        var students = await _unitOfWork.Students.GetByDistrictAsync(districtId, cancellationToken);
        return Result.Success(_mapper.Map<IReadOnlyList<StudentDto>>(students));
    }

    public async Task<Result<IReadOnlyList<StudentDto>>> GetActiveStudentsAsync(CancellationToken cancellationToken = default)
    {
        var students = await _unitOfWork.Students.GetActiveStudentsAsync(cancellationToken);
        return Result.Success(_mapper.Map<IReadOnlyList<StudentDto>>(students));
    }

    #endregion

    #region Assignment Operations

    public async Task<Result<StudentAssignmentDto>> AssignStudentToBusAsync(CreateAssignmentDto dto, Guid assignedBy, CancellationToken cancellationToken = default)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(dto.StudentId, cancellationToken);
        if (student == null)
            return Result.Failure<StudentAssignmentDto>("الطالب غير موجود");

        var bus = await _unitOfWork.Buses.GetByIdAsync(dto.BusId, cancellationToken);
        if (bus == null)
            return Result.Failure<StudentAssignmentDto>("الباص غير موجود");

        // Check if student already has an active assignment
        var existingAssignment = await _unitOfWork.StudentBusAssignments.GetActiveByStudentIdAsync(dto.StudentId, cancellationToken);
        if (existingAssignment != null)
            return Result.Failure<StudentAssignmentDto>("الطالب لديه تعيين نشط بالفعل");

        // Parse transport type
        if (!Enum.TryParse<TransportType>(dto.TransportType, true, out var transportType))
            transportType = TransportType.Both;

        // Validate split transport buses
        if (transportType != TransportType.Both)
        {
            if (dto.ArrivalBusId.HasValue)
            {
                var arrivalBus = await _unitOfWork.Buses.GetByIdAsync(dto.ArrivalBusId.Value, cancellationToken);
                if (arrivalBus == null)
                    return Result.Failure<StudentAssignmentDto>("باص الذهاب غير موجود");
            }
            if (dto.ReturnBusId.HasValue)
            {
                var returnBus = await _unitOfWork.Buses.GetByIdAsync(dto.ReturnBusId.Value, cancellationToken);
                if (returnBus == null)
                    return Result.Failure<StudentAssignmentDto>("باص العودة غير موجود");
            }
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var assignment = StudentBusAssignment.Create(
                studentId: dto.StudentId,
                busId: dto.BusId,
                transportType: transportType,
                arrivalBusId: dto.ArrivalBusId,
                returnBusId: dto.ReturnBusId,
                assignedBy: assignedBy
            );

            await _unitOfWork.StudentBusAssignments.AddAsync(assignment, cancellationToken);

            var auditLog = AuditLog.Create(
                action: "StudentAssignedToBus",
                userId: assignedBy,
                entityType: "StudentBusAssignment",
                entityId: assignment.Id,
                newValues: System.Text.Json.JsonSerializer.Serialize(new { dto.StudentId, dto.BusId, dto.TransportType })
            );
            await _unitOfWork.AuditLogs.AddAsync(auditLog, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(MapToAssignmentDto(assignment, student, bus));
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<Result<StudentAssignmentDto>> UpdateStudentAssignmentAsync(Guid studentId, UpdateAssignmentDto dto, CancellationToken cancellationToken = default)
    {
        var assignment = await _unitOfWork.StudentBusAssignments.GetActiveByStudentIdAsync(studentId, cancellationToken);
        if (assignment == null)
            return Result.Failure<StudentAssignmentDto>("لا يوجد تعيين نشط لهذا الطالب");

        var bus = await _unitOfWork.Buses.GetByIdAsync(dto.BusId, cancellationToken);
        if (bus == null)
            return Result.Failure<StudentAssignmentDto>("الباص غير موجود");

        if (!Enum.TryParse<TransportType>(dto.TransportType, true, out var transportType))
            transportType = TransportType.Both;

        assignment.UpdateAssignment(dto.BusId, transportType, dto.ArrivalBusId, dto.ReturnBusId);
        _unitOfWork.StudentBusAssignments.Update(assignment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var student = await _unitOfWork.Students.GetByIdAsync(studentId, cancellationToken);
        return Result.Success(MapToAssignmentDto(assignment, student, bus));
    }

    public async Task<Result<StudentAssignmentDto>> GetStudentAssignmentAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var assignment = await _unitOfWork.StudentBusAssignments.GetActiveByStudentIdAsync(studentId, cancellationToken);
        if (assignment == null)
            return Result.Failure<StudentAssignmentDto>("لا يوجد تعيين نشط لهذا الطالب");

        var student = await _unitOfWork.Students.GetByIdAsync(studentId, cancellationToken);
        var bus = await _unitOfWork.Buses.GetByIdAsync(assignment.BusId, cancellationToken);

        return Result.Success(MapToAssignmentDto(assignment, student, bus));
    }

    public async Task<Result<IReadOnlyList<StudentAssignmentDto>>> GetStudentsByBusAsync(Guid busId, CancellationToken cancellationToken = default)
    {
        var assignments = await _unitOfWork.StudentBusAssignments.GetActiveByBusIdAsync(busId, cancellationToken);
        var bus = await _unitOfWork.Buses.GetByIdAsync(busId, cancellationToken);

        var result = assignments.Select(a => MapToAssignmentDto(a, a.Student, bus)).ToList();
        return Result.Success<IReadOnlyList<StudentAssignmentDto>>(result);
    }

    #endregion

    #region Suspension Operations

    public async Task<Result<StudentSuspensionDto>> SuspendStudentAsync(CreateSuspensionDto dto, Guid suspendedBy, CancellationToken cancellationToken = default)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(dto.StudentId, cancellationToken);
        if (student == null)
            return Result.Failure<StudentSuspensionDto>("الطالب غير موجود");

        if (student.Status == StudentStatus.Suspended)
            return Result.Failure<StudentSuspensionDto>("الطالب موقوف بالفعل");

        var existingActive = await _unitOfWork.StudentSuspensions.HasActiveSuspensionAsync(dto.StudentId, cancellationToken);
        if (existingActive)
            return Result.Failure<StudentSuspensionDto>("الطالب لديه إيقاف نشط بالفعل");

        // Get current bus assignment
        var assignment = await _unitOfWork.StudentBusAssignments.GetActiveByStudentIdAsync(dto.StudentId, cancellationToken);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var suspension = StudentSuspension.Create(
                studentId: dto.StudentId,
                suspendedBy: suspendedBy,
                reason: dto.Reason,
                busId: assignment?.BusId
            );

            await _unitOfWork.StudentSuspensions.AddAsync(suspension, cancellationToken);

            // Update student status
            student.Suspend();
            _unitOfWork.Students.Update(student);

            // Deactivate bus assignment if exists
            if (assignment != null)
            {
                assignment.Deactivate();
                _unitOfWork.StudentBusAssignments.Update(assignment);
            }

            var auditLog = AuditLog.Create(
                action: "StudentSuspended",
                userId: suspendedBy,
                entityType: "StudentSuspension",
                entityId: suspension.Id,
                newValues: System.Text.Json.JsonSerializer.Serialize(new { dto.StudentId, dto.Reason })
            );
            await _unitOfWork.AuditLogs.AddAsync(auditLog, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(new StudentSuspensionDto
            {
                Id = suspension.Id,
                StudentId = suspension.StudentId,
                StudentName = student.StudentName,
                BusId = suspension.BusId,
                Reason = suspension.Reason,
                SuspendedAt = suspension.SuspendedAt,
                IsReactivated = false
            });
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<Result<StudentSuspensionDto>> ReactivateStudentAsync(Guid suspensionId, ReactivateStudentDto dto, Guid reactivatedBy, CancellationToken cancellationToken = default)
    {
        var suspension = await _unitOfWork.StudentSuspensions.GetByIdAsync(suspensionId, cancellationToken);
        if (suspension == null)
            return Result.Failure<StudentSuspensionDto>("سجل الإيقاف غير موجود");

        if (suspension.IsReactivated)
            return Result.Failure<StudentSuspensionDto>("تم إعادة تفعيل الطالب مسبقاً");

        var student = await _unitOfWork.Students.GetByIdAsync(suspension.StudentId, cancellationToken);
        if (student == null)
            return Result.Failure<StudentSuspensionDto>("الطالب غير موجود");

        // Validate new bus if provided
        Bus? newBus = null;
        if (dto.NewBusId.HasValue)
        {
            newBus = await _unitOfWork.Buses.GetByIdAsync(dto.NewBusId.Value, cancellationToken);
            if (newBus == null)
                return Result.Failure<StudentSuspensionDto>("الباص المحدد غير موجود");
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            suspension.Reactivate(reactivatedBy, dto.NewBusId, dto.Notes);
            _unitOfWork.StudentSuspensions.Update(suspension);

            student.Activate();
            _unitOfWork.Students.Update(student);

            // Create new bus assignment if bus is specified
            if (dto.NewBusId.HasValue)
            {
                var assignment = StudentBusAssignment.Create(
                    studentId: student.Id,
                    busId: dto.NewBusId.Value,
                    assignedBy: reactivatedBy
                );
                await _unitOfWork.StudentBusAssignments.AddAsync(assignment, cancellationToken);
            }

            var auditLog = AuditLog.Create(
                action: "StudentReactivated",
                userId: reactivatedBy,
                entityType: "StudentSuspension",
                entityId: suspension.Id,
                newValues: System.Text.Json.JsonSerializer.Serialize(new { suspension.StudentId, dto.NewBusId, dto.Notes })
            );
            await _unitOfWork.AuditLogs.AddAsync(auditLog, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(new StudentSuspensionDto
            {
                Id = suspension.Id,
                StudentId = suspension.StudentId,
                StudentName = student.StudentName,
                BusId = suspension.BusId,
                Reason = suspension.Reason,
                SuspendedAt = suspension.SuspendedAt,
                IsReactivated = true,
                ReactivatedAt = suspension.ReactivatedAt,
                NewBusIdAfterReactivation = suspension.NewBusIdAfterReactivation,
                NewBusNumberAfterReactivation = newBus?.BusNumber,
                ReactivationNotes = suspension.ReactivationNotes
            });
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<Result<IReadOnlyList<StudentSuspensionDto>>> GetSuspendedStudentsAsync(CancellationToken cancellationToken = default)
    {
        var suspensions = await _unitOfWork.StudentSuspensions.GetActiveSuspensionsAsync(cancellationToken);
        var result = suspensions.Select(s => new StudentSuspensionDto
        {
            Id = s.Id,
            StudentId = s.StudentId,
            StudentName = s.Student?.StudentName,
            BusId = s.BusId,
            BusNumber = s.Bus?.BusNumber,
            Reason = s.Reason,
            SuspendedAt = s.SuspendedAt,
            IsReactivated = s.IsReactivated
        }).ToList();

        return Result.Success<IReadOnlyList<StudentSuspensionDto>>(result);
    }

    public async Task<Result<IReadOnlyList<StudentSuspensionDto>>> GetStudentSuspensionHistoryAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var suspensions = await _unitOfWork.StudentSuspensions.GetByStudentIdAsync(studentId, cancellationToken);
        var result = suspensions.Select(s => new StudentSuspensionDto
        {
            Id = s.Id,
            StudentId = s.StudentId,
            BusId = s.BusId,
            BusNumber = s.Bus?.BusNumber,
            Reason = s.Reason,
            SuspendedAt = s.SuspendedAt,
            IsReactivated = s.IsReactivated,
            ReactivatedAt = s.ReactivatedAt,
            NewBusIdAfterReactivation = s.NewBusIdAfterReactivation,
            ReactivationNotes = s.ReactivationNotes
        }).ToList();

        return Result.Success<IReadOnlyList<StudentSuspensionDto>>(result);
    }

    #endregion

    #region Leave Operations

    public async Task<Result<StudentLeaveDto>> CreateLeaveAsync(CreateLeaveDto dto, Guid createdBy, CancellationToken cancellationToken = default)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(dto.StudentId, cancellationToken);
        if (student == null)
            return Result.Failure<StudentLeaveDto>("الطالب غير موجود");

        if (dto.EndDate < dto.StartDate)
            return Result.Failure<StudentLeaveDto>("تاريخ الانتهاء يجب أن يكون بعد تاريخ البداية");

        var hasOverlapping = await _unitOfWork.StudentLeaves.HasOverlappingLeaveAsync(dto.StudentId, dto.StartDate, dto.EndDate, cancellationToken);
        if (hasOverlapping)
            return Result.Failure<StudentLeaveDto>("يوجد تعارض مع إجازة أخرى في نفس الفترة");

        var leave = StudentLeave.Create(
            studentId: dto.StudentId,
            startDate: dto.StartDate,
            endDate: dto.EndDate,
            reason: dto.Reason,
            createdBy: createdBy,
            attachmentUrl: dto.AttachmentUrl,
            attachmentFileName: dto.AttachmentFileName
        );

        await _unitOfWork.StudentLeaves.AddAsync(leave, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(MapToLeaveDto(leave, student.StudentName));
    }

    public async Task<Result<StudentLeaveDto>> ApproveLeaveAsync(Guid leaveId, Guid approvedBy, CancellationToken cancellationToken = default)
    {
        var leave = await _unitOfWork.StudentLeaves.GetByIdAsync(leaveId, cancellationToken);
        if (leave == null)
            return Result.Failure<StudentLeaveDto>("سجل الإجازة غير موجود");

        if (leave.IsApproved)
            return Result.Failure<StudentLeaveDto>("تمت الموافقة على الإجازة مسبقاً");

        if (leave.IsCancelled)
            return Result.Failure<StudentLeaveDto>("تم إلغاء الإجازة");

        var student = await _unitOfWork.Students.GetByIdAsync(leave.StudentId, cancellationToken);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            leave.Approve(approvedBy);
            _unitOfWork.StudentLeaves.Update(leave);

            // Update student status if leave is active now
            if (leave.IsActive() && student != null)
            {
                student.SetOnLeave();
                _unitOfWork.Students.Update(student);
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(MapToLeaveDto(leave, student?.StudentName));
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<Result<StudentLeaveDto>> CancelLeaveAsync(Guid leaveId, CancelLeaveDto dto, Guid cancelledBy, CancellationToken cancellationToken = default)
    {
        var leave = await _unitOfWork.StudentLeaves.GetByIdAsync(leaveId, cancellationToken);
        if (leave == null)
            return Result.Failure<StudentLeaveDto>("سجل الإجازة غير موجود");

        if (leave.IsCancelled)
            return Result.Failure<StudentLeaveDto>("تم إلغاء الإجازة مسبقاً");

        var student = await _unitOfWork.Students.GetByIdAsync(leave.StudentId, cancellationToken);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var wasActive = leave.IsActive();
            leave.Cancel(cancelledBy, dto.Reason);
            _unitOfWork.StudentLeaves.Update(leave);

            // Reactivate student if leave was active
            if (wasActive && student != null && student.Status == StudentStatus.OnLeave)
            {
                student.Activate();
                _unitOfWork.Students.Update(student);
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(MapToLeaveDto(leave, student?.StudentName));
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<Result<IReadOnlyList<StudentLeaveDto>>> GetPendingLeavesAsync(CancellationToken cancellationToken = default)
    {
        var leaves = await _unitOfWork.StudentLeaves.GetPendingLeavesAsync(cancellationToken);
        var result = leaves.Select(l => MapToLeaveDto(l, l.Student?.StudentName)).ToList();
        return Result.Success<IReadOnlyList<StudentLeaveDto>>(result);
    }

    public async Task<Result<IReadOnlyList<StudentLeaveDto>>> GetStudentLeaveHistoryAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var leaves = await _unitOfWork.StudentLeaves.GetByStudentIdAsync(studentId, cancellationToken);
        var result = leaves.Select(l => MapToLeaveDto(l, l.Student?.StudentName)).ToList();
        return Result.Success<IReadOnlyList<StudentLeaveDto>>(result);
    }

    #endregion

    #region Transfer Operations

    public async Task<Result<StudentTransferDto>> TransferStudentAsync(CreateTransferDto dto, Guid transferredBy, CancellationToken cancellationToken = default)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(dto.StudentId, cancellationToken);
        if (student == null)
            return Result.Failure<StudentTransferDto>("الطالب غير موجود");

        var toBus = await _unitOfWork.Buses.GetByIdAsync(dto.ToBusId, cancellationToken);
        if (toBus == null)
            return Result.Failure<StudentTransferDto>("الباص المستهدف غير موجود");

        var currentAssignment = await _unitOfWork.StudentBusAssignments.GetActiveByStudentIdAsync(dto.StudentId, cancellationToken);
        if (currentAssignment == null)
            return Result.Failure<StudentTransferDto>("الطالب ليس لديه تعيين حالي للنقل منه");

        var fromBus = await _unitOfWork.Buses.GetByIdAsync(currentAssignment.BusId, cancellationToken);

        if (currentAssignment.BusId == dto.ToBusId)
            return Result.Failure<StudentTransferDto>("الطالب موجود بالفعل في هذا الباص");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Create transfer record
            var transfer = StudentTransfer.Create(
                studentId: dto.StudentId,
                fromBusId: currentAssignment.BusId,
                toBusId: dto.ToBusId,
                transferredBy: transferredBy,
                reason: dto.Reason,
                effectiveDate: dto.EffectiveDate
            );

            await _unitOfWork.StudentTransfers.AddAsync(transfer, cancellationToken);

            // Deactivate old assignment
            currentAssignment.Deactivate();
            _unitOfWork.StudentBusAssignments.Update(currentAssignment);

            // Create new assignment
            var newAssignment = StudentBusAssignment.Create(
                studentId: dto.StudentId,
                busId: dto.ToBusId,
                assignedBy: transferredBy
            );
            await _unitOfWork.StudentBusAssignments.AddAsync(newAssignment, cancellationToken);

            var auditLog = AuditLog.Create(
                action: "StudentTransferred",
                userId: transferredBy,
                entityType: "StudentTransfer",
                entityId: transfer.Id,
                newValues: System.Text.Json.JsonSerializer.Serialize(new { dto.StudentId, FromBusId = currentAssignment.BusId, dto.ToBusId, dto.Reason })
            );
            await _unitOfWork.AuditLogs.AddAsync(auditLog, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(new StudentTransferDto
            {
                Id = transfer.Id,
                StudentId = transfer.StudentId,
                StudentName = student.StudentName,
                FromBusId = transfer.FromBusId,
                FromBusNumber = fromBus?.BusNumber,
                ToBusId = transfer.ToBusId,
                ToBusNumber = toBus.BusNumber,
                Reason = transfer.Reason,
                TransferredAt = transfer.TransferredAt,
                EffectiveDate = transfer.EffectiveDate
            });
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<Result<IReadOnlyList<StudentTransferDto>>> GetStudentTransferHistoryAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var transfers = await _unitOfWork.StudentTransfers.GetByStudentIdAsync(studentId, cancellationToken);
        var result = transfers.Select(t => new StudentTransferDto
        {
            Id = t.Id,
            StudentId = t.StudentId,
            StudentName = t.Student?.StudentName,
            FromBusId = t.FromBusId,
            FromBusNumber = t.FromBus?.BusNumber,
            ToBusId = t.ToBusId,
            ToBusNumber = t.ToBus?.BusNumber,
            Reason = t.Reason,
            TransferredAt = t.TransferredAt,
            EffectiveDate = t.EffectiveDate
        }).ToList();

        return Result.Success<IReadOnlyList<StudentTransferDto>>(result);
    }

    #endregion

    #region Timeline

    public async Task<Result<StudentTimelineDto>> GetStudentTimelineAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(studentId, cancellationToken);
        if (student == null)
            return Result.Failure<StudentTimelineDto>("الطالب غير موجود");

        var events = new List<TimelineEventDto>();

        // Get suspensions
        var suspensions = await _unitOfWork.StudentSuspensions.GetByStudentIdAsync(studentId, cancellationToken);
        foreach (var s in suspensions)
        {
            events.Add(new TimelineEventDto
            {
                Id = s.Id,
                EventType = "Suspension",
                Description = $"تم إيقاف الطالب: {s.Reason}",
                OccurredAt = s.SuspendedAt,
                Details = new Dictionary<string, object?> { ["reason"] = s.Reason, ["busId"] = s.BusId }
            });

            if (s.IsReactivated && s.ReactivatedAt.HasValue)
            {
                events.Add(new TimelineEventDto
                {
                    Id = s.Id,
                    EventType = "Reactivation",
                    Description = "تم إعادة تفعيل الطالب",
                    OccurredAt = s.ReactivatedAt.Value,
                    Details = new Dictionary<string, object?> { ["newBusId"] = s.NewBusIdAfterReactivation, ["notes"] = s.ReactivationNotes }
                });
            }
        }

        // Get leaves
        var leaves = await _unitOfWork.StudentLeaves.GetByStudentIdAsync(studentId, cancellationToken);
        foreach (var l in leaves)
        {
            events.Add(new TimelineEventDto
            {
                Id = l.Id,
                EventType = "Leave",
                Description = $"إجازة: {l.Reason}",
                OccurredAt = l.CreatedAt,
                Details = new Dictionary<string, object?> 
                { 
                    ["startDate"] = l.StartDate, 
                    ["endDate"] = l.EndDate, 
                    ["isApproved"] = l.IsApproved,
                    ["isCancelled"] = l.IsCancelled
                }
            });
        }

        // Get transfers
        var transfers = await _unitOfWork.StudentTransfers.GetByStudentIdAsync(studentId, cancellationToken);
        foreach (var t in transfers)
        {
            events.Add(new TimelineEventDto
            {
                Id = t.Id,
                EventType = "Transfer",
                Description = $"نقل من باص {t.FromBus?.BusNumber} إلى باص {t.ToBus?.BusNumber}",
                OccurredAt = t.TransferredAt,
                Details = new Dictionary<string, object?> 
                { 
                    ["fromBusId"] = t.FromBusId, 
                    ["toBusId"] = t.ToBusId, 
                    ["reason"] = t.Reason 
                }
            });
        }

        // Sort by date descending
        events = events.OrderByDescending(e => e.OccurredAt).ToList();

        return Result.Success(new StudentTimelineDto
        {
            StudentId = studentId,
            StudentName = student.StudentName,
            Events = events
        });
    }

    #endregion

    #region Private Helpers

    private static StudentAssignmentDto MapToAssignmentDto(StudentBusAssignment assignment, Student? student, Bus? bus)
    {
        return new StudentAssignmentDto
        {
            Id = assignment.Id,
            StudentId = assignment.StudentId,
            StudentName = student?.StudentName,
            BusId = assignment.BusId,
            BusNumber = bus?.BusNumber,
            TransportType = assignment.TransportType.ToString(),
            ArrivalBusId = assignment.ArrivalBusId,
            ReturnBusId = assignment.ReturnBusId,
            IsActive = assignment.IsActive,
            AssignedAt = assignment.AssignedAt
        };
    }

    private static StudentLeaveDto MapToLeaveDto(StudentLeave leave, string? studentName)
    {
        return new StudentLeaveDto
        {
            Id = leave.Id,
            StudentId = leave.StudentId,
            StudentName = studentName,
            StartDate = leave.StartDate,
            EndDate = leave.EndDate,
            Reason = leave.Reason,
            AttachmentUrl = leave.AttachmentUrl,
            AttachmentFileName = leave.AttachmentFileName,
            IsApproved = leave.IsApproved,
            ApprovedAt = leave.ApprovedAt,
            IsCancelled = leave.IsCancelled,
            CancelledAt = leave.CancelledAt,
            CancellationReason = leave.CancellationReason,
            CreatedAt = leave.CreatedAt
        };
    }

    #endregion
}
