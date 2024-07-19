using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.RequestFeatures;

namespace Events.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class NotificationsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public NotificationsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost]
        //[Authorize(UserRoles.Admin)]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            await _service.NotificationService.SendMessageAsync(request, false);

            return Ok();
        }
    }
}
