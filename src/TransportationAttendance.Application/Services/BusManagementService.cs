using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.BusManagement;
using TransportationAttendance.Application.Interfaces.Services;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Application.Services;

public class BusManagementService : IBusManagementService
{
    private readonly IDriverRepository _driverRepository;
    private readonly IRouteRepository _routeRepository;
    private readonly IBusRepository _busRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BusManagementService(
        IDriverRepository driverRepository,
        IRouteRepository routeRepository,
        IBusRepository busRepository,
        IUnitOfWork unitOfWork)
    {
        _driverRepository = driverRepository;
        _routeRepository = routeRepository;
        _busRepository = busRepository;
        _unitOfWork = unitOfWork;
    }

    #region Driver Operations

    public async Task<Result<IReadOnlyList<DriverManagementDto>>> GetAllDriversAsync(DriverQueryDto? query = null, CancellationToken cancellationToken = default)
    {
        var drivers = await _driverRepository.GetAllAsync(cancellationToken);

        if (query != null)
        {
            var filtered = drivers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query.Search))
                filtered = filtered.Where(d => d.FullName.Contains(query.Search, StringComparison.OrdinalIgnoreCase) ||
                                               d.PhoneNumber.Contains(query.Search));

            if (query.IsActive.HasValue)
                filtered = filtered.Where(d => d.IsActive == query.IsActive.Value);

