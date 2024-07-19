using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects.User;

namespace BusinessLogicLayer.Services
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(RegistrationDto userForRegistration);
        Task<User> ValidateUser(LogInDto userForAuth);
    }
}
