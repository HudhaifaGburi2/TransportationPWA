using AutoMapper;
using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.Route;
using TransportationAttendance.Application.Interfaces.Services;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Interfaces;

namespace TransportationAttendance.Application.Services;

public class RouteService : IRouteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RouteService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<RouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var routes = await _unitOfWork.Routes.GetAllAsync(cancellationToken);
        var dtos = routes.Select(MapToDto).ToList();
        return Result.Success<IReadOnlyList<RouteDto>>(dtos);
    }

    public async Task<Result<RouteDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var route = await _unitOfWork.Routes.GetWithBusesAsync(id, cancellationToken);
        if (route == null)
            return Result.Failure<RouteDto>("المسار غير موجود");

        return MapToDto(route);
    }

    public async Task<Result<RouteDto>> CreateAsync(CreateRouteDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _unitOfWork.Routes.GetByNameAsync(dto.RouteName, cancellationToken);
        if (existing != null)
            return Result.Failure<RouteDto>("اسم المسار موجود مسبقاً");

        var route = Route.Create(dto.RouteName, dto.RouteDescription);

        await _unitOfWork.Routes.AddAsync(route, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(route);
    }

    public async Task<Result<RouteDto>> UpdateAsync(Guid id, UpdateRouteDto dto, CancellationToken cancellationToken = default)
    {
        var route = await _unitOfWork.Routes.GetByIdAsync(id, cancellationToken);
        if (route == null)
            return Result.Failure<RouteDto>("المسار غير موجود");

        var existing = await _unitOfWork.Routes.GetByNameAsync(dto.RouteName, cancellationToken);
        if (existing != null && existing.Id != id)
            return Result.Failure<RouteDto>("اسم المسار موجود مسبقاً");

        route.Update(dto.RouteName, dto.RouteDescription);

        if (dto.IsActive)
            route.Activate();
        else
            route.Deactivate();

        _unitOfWork.Routes.Update(route);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(route);
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var route = await _unitOfWork.Routes.GetWithBusesAsync(id, cancellationToken);
        if (route == null)
            return Result.Failure<bool>("المسار غير موجود");

        _unitOfWork.Routes.Remove(route);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    private RouteDto MapToDto(Route route)
    {
        return new RouteDto
        {
            RouteId = route.Id,
            RouteName = route.RouteName,
            RouteDescription = route.RouteDescription,
            IsActive = route.IsActive,
            BusCount = 0
        };
    }
}
