using Events.Application.DataTransferObjects.Token;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.UseCases.Auth.Commands
{
    public sealed record CreateTokenCommand(User user, bool populateExp) : IRequest<TokenDto>;
}
