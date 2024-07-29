using Events.Application.DataTransferObjects.User;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.UseCases.Auth.Commands
{
    public sealed record ValidateUserCommand(LogInDto userForAuth) : IRequest<User>;
}
