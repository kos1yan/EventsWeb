using Events.Application.DataTransferObjects.User;
using Events.Application.UseCases.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender _sender;
        public AuthenticationController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationDto userForRegistration)
        {
            await _sender.Send(new RegisterUserCommand(userForRegistration));
            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LogInDto userForAuthentication)
        {
            var user = await _sender.Send(new ValidateUserCommand(userForAuthentication));
            var tokenDto = await _sender.Send(new CreateTokenCommand(user, populateExp: true));

            return Ok(tokenDto);
        }
    }
}
