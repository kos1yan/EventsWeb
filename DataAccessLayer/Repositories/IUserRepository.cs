using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByRefreshTokenAsync(string refreshToken);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IList<string>> GetRolesAsync(User user);
    }
}
