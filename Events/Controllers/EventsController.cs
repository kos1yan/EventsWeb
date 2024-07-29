using Events.API.Extensions;
using Events.Application.DataTransferObjects.Event;
using Events.Application.UseCases.Events.Commands;
using Events.Application.UseCases.Events.Queries;
using Events.Domain.Entities.ConfigurationModels;
using Events.Domain.RequestFeatures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Events.API.Controllers
{
    [Route("api/events")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class EventsController : ControllerBase
    {
        private readonly ISender _sender;

        public EventsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        
        public async Task<IActionResult> GetEvents([FromQuery] EventParameters eventParameters, CancellationToken token)
        {
            var userId = HttpContext.User.GetUserId();
            var pagedResult = await _sender.Send(new GetEventsQuery(userId, eventParameters, trackChanges: false), token);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.events);
        }

        [HttpGet("user")]
        [Authorize(UserRoles.User)]
        public async Task<IActionResult> GetUserEvents([FromQuery] EventParameters eventParameters, CancellationToken token)
        {
            var userId = HttpContext.User.GetUserId();
            var pagedResult = await _sender.Send(new GetUserEventsQuery(userId, eventParameters, trackChanges: false), token);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.events);
        }

        [HttpGet("{eventId:guid}", Name = "EventById")]
        public async Task<IActionResult> GetEvent(Guid eventId, CancellationToken token)
        {
            var userId = HttpContext.User.GetUserId();
            var eventDto = await _sender.Send(new GetEventQuery(userId, eventId, trackChanges: false), token);

            return Ok(eventDto);
        }

        [HttpPost]
        [Authorize(UserRoles.Admin)]
        public async Task<IActionResult> CreateEvent([FromForm] EventForCreationDto eventForCreation, CancellationToken token)
        {
            var eventToReturn = await _sender.Send(new CreateEventCommand(eventForCreation), token);

            return CreatedAtRoute("EventById", new
            {
                eventId = eventToReturn.Id
            },
           eventToReturn);

        }

        [HttpDelete("{eventId:guid}")]
        [Authorize(UserRoles.Admin)]
        public async Task<IActionResult> DeleteEvent(Guid eventId, CancellationToken token)
        {
            await _sender.Send(new DeleteEventCommand(eventId, trackChanges: false), token);
            return NoContent();
        }

        [HttpPut("{eventId:guid}")]
        [Authorize(UserRoles.Admin)]
        public async Task<IActionResult> UpdateEvent(Guid eventId, [FromForm] EventForUpdateDto eventForUpdate, CancellationToken token)
        {
            var eventToReturn = await _sender.Send(new UpdateEventCommand(eventId, eventForUpdate, trackChanges: true), token);
            return Ok(eventToReturn);
        }
    }
}
