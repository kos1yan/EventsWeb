using Events.Application.DataTransferObjects.User;
using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Events.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(RegistrationDto userForRegistration);
        Task<User> ValidateUser(LogInDto userForAuth);
    }
}
