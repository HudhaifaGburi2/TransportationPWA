namespace TransportationAttendance.Domain.Entities;

public class Route : BaseEntity
{
    public string RouteName { get; private set; } = string.Empty;
    public string? RouteDescription { get; private set; }
    public bool IsActive { get; private set; }

    private Route() { }

    public static Route Create(string routeName, string? routeDescription = null)
    {
        return new Route
        {
            RouteName = routeName,
            RouteDescription = routeDescription,
            IsActive = true
        };
    }

    public void Update(string routeName, string? routeDescription)
    {
        RouteName = routeName;
        RouteDescription = routeDescription;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
