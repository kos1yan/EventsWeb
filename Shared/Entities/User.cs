using Microsoft.AspNetCore.Identity;

namespace Events.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? DeviceToken { get; set; }
        public List<Member>? Members { get; set; }
    }
}
