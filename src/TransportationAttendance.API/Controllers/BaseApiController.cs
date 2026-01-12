using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace TransportationAttendance.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value 
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    protected Guid? GetCurrentUserGuid()
    {
        var userIdClaim = User.FindFirst("userId")?.Value 
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    protected IReadOnlyList<string> GetCurrentUserRoles()
    {
        return User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
    }

    protected bool IsInRole(string role)
    {
        return User.IsInRole(role);
    }

    protected bool IsInAnyRole(params string[] roles)
    {
        return roles.Any(role => User.IsInRole(role));
    }
}
