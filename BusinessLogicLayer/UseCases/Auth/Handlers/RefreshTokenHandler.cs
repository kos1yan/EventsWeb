using Events.Application.DataTransferObjects.Token;
using Events.Application.Interfaces;
using Events.Application.UseCases.Auth.Commands;
using MediatR;

namespace Events.Application.UseCases.Auth.Handlers
{
    internal sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, TokenDto>
    {
        private readonly ITokenService _tokenService;

        public RefreshTokenHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<TokenDto> Handle(RefreshTokenCommand request, CancellationToken token)
        {
            return await _tokenService.RefreshToken(request.refreshToken);
        }
    }
}
