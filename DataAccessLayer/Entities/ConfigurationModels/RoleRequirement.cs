using Microsoft.AspNetCore.Authorization;

namespace DataAccessLayer.Entities.ConfigurationModels
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(string role) => Role = role;

        public string Role { get; }
    }
}
