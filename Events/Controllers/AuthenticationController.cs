using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects.User;

namespace Events.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;
        public AuthenticationController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationDto userForRegistration)
        {
            await _service.AuthenticationService.RegisterUser(userForRegistration);
            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LogInDto userForAuthentication)
        {
            var user = await _service.AuthenticationService.ValidateUser(userForAuthentication);
            var tokenDto = await _service.TokenService.CreateToken(user, populateExp: true);

            return Ok(tokenDto);
        }
    }
}
