using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.BusManagement;
using TransportationAttendance.Application.Interfaces.Services;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces;
using TransportationAttendance.Domain.Interfaces.Repositories;

namespace TransportationAttendance.Application.Services;

public class BusManagementService : IBusManagementService
{
    private readonly IActualDriverRepository _driverRepository;
    private readonly IActualRouteRepository _routeRepository;
    private readonly IActualBusRepository _busRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BusManagementService(
        IActualDriverRepository driverRepository,
        IActualRouteRepository routeRepository,
        IActualBusRepository busRepository,
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

            if (query.LicenseExpiringSoon == true)
                filtered = filtered.Where(d => d.LicenseExpiryDate <= DateTime.UtcNow.AddDays(30));

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

        var existingByLicense = await _driverRepository.GetByLicenseNumberAsync(dto.LicenseNumber, cancellationToken);
        if (existingByLicense != null)
            return Result.Failure<DriverManagementDto>("رقم الرخصة مسجل مسبقاً");

        var driver = ActualDriver.Create(
            dto.FullName,
            dto.PhoneNumber,
            dto.LicenseNumber,
            dto.LicenseExpiryDate,
            dto.EmployeeId);

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

        var existingByLicense = await _driverRepository.GetByLicenseNumberAsync(dto.LicenseNumber, cancellationToken);
        if (existingByLicense != null && existingByLicense.Id != id)
            return Result.Failure<DriverManagementDto>("رقم الرخصة مسجل مسبقاً");

        driver.Update(dto.FullName, dto.PhoneNumber, dto.LicenseNumber, dto.LicenseExpiryDate, dto.EmployeeId);
        
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
                                               r.Code.Contains(query.Search, StringComparison.OrdinalIgnoreCase) ||
                                               r.District.Contains(query.Search, StringComparison.OrdinalIgnoreCase));

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
        var existingByCode = await _routeRepository.GetByCodeAsync(dto.Code, cancellationToken);
        if (existingByCode != null)
            return Result.Failure<RouteManagementDto>("رمز المسار مسجل مسبقاً");

        var route = ActualRoute.Create(
            dto.Name,
            dto.Code,
            dto.District,
            dto.MeetingPoint,
            dto.PickupTime,
            dto.DropoffTime,
            dto.Capacity);

        if (dto.MeetingPointLatitude.HasValue && dto.MeetingPointLongitude.HasValue)
            route.SetLocation(dto.MeetingPointLatitude.Value, dto.MeetingPointLongitude.Value);

        await _routeRepository.AddAsync(route, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapRouteToDto(route);
    }

