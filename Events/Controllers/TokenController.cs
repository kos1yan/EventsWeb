using Events.Application.UseCases.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers
{
    [Route("api/token")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class TokenController : ControllerBase
    {
        private readonly ISender _sender;
        public TokenController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromHeader(Name = "Refresh-Token")] string refreshToken)
        {
            var tokenDtoToReturn = await _sender.Send(new RefreshTokenCommand(refreshToken));
            return Ok(tokenDtoToReturn);
        }
    }
}
