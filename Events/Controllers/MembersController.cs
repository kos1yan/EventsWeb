using BusinessLogicLayer.Services;
using DataAccessLayer.Entities.ConfigurationModels;
using Events.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects.Member;

namespace Events.Controllers
{
    [Route("api/members")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class MembersController : ControllerBase
    {
        private readonly IServiceManager _service;

        public MembersController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("{memberId:guid}", Name = "MemberById")]
        public async Task<IActionResult> GetMember(Guid memberId)
        {
            var member = await _service.MemberService.GetMemberAsync(memberId, trackChanges: false);

            return Ok(member);
        }

        [HttpGet("event/{eventId:guid}")]
        
        public async Task<IActionResult> GetMembers(Guid eventId)
        {
            var members = await _service.MemberService.GetMembersAsync(eventId, trackChanges: false);

            return Ok(members);
        }

        [HttpPost("event/{eventId:guid}")]
        [Authorize(UserRoles.User)]
        public async Task<IActionResult> CreateMember(Guid eventId, [FromBody] MemberForCreationDto memberForCreation)
        {
            var userId = HttpContext.User.GetUserId();
            var memberToReturn = await _service.MemberService.CreateMemberAsync(userId, eventId, memberForCreation, trackChanges: true);

            return CreatedAtRoute("MemberById", new
            {
                memberId = memberToReturn.Id
            },
           memberToReturn);

        }

        [HttpDelete("event/{eventId:guid}")]
        [Authorize(UserRoles.User)]
        public async Task<IActionResult> DeleteMember(Guid eventId)
        {
            var userId = HttpContext.User.GetUserId();
            await _service.MemberService.DeleteMemberAsync(userId, eventId, trackChanges: true);
            return NoContent();
        }
    }
}
