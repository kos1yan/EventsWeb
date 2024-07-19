using AutoMapper;
using BusinessLogicLayer.MappingProfiles;
using BusinessLogicLayer.Services.Implementations;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Moq;
using Shared.DataTransferObjects.User;
using Shared.Exceptions;

namespace Events.Tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IRepositoryManager> _repository;
        private readonly IMapper _mapper;
        private AuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            _repository = new Mock<IRepositoryManager>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserMappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
            _authenticationService = new AuthenticationService(_repository.Object, _mapper);
        }

        [Fact]
        public async Task AuthenticationService_ValidateUser_ReturnUser()
        {
            //Arrange

            var userForAuth = new LogInDto
            {
                Email = "Email",
                Password = "Password"
            };

            var user = new User();
            _repository.Setup(c => c.User.GetByEmailAsync(userForAuth.Email)).ReturnsAsync(user);
            _repository.Setup(c => c.User.CheckPasswordAsync(user, userForAuth.Password)).ReturnsAsync(true);

            //Act

            var serviceResult = await _authenticationService.ValidateUser(userForAuth);

            //Assert

            Assert.NotNull(serviceResult);
            Assert.IsType<User>(serviceResult);
        }

        [Fact]
        public async Task AuthenticationService_ValidateUser_IdentityException()
        {
            //Arrange

            var userForAuth = new LogInDto
            {
                Email = "Email",
                Password = "Password"
            };

            var user = new User();
            _repository.Setup(c => c.User.GetByEmailAsync(userForAuth.Email)).ReturnsAsync(user);
            _repository.Setup(c => c.User.CheckPasswordAsync(user, userForAuth.Password)).ReturnsAsync(false);

            //Act


            //Assert

            await Assert.ThrowsAnyAsync<IdentityException>(async () =>
            await _authenticationService.ValidateUser(userForAuth));
        }

    }
}
