namespace TransportationAttendance.Application.DTOs.Route;

public record RouteDto
{
    public Guid RouteId { get; init; }
    public string RouteName { get; init; } = string.Empty;
    public string? RouteDescription { get; init; }
    public bool IsActive { get; init; }
    public int BusCount { get; init; }
}

public record CreateRouteDto
{
    public string RouteName { get; init; } = string.Empty;
    public string? RouteDescription { get; init; }
}

public record UpdateRouteDto
{
    public string RouteName { get; init; } = string.Empty;
    public string? RouteDescription { get; init; }
    public bool IsActive { get; init; } = true;
}
