using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Events.Application.Interfaces;
using Events.Application.DataTransferObjects.User;
using Events.Domain.Entities;
using Events.Domain.Entities.ConfigurationModels;
using Events.Domain.Exceptions;

namespace Events.Infrastructure.Auth
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public AuthenticationService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IdentityResult> RegisterUser(RegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _repository.User.CreateUserAsync(user, userForRegistration.Password);

            if (result.Succeeded) await _repository.User.AddToRoleAsync(user, UserRoles.User);
            else throw new IdentityException("Registration failed. Incorrect password or email.");

            return result;
        }

        public async Task<User> ValidateUser(LogInDto userForAuth)
        {
            var user = await _repository.User.GetByEmailAsync(userForAuth.Email);
            var result = (user != null && await _repository.User.CheckPasswordAsync(user, userForAuth.Password));

            if (!result) throw new IdentityException("Authentication failed. Wrong email or password.");

            return user;
        }
    }
}
