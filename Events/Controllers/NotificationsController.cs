using Events.Application.UseCases.Notifications.Commands;
using Events.Domain.Entities.ConfigurationModels;
using Events.Domain.RequestFeatures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class NotificationsController : ControllerBase
    {
        private readonly ISender _sender;

        public NotificationsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(UserRoles.Admin)]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request, CancellationToken token)
        {
            await _sender.Send(new SendMessageCommand(request, trackChanges : false), token);

            return Ok();
        }
    }
}
