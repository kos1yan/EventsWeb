using System.Security.Claims;

namespace Events.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.Claims.Where(x => x.Type == "userId").First().Value;
        }
    }
}
