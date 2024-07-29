using Events.Application.Interfaces;
using Events.Application.UseCases.Auth.Commands;
using MediatR;

namespace Events.Application.UseCases.Auth.Handlers
{
    internal sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Unit>
    {
        private readonly IAuthenticationService _authenticationService;

        public RegisterUserHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken token)
        {
            await _authenticationService.RegisterUser(request.userForRegistration);

            return Unit.Value;
        }
    }
}
