using Events.Application.DataTransferObjects.Token;
using Events.Application.Interfaces;
using Events.Application.UseCases.Auth.Commands;
using MediatR;

namespace Events.Application.UseCases.Auth.Handlers
{
    internal sealed class CreateTokenHandler : IRequestHandler<CreateTokenCommand, TokenDto>
    {
        private readonly ITokenService _tokenService;

        public CreateTokenHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<TokenDto> Handle(CreateTokenCommand request, CancellationToken token)
        {
            return await _tokenService.CreateToken(request.user, request.populateExp);
        }
    }
}
