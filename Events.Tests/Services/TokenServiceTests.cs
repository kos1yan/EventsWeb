using Events.Application.DataTransferObjects.Token;
using Events.Application.Interfaces;
using Events.Domain.Entities;
using Events.Domain.Entities.ConfigurationModels;
using Events.Domain.Exceptions;
using Events.Infrastructure.Auth;
using Microsoft.Extensions.Options;
using Moq;

namespace Events.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly Mock<IRepositoryManager> _repository;
        private readonly Mock<IOptions<JwtConfiguration>> _configuration;
        private readonly JwtConfiguration _jwtConfiguration;
        private TokenService _tokenService;

        public TokenServiceTests()
        {
            _repository = new Mock<IRepositoryManager>();
            _configuration = new Mock<IOptions<JwtConfiguration>>();
            _jwtConfiguration = new JwtConfiguration
            {
                SecretKey = "SecretKeySecretKeySecretKeySecretKey"
            };
            _configuration.Setup(c => c.Value).Returns(_jwtConfiguration);
            
            _tokenService = new TokenService(_repository.Object, _configuration.Object);
        }

        [Fact]
        public async Task TokenService_CreateToken_ReturnTokenDto()
        {
            //Arrange


            var user = new User();
            var populateExp = true;

            _repository.Setup(c => c.User.UpdateUserAsync(user));
            _repository.Setup(c => c.User.GetRolesAsync(user)).ReturnsAsync(new List<string>());

            //Act

            var serviceResult = await _tokenService.CreateToken(user, populateExp);

            //Assert

            Assert.NotNull(serviceResult);
            Assert.IsType<TokenDto>(serviceResult);
            Assert.Equal(user.RefreshToken, serviceResult.RefreshToken);
        }

        [Fact]
        public async Task TokenService_RefreshToken_ReturnTokenDto()
        {
            //Arrange

            var refreshToken = "refresh token";
            var user = new User() { RefreshTokenExpiryTime = DateTime.MaxValue};
            var populateExp = true;

            _repository.Setup(c => c.User.UpdateUserAsync(user));
            _repository.Setup(c => c.User.GetRolesAsync(user)).ReturnsAsync(new List<string>());
            _repository.Setup(c => c.User.GetByRefreshTokenAsync(refreshToken)).ReturnsAsync(user);

            //Act

            var serviceResult = await _tokenService.RefreshToken(refreshToken);

            //Assert

            Assert.NotNull(serviceResult);
            Assert.IsType<TokenDto>(serviceResult);
            Assert.Equal(user.RefreshToken, serviceResult.RefreshToken);
        }

        [Fact]
        public async Task TokenService_RefreshToken_ThrowRefreshTokenBadRequestException()
        {
            //Arrange

            var refreshToken = "refresh token";
            var user = new User() { RefreshTokenExpiryTime = DateTime.Now };

            
            _repository.Setup(c => c.User.GetByRefreshTokenAsync(refreshToken)).ReturnsAsync(user);

            //Act
 

            //Assert

            await Assert.ThrowsAnyAsync<RefreshTokenBadRequestException>(async () =>
             await _tokenService.RefreshToken(refreshToken));
        }
    }
}
