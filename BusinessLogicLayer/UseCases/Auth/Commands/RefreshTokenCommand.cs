using Events.Application.DataTransferObjects.Token;
using MediatR;

namespace Events.Application.UseCases.Auth.Commands
{
    public sealed record RefreshTokenCommand(string refreshToken) : IRequest<TokenDto>;
}