            drivers = filtered.ToList();
        }

        var dtos = drivers.Select(MapDriverToDto).ToList();
        return Result.Success<IReadOnlyList<DriverManagementDto>>(dtos);
    }

    public async Task<Result<DriverManagementDto>> GetDriverByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var driver = await _driverRepository.GetByIdAsync(id, cancellationToken);
        if (driver == null)
            return Result.Failure<DriverManagementDto>("السائق غير موجود");

        return MapDriverToDto(driver);
    }

    public async Task<Result<DriverManagementDto>> CreateDriverAsync(CreateDriverManagementDto dto, CancellationToken cancellationToken = default)
    {
        var existingByPhone = await _driverRepository.GetByPhoneNumberAsync(dto.PhoneNumber, cancellationToken);
        if (existingByPhone != null)
            return Result.Failure<DriverManagementDto>("رقم الهاتف مسجل مسبقاً");

        var driver = Driver.Create(
            dto.FullName,
            dto.PhoneNumber,
            $"DRV-{Guid.NewGuid():N}".Substring(0, 20),
            DateTime.UtcNow.AddYears(5),
            null);

        await _driverRepository.AddAsync(driver, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapDriverToDto(driver);
    }

    public async Task<Result<DriverManagementDto>> UpdateDriverAsync(Guid id, UpdateDriverManagementDto dto, CancellationToken cancellationToken = default)
    {
        var driver = await _driverRepository.GetByIdAsync(id, cancellationToken);
        if (driver == null)
            return Result.Failure<DriverManagementDto>("السائق غير موجود");

        var existingByPhone = await _driverRepository.GetByPhoneNumberAsync(dto.PhoneNumber, cancellationToken);
        if (existingByPhone != null && existingByPhone.Id != id)
            return Result.Failure<DriverManagementDto>("رقم الهاتف مسجل مسبقاً");

        driver.Update(dto.FullName, dto.PhoneNumber, driver.LicenseNumber, driver.LicenseExpiryDate, driver.EmployeeId);
        
        if (dto.IsActive)
            driver.Activate();
        else
            driver.Deactivate();

        _driverRepository.Update(driver);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapDriverToDto(driver);
    }

    public async Task<Result<bool>> DeleteDriverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var driver = await _driverRepository.GetByIdAsync(id, cancellationToken);
        if (driver == null)
            return Result.Failure<bool>("السائق غير موجود");

        _driverRepository.Remove(driver);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion

    #region Route Operations

    public async Task<Result<IReadOnlyList<RouteManagementDto>>> GetAllRoutesAsync(RouteQueryDto? query = null, CancellationToken cancellationToken = default)
    {
        var routes = await _routeRepository.GetAllAsync(cancellationToken);

        if (query != null)
        {
            var filtered = routes.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query.Search))
                filtered = filtered.Where(r => r.Name.Contains(query.Search, StringComparison.OrdinalIgnoreCase) ||
                                               (r.Description != null && r.Description.Contains(query.Search, StringComparison.OrdinalIgnoreCase)));

            if (query.IsActive.HasValue)
                filtered = filtered.Where(r => r.IsActive == query.IsActive.Value);

            routes = filtered.ToList();
        }

        var dtos = routes.Select(MapRouteToDto).ToList();
        return Result.Success<IReadOnlyList<RouteManagementDto>>(dtos);
    }

    public async Task<Result<RouteManagementDto>> GetRouteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var route = await _routeRepository.GetByIdAsync(id, cancellationToken);
        if (route == null)
            return Result.Failure<RouteManagementDto>("المسار غير موجود");

        return MapRouteToDto(route);
    }

    public async Task<Result<RouteManagementDto>> CreateRouteAsync(CreateRouteManagementDto dto, CancellationToken cancellationToken = default)
    {
        var route = Route.Create(dto.Name, dto.Description);

        await _routeRepository.AddAsync(route, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapRouteToDto(route);
    }

    public async Task<Result<RouteManagementDto>> UpdateRouteAsync(Guid id, UpdateRouteManagementDto dto, CancellationToken cancellationToken = default)
    {
        var route = await _routeRepository.GetByIdAsync(id, cancellationToken);
        if (route == null)
            return Result.Failure<RouteManagementDto>("المسار غير موجود");

        route.Update(dto.Name, dto.Description);

        if (dto.IsActive)
            route.Activate();
        else
            route.Deactivate();

        _routeRepository.Update(route);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapRouteToDto(route);
    }

    public async Task<Result<bool>> DeleteRouteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var route = await _routeRepository.GetByIdAsync(id, cancellationToken);
        if (route == null)
            return Result.Failure<bool>("المسار غير موجود");

        _routeRepository.Remove(route);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion

    #region Bus Operations

    public async Task<Result<IReadOnlyList<BusManagementDto>>> GetAllBusesAsync(BusManagementQueryDto? query = null, CancellationToken cancellationToken = default)
    {
        var buses = await _busRepository.GetAllAsync(cancellationToken);

        if (query != null)
        {
            var filtered = buses.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query.Search))
                filtered = filtered.Where(b => (b.BusNumber != null && b.BusNumber.Contains(query.Search, StringComparison.OrdinalIgnoreCase)) ||
                                               (b.DriverName != null && b.DriverName.Contains(query.Search, StringComparison.OrdinalIgnoreCase)));

            if (query.IsActive.HasValue)
                filtered = filtered.Where(b => b.IsActive == query.IsActive.Value);

            buses = filtered.ToList();
        }

        var dtos = buses.Select(MapBusToDto).ToList();
        return Result.Success<IReadOnlyList<BusManagementDto>>(dtos);
    }

    public async Task<Result<BusManagementDto>> GetBusByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var bus = await _busRepository.GetByIdAsync(id, cancellationToken);
        if (bus == null)
            return Result.Failure<BusManagementDto>("الباص غير موجود");

        return MapBusToDto(bus);
    }

    public async Task<Result<BusManagementDto>> CreateBusAsync(CreateBusManagementDto dto, CancellationToken cancellationToken = default)
    {
        var existingByNumber = await _busRepository.GetByBusNumberAsync(dto.BusNumber, cancellationToken);
        if (existingByNumber != null)
            return Result.Failure<BusManagementDto>("رقم الباص مسجل مسبقاً");

        var bus = Bus.Create(
            dto.BusNumber,
            dto.PeriodId,
            dto.Capacity,
            dto.DriverName,
            dto.DriverPhoneNumber);

        if (dto.RouteId.HasValue)
            bus.AssignRoute(dto.RouteId);

        await _busRepository.AddAsync(bus, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapBusToDto(bus);
    }

    public async Task<Result<BusManagementDto>> UpdateBusAsync(Guid id, UpdateBusManagementDto dto, CancellationToken cancellationToken = default)
    {
        var bus = await _busRepository.GetByIdAsync(id, cancellationToken);
        if (bus == null)
            return Result.Failure<BusManagementDto>("الباص غير موجود");

        var existingByNumber = await _busRepository.GetByBusNumberAsync(dto.BusNumber, cancellationToken);
        if (existingByNumber != null && existingByNumber.Id != id)
            return Result.Failure<BusManagementDto>("رقم الباص مسجل مسبقاً");

        bus.Update(dto.BusNumber, dto.PeriodId, dto.Capacity, dto.DriverName, dto.DriverPhoneNumber);
        bus.AssignRoute(dto.RouteId);

        if (dto.IsActive)
            bus.Activate();
        else
            bus.Deactivate();

        _busRepository.Update(bus);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapBusToDto(bus);
    }

    public async Task<Result<bool>> DeleteBusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var bus = await _busRepository.GetByIdAsync(id, cancellationToken);
        if (bus == null)
            return Result.Failure<bool>("الباص غير موجود");

        _busRepository.Remove(bus);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion

    #region Statistics

    public async Task<Result<BusManagementStatisticsDto>> GetStatisticsAsync(CancellationToken cancellationToken = default)
    {
        var routes = await _routeRepository.GetAllAsync(cancellationToken);
        var buses = await _busRepository.GetAllAsync(cancellationToken);

        // Count drivers from buses that have driver names
        var driversWithNames = buses.Where(b => !string.IsNullOrEmpty(b.DriverName)).ToList();

        var stats = new BusManagementStatisticsDto
        {
            TotalBuses = buses.Count,
            ActiveBuses = buses.Count(b => b.IsActive),
            InactiveBuses = buses.Count(b => !b.IsActive),
            TotalCapacity = buses.Where(b => b.IsActive).Sum(b => b.Capacity),
            TotalDrivers = driversWithNames.Count,
            ActiveDrivers = driversWithNames.Count(b => b.IsActive),
            TotalRoutes = routes.Count,
            ActiveRoutes = routes.Count(r => r.IsActive)
        };

        return stats;
    }

    #endregion

    #region Mapping Helpers

    private static DriverManagementDto MapDriverToDto(Driver driver) => new()
    {
        Id = driver.Id,
        FullName = driver.FullName,
        PhoneNumber = driver.PhoneNumber,
        IsActive = driver.IsActive,
        CreatedAt = driver.CreatedAt
    };

    private static RouteManagementDto MapRouteToDto(Route route) => new()
    {
        Id = route.Id,
        Name = route.Name,
        Description = route.Description,
        IsActive = route.IsActive,
        CreatedAt = route.CreatedAt
    };

    private static BusManagementDto MapBusToDto(Bus bus) => new()
    {
        Id = bus.Id,
        BusNumber = bus.BusNumber,
        PeriodId = bus.PeriodId,
        RouteId = bus.RouteId,
        DriverName = bus.DriverName,
        DriverPhoneNumber = bus.DriverPhoneNumber,
        Capacity = bus.Capacity,
        IsActive = bus.IsActive,
        IsMerged = bus.IsMerged,
        CreatedAt = bus.CreatedAt
    };

    #endregion

    public async Task<Result<BusStudentsDto>> GetBusStudentsAsync(Guid busId, CancellationToken cancellationToken = default)
    {
        var bus = await _busRepository.GetByIdAsync(busId, cancellationToken);
        if (bus == null)
            return Result.Failure<BusStudentsDto>("الباص غير موجود");

        var assignments = await _unitOfWork.StudentBusAssignments.GetActiveByBusIdAsync(busId, cancellationToken);

        var students = assignments.Select(a => new BusStudentDto
        {
            Id = a.StudentId,
            StudentId = a.Student?.StudentId ?? "",
            StudentName = a.Student?.StudentName ?? "",
            DistrictName = a.Student?.District?.DistrictNameAr,
            PeriodName = null,
            AssignedAt = a.AssignedAt
        }).ToList();

        return Result.Success(new BusStudentsDto
        {
            BusId = bus.Id,
            BusNumber = bus.BusNumber,
            Capacity = bus.Capacity,
            Students = students
        });
    }
}