    public async Task<Result<RouteManagementDto>> UpdateRouteAsync(Guid id, UpdateRouteManagementDto dto, CancellationToken cancellationToken = default)
    {
        var route = await _routeRepository.GetByIdAsync(id, cancellationToken);
        if (route == null)
            return Result.Failure<RouteManagementDto>("المسار غير موجود");

        var existingByCode = await _routeRepository.GetByCodeAsync(dto.Code, cancellationToken);
        if (existingByCode != null && existingByCode.Id != id)
            return Result.Failure<RouteManagementDto>("رمز المسار مسجل مسبقاً");

        route.Update(dto.Name, dto.Code, dto.District, dto.MeetingPoint, dto.PickupTime, dto.DropoffTime, dto.Capacity);

        if (dto.MeetingPointLatitude.HasValue && dto.MeetingPointLongitude.HasValue)
            route.SetLocation(dto.MeetingPointLatitude.Value, dto.MeetingPointLongitude.Value);

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
                filtered = filtered.Where(b => b.BusNumber.Contains(query.Search, StringComparison.OrdinalIgnoreCase) ||
                                               b.LicensePlate.Contains(query.Search, StringComparison.OrdinalIgnoreCase));

            if (query.IsActive.HasValue)
                filtered = filtered.Where(b => b.IsActive == query.IsActive.Value);

            if (query.NeedsMaintenance == true)
                filtered = filtered.Where(b => b.NextMaintenanceDate.HasValue && b.NextMaintenanceDate.Value < DateTime.UtcNow);

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
        var existingByPlate = await _busRepository.GetByLicensePlateAsync(dto.LicensePlate, cancellationToken);
        if (existingByPlate != null)
            return Result.Failure<BusManagementDto>("رقم اللوحة مسجل مسبقاً");

        var bus = ActualBus.Create(
            dto.BusNumber,
            dto.LicensePlate,
            dto.Capacity,
            dto.Model,
            dto.Year);

        await _busRepository.AddAsync(bus, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapBusToDto(bus);
    }

    public async Task<Result<BusManagementDto>> UpdateBusAsync(Guid id, UpdateBusManagementDto dto, CancellationToken cancellationToken = default)
    {
        var bus = await _busRepository.GetByIdAsync(id, cancellationToken);
        if (bus == null)
            return Result.Failure<BusManagementDto>("الباص غير موجود");

        var existingByPlate = await _busRepository.GetByLicensePlateAsync(dto.LicensePlate, cancellationToken);
        if (existingByPlate != null && existingByPlate.Id != id)
            return Result.Failure<BusManagementDto>("رقم اللوحة مسجل مسبقاً");

        bus.Update(dto.BusNumber, dto.LicensePlate, dto.Capacity, dto.Model, dto.Year);
        bus.SetMaintenanceSchedule(dto.LastMaintenanceDate, dto.NextMaintenanceDate);

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
        var drivers = await _driverRepository.GetAllAsync(cancellationToken);
        var routes = await _routeRepository.GetAllAsync(cancellationToken);
        var buses = await _busRepository.GetAllAsync(cancellationToken);

        var stats = new BusManagementStatisticsDto
        {
            TotalBuses = buses.Count,
            ActiveBuses = buses.Count(b => b.IsActive),
            InactiveBuses = buses.Count(b => !b.IsActive),
            TotalCapacity = buses.Where(b => b.IsActive).Sum(b => b.Capacity),
            BusesNeedingMaintenance = buses.Count(b => b.NextMaintenanceDate.HasValue && b.NextMaintenanceDate.Value < DateTime.UtcNow),
            TotalDrivers = drivers.Count,
            ActiveDrivers = drivers.Count(d => d.IsActive),
            DriversWithExpiringLicense = drivers.Count(d => d.LicenseExpiryDate <= DateTime.UtcNow.AddDays(30)),
            TotalRoutes = routes.Count,
            ActiveRoutes = routes.Count(r => r.IsActive)
        };

        return stats;
    }

    #endregion

    #region Mapping Helpers

    private static DriverManagementDto MapDriverToDto(ActualDriver driver) => new()
    {
        Id = driver.Id,
        FullName = driver.FullName,
        PhoneNumber = driver.PhoneNumber,
        LicenseNumber = driver.LicenseNumber,
        LicenseExpiryDate = driver.LicenseExpiryDate,
        EmployeeId = driver.EmployeeId,
        IsActive = driver.IsActive,
        CreatedAt = driver.CreatedAt
    };

    private static RouteManagementDto MapRouteToDto(ActualRoute route) => new()
    {
        Id = route.Id,
        Name = route.Name,
        Code = route.Code,
        District = route.District,
        MeetingPoint = route.MeetingPoint,
        MeetingPointLatitude = route.MeetingPointLatitude,
        MeetingPointLongitude = route.MeetingPointLongitude,
        PickupTime = route.PickupTime,
        DropoffTime = route.DropoffTime,
        Capacity = route.Capacity,
        IsActive = route.IsActive,
        CreatedAt = route.CreatedAt
    };

    private static BusManagementDto MapBusToDto(ActualBus bus) => new()
    {
        Id = bus.Id,
        BusNumber = bus.BusNumber,
        LicensePlate = bus.LicensePlate,
        Capacity = bus.Capacity,
        Model = bus.Model,
        Year = bus.Year,
        IsActive = bus.IsActive,
        LastMaintenanceDate = bus.LastMaintenanceDate,
        NextMaintenanceDate = bus.NextMaintenanceDate,
        CreatedAt = bus.CreatedAt
    };

    #endregion
}
