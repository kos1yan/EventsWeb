using DataAccessLayer.Entities.ConfigurationModels;
using Microsoft.AspNetCore.Authorization;
using Shared.Exceptions;

namespace Events.Extensions
{
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            IEnumerable<IAuthorizationRequirement> requirements = context.Requirements;

            if (context.User.Claims.Count() == 0) throw new ClaimsBadRequestException();

            var role = context.User.FindFirst(x => x.Type == "userRole")!.Value;

            string[] roles = role.Split(',');
            string expectedRole = requirement.Role;

            string[] requireRoles = requirements.Where(y => y.GetType() == typeof(RoleRequirement)).Select(x => ((RoleRequirement)x).Role).ToArray();

            var isMatch = requireRoles.Any(x => roles.Any(y => x == y));

            if (!isMatch) return Task.FromException(new IdentityException("User doesn't has the required role"));

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
