using System.Security.Claims;
using TransportationAttendance.Application.Interfaces;

namespace TransportationAttendance.API.Services;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("userId");
    
    public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name
                               ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("username");
    
    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public IEnumerable<string> Roles => _httpContextAccessor.HttpContext?.User?
        .FindAll(ClaimTypes.Role)?.Select(c => c.Value) 
        ?? _httpContextAccessor.HttpContext?.User?.FindAll("role")?.Select(c => c.Value)
        ?? Enumerable.Empty<string>();

    public bool IsInRole(string role) => _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
}
