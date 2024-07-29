using Events.Application.Interfaces;
using Events.Application.UseCases.Auth.Commands;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.UseCases.Auth.Handlers
{
    internal sealed class ValidateUserHandler : IRequestHandler<ValidateUserCommand, User>
    {
        private readonly IAuthenticationService _authenticationService;

        public ValidateUserHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<User> Handle(ValidateUserCommand request, CancellationToken token)
        {
            return await _authenticationService.ValidateUser(request.userForAuth);
        }
    }
}
