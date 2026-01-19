using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TransportationAttendance.API.Infrastructure;

public class CaseInsensitiveRoleRequirement : IAuthorizationRequirement
{
    public string[] AllowedRolePatterns { get; }

    public CaseInsensitiveRoleRequirement(params string[] allowedRolePatterns)
    {
        AllowedRolePatterns = allowedRolePatterns;
    }
}

public class CaseInsensitiveRoleHandler : AuthorizationHandler<CaseInsensitiveRoleRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CaseInsensitiveRoleRequirement requirement)
    {
        var userRoles = context.User.FindAll(ClaimTypes.Role)
            .Concat(context.User.FindAll("role"))
            .Select(c => c.Value)
            .ToList();

        var hasAccess = userRoles.Any(userRole =>
            requirement.AllowedRolePatterns.Any(pattern =>
                userRole.Contains(pattern, StringComparison.OrdinalIgnoreCase) ||
                userRole.Equals(pattern, StringComparison.OrdinalIgnoreCase)));

        if (hasAccess)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
