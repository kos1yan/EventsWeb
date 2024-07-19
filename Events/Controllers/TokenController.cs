using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Events.Controllers
{
    [Route("api/token")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class TokenController : ControllerBase
    {
        private readonly IServiceManager _service;
        public TokenController(IServiceManager service) => _service = service;


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromHeader(Name = "Refresh-Token")] string refreshToken)
        {
            var tokenDtoToReturn = await _service.TokenService.RefreshToken(refreshToken);
            return Ok(tokenDtoToReturn);
        }
    }
}
