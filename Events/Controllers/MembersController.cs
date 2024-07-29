using Events.API.Extensions;
using Events.Application.DataTransferObjects.Member;
using Events.Application.UseCases.Members.Commands;
using Events.Application.UseCases.Members.Queries;
using Events.Domain.Entities.ConfigurationModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers
{
    [Route("api/members")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class MembersController : ControllerBase
    {
        private readonly ISender _sender;

        public MembersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{memberId:guid}", Name = "MemberById")]
        public async Task<IActionResult> GetMember(Guid memberId, CancellationToken token)
        {
            var member = await _sender.Send(new GetMemberQuery(memberId, trackChanges: false), token);

            return Ok(member);
        }

        [HttpGet("event/{eventId:guid}")]
        
        public async Task<IActionResult> GetMembers(Guid eventId, CancellationToken token)
        {
            var members = await _sender.Send(new GetMemberQuery(eventId, trackChanges: false), token);

            return Ok(members);
        }

        [HttpPost("event/{eventId:guid}")]
        [Authorize(UserRoles.User)]
        public async Task<IActionResult> CreateMember(Guid eventId, [FromBody] MemberForCreationDto memberForCreation, CancellationToken token)
        {
            var userId = HttpContext.User.GetUserId();
            var memberToReturn = await _sender.Send(new CreateMemberCommand(userId, eventId, memberForCreation, trackChanges: true), token);

            return CreatedAtRoute("MemberById", new
            {
                memberId = memberToReturn.Id
            },
           memberToReturn);

        }

        [HttpDelete("event/{eventId:guid}")]
        [Authorize(UserRoles.User)]
        public async Task<IActionResult> DeleteMember(Guid eventId, CancellationToken token)
        {
            var userId = HttpContext.User.GetUserId();
            await _sender.Send(new DeleteMemberCommand(userId, eventId, trackChanges: true), token);
            return NoContent();
        }
    }
}
