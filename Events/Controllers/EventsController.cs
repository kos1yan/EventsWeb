using BusinessLogicLayer.Services;
using DataAccessLayer.Entities.ConfigurationModels;
using Events.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects.Event;
using Shared.RequestFeatures;
using System.Text.Json;

namespace Events.Controllers
{
    [Route("api/events")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class EventsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public EventsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        
        public async Task<IActionResult> GetEvents([FromQuery] EventParameters eventParameters)
        {
            var userId = HttpContext.User.GetUserId();
            var pagedResult = await _service.EventService.GetEventsAsync(userId, eventParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.events);
        }

        [HttpGet("user")]
        [Authorize(UserRoles.User)]
        public async Task<IActionResult> GetUserEvents([FromQuery] EventParameters eventParameters)
        {
            var userId = HttpContext.User.GetUserId();
            var pagedResult = await _service.EventService.GetUserEventsAsync(userId, eventParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.events);
        }

        [HttpGet("{eventId:guid}", Name = "EventById")]
        public async Task<IActionResult> GetEvent(Guid eventId)
        {
            var userId = HttpContext.User.GetUserId();
            var eventDto = await _service.EventService.GetEventAsync(userId, eventId, trackChanges: false);

            return Ok(eventDto);
        }

        [HttpPost]
        [Authorize(UserRoles.Admin)]
        public async Task<IActionResult> CreateEvent([FromForm] EventForCreationDto eventForCreation)
        {
            var eventToReturn = await _service.EventService.CreateEventAsync(eventForCreation);

            return CreatedAtRoute("EventById", new
            {
                eventId = eventToReturn.Id
            },
           eventToReturn);

        }

        [HttpDelete("{eventId:guid}")]
        [Authorize(UserRoles.Admin)]
        public async Task<IActionResult> DeleteEvent(Guid eventId)
        {
            await _service.EventService.DeleteEventAsync(eventId, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{eventId:guid}")]
        [Authorize(UserRoles.Admin)]
        public async Task<IActionResult> UpdateEvent(Guid eventId, [FromForm] EventForUpdateDto eventForUpdate)
        {
            var eventToReturn = await _service.EventService.UpdateEventAsync(eventId, eventForUpdate, trackChanges: true);
            return Ok(eventToReturn);
        }
    }
}
