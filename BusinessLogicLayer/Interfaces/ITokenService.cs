using Events.Application.DataTransferObjects.Token;
using Events.Domain.Entities;

namespace Events.Application.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDto> CreateToken(User user, bool populateExp);
        Task<TokenDto> RefreshToken(string refreshToken);
    }
}
