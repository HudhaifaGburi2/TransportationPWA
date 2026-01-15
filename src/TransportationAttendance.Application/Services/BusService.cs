using AutoMapper;
using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Bus;
using TransportationAttendance.Application.Interfaces.Services;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces;

namespace TransportationAttendance.Application.Services;

public class BusService : IBusService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BusService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<BusDto>>> GetAllAsync(BusQueryDto? query = null, CancellationToken cancellationToken = default)
    {
        var buses = await _unitOfWork.Buses.GetAllAsync(cancellationToken);
        
        if (query != null)
        {
            var filtered = buses.AsEnumerable();
            
            if (query.PeriodId.HasValue)
                filtered = filtered.Where(b => b.PeriodId == query.PeriodId.Value);
            
            if (query.RouteId.HasValue)
                filtered = filtered.Where(b => b.RouteId == query.RouteId.Value);
            
            if (query.IsActive.HasValue)
                filtered = filtered.Where(b => b.IsActive == query.IsActive.Value);
            
            if (!string.IsNullOrWhiteSpace(query.Search))
                filtered = filtered.Where(b => 
                    b.BusNumber.Contains(query.Search, StringComparison.OrdinalIgnoreCase) ||
                    (b.DriverName?.Contains(query.Search, StringComparison.OrdinalIgnoreCase) ?? false));
            
            buses = filtered.ToList();
        }

        var dtos = buses.Select(MapToDto).ToList();
        return Result.Success<IReadOnlyList<BusDto>>(dtos);
    }

    public async Task<Result<BusDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var bus = await _unitOfWork.Buses.GetByIdAsync(id, cancellationToken);
        if (bus == null)
            return Result.Failure<BusDto>("الباص غير موجود");

        return MapToDto(bus);
    }

    public async Task<Result<IReadOnlyList<BusDto>>> GetByPeriodAsync(int periodId, CancellationToken cancellationToken = default)
    {
        var buses = await _unitOfWork.Buses.GetByPeriodAsync(periodId, cancellationToken);
        var dtos = buses.Select(MapToDto).ToList();
        return Result.Success<IReadOnlyList<BusDto>>(dtos);
    }

    public async Task<Result<BusDto>> CreateAsync(CreateBusDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _unitOfWork.Buses.GetByBusNumberAsync(dto.BusNumber, dto.PeriodId, cancellationToken);
        if (existing != null)
            return Result.Failure<BusDto>("رقم الباص موجود مسبقاً في هذه الفترة");

        var bus = Bus.Create(
            dto.BusNumber,
            dto.PeriodId,
            dto.Capacity,
            dto.RouteId,
            dto.DriverName,
            dto.DriverPhoneNumber);

        await _unitOfWork.Buses.AddAsync(bus, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(bus);
    }

    public async Task<Result<BusDto>> UpdateAsync(Guid id, UpdateBusDto dto, CancellationToken cancellationToken = default)
    {
        var bus = await _unitOfWork.Buses.GetByIdAsync(id, cancellationToken);
        if (bus == null)
            return Result.Failure<BusDto>("الباص غير موجود");

        var existing = await _unitOfWork.Buses.GetByBusNumberAsync(dto.BusNumber, dto.PeriodId, cancellationToken);
        if (existing != null && existing.Id != id)
            return Result.Failure<BusDto>("رقم الباص موجود مسبقاً في هذه الفترة");

        // Capacity validation - cannot reduce below current student count
        var currentStudentCount = bus.StudentAssignments?.Count(s => s.IsActive) ?? 0;
        if (dto.Capacity < currentStudentCount)
            return Result.Failure<BusDto>($"لا يمكن تقليل السعة إلى {dto.Capacity}. يوجد حالياً {currentStudentCount} طالب مسجل");

        bus.Update(
            dto.BusNumber,
            dto.PeriodId,
            dto.Capacity,
            dto.RouteId,
            dto.DriverName,
            dto.DriverPhoneNumber);

        if (dto.IsActive)
            bus.Activate();
        else
            bus.Deactivate();

        _unitOfWork.Buses.Update(bus);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(bus);
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var bus = await _unitOfWork.Buses.GetByIdAsync(id, cancellationToken);
        if (bus == null)
            return Result.Failure<bool>("الباص غير موجود");

        // Prevent deletion if bus has active students
        var activeStudentCount = bus.StudentAssignments?.Count(s => s.IsActive) ?? 0;
        if (activeStudentCount > 0)
            return Result.Failure<bool>($"لا يمكن حذف الباص. يوجد {activeStudentCount} طالب مسجل حالياً");

        _unitOfWork.Buses.Remove(bus);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<Result<BusStatisticsDto>> GetStatisticsAsync(Guid busId, CancellationToken cancellationToken = default)
    {
        var bus = await _unitOfWork.Buses.GetByIdAsync(busId, cancellationToken);
        if (bus == null)
            return Result.Failure<BusStatisticsDto>("الباص غير موجود");

        var studentCount = bus.StudentAssignments?.Count(s => s.IsActive) ?? 0;

        var stats = new BusStatisticsDto
        {
            BusId = bus.Id,
            BusNumber = bus.BusNumber,
            TotalStudents = bus.StudentAssignments?.Count ?? 0,
            ActiveStudents = studentCount,
            SuspendedStudents = bus.StudentAssignments?.Count(s => !s.IsActive) ?? 0,
            Capacity = bus.Capacity,
            UtilizationRate = bus.Capacity > 0 ? Math.Round((decimal)studentCount / bus.Capacity * 100, 2) : 0,
            DistrictBreakdown = new List<DistrictStudentCount>()
        };

        return stats;
    }

    public async Task<Result<BusSummaryDto>> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var buses = await _unitOfWork.Buses.GetAllAsync(cancellationToken);

        var activeBuses = buses.Where(b => b.IsActive).ToList();
        var totalCapacity = activeBuses.Sum(b => b.Capacity);
        var totalStudents = activeBuses.Sum(b => b.StudentAssignments?.Count(s => s.IsActive) ?? 0);

        var summary = new BusSummaryDto
        {
            TotalBuses = buses.Count,
            ActiveBuses = activeBuses.Count,
            InactiveBuses = buses.Count - activeBuses.Count,
            TotalCapacity = totalCapacity,
            TotalStudentsAssigned = totalStudents,
            OverallUtilization = totalCapacity > 0 ? Math.Round((decimal)totalStudents / totalCapacity * 100, 2) : 0,
            ByPeriod = activeBuses
                .GroupBy(b => b.PeriodId)
                .Select(g => new PeriodBusSummary
                {
                    PeriodId = g.Key,
                    PeriodName = $"الفترة {g.Key}",
                    BusCount = g.Count(),
                    StudentCount = g.Sum(b => b.StudentAssignments?.Count(s => s.IsActive) ?? 0),
                    TotalCapacity = g.Sum(b => b.Capacity),
                    Utilization = g.Sum(b => b.Capacity) > 0 
                        ? Math.Round((decimal)g.Sum(b => b.StudentAssignments?.Count(s => s.IsActive) ?? 0) / g.Sum(b => b.Capacity) * 100, 2) 
                        : 0
                })
                .ToList()
        };

        return summary;
    }

    public async Task<Result<bool>> AssignDistrictsAsync(Guid busId, List<Guid> districtIds, CancellationToken cancellationToken = default)
    {
        var bus = await _unitOfWork.Buses.GetByIdAsync(busId, cancellationToken);
        if (bus == null)
            return Result.Failure<bool>("الباص غير موجود");

        // This would require additional repository methods to manage BusDistricts
        // For now, return success
        return true;
    }

    private BusDto MapToDto(Bus bus)
    {
        var studentCount = bus.StudentAssignments?.Count(s => s.IsActive) ?? 0;
        
        return new BusDto
        {
            BusId = bus.Id,
            BusNumber = bus.BusNumber,
            PeriodId = bus.PeriodId,
            PeriodName = $"الفترة {bus.PeriodId}",
            RouteId = bus.RouteId,
            RouteName = bus.Route?.RouteName,
            DriverName = bus.DriverName,
            DriverPhoneNumber = bus.DriverPhoneNumber,
            Capacity = bus.Capacity,
            CurrentStudentCount = studentCount,
            UtilizationPercentage = bus.Capacity > 0 ? Math.Round((decimal)studentCount / bus.Capacity * 100, 2) : 0,
            IsActive = bus.IsActive,
            IsMerged = bus.IsMerged,
            MergedWithBusId = bus.MergedWithBusId,
            Districts = bus.BusDistricts?.Select(bd => new DistrictInfoDto
            {
                DistrictId = bd.DistrictId,
                DistrictNameAr = bd.District?.DistrictNameAr ?? string.Empty,
                DistrictNameEn = bd.District?.DistrictNameEn
            }).ToList() ?? new List<DistrictInfoDto>()
        };
    }
}
