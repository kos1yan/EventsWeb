using Events.Application.DataTransferObjects.User;
using MediatR;

namespace Events.Application.UseCases.Auth.Commands
{
    public sealed record RegisterUserCommand(RegistrationDto userForRegistration) : IRequest<Unit>;
}
