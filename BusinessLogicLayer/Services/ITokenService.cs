using DataAccessLayer.Entities;
using Shared.DataTransferObjects.Token;

namespace BusinessLogicLayer.Services
{
    public interface ITokenService
    {
        Task<TokenDto> CreateToken(User user, bool populateExp);
        Task<TokenDto> RefreshToken(string refreshToken);
    }
}
