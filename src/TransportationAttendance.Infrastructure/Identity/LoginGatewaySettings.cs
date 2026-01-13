namespace TransportationAttendance.Infrastructure.Identity;

public class LoginGatewaySettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string BearerToken { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 30;
}
