using Microsoft.AspNetCore.Authorization;

namespace Events.Domain.Entities.ConfigurationModels
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(string role) => Role = role;

        public string Role { get; }
    }
}
