using Events.Application.DataTransferObjects.Member;
using MediatR;

namespace Events.Application.UseCases.Members.Commands
{
    public sealed record CreateMemberCommand(string userId, Guid eventId, MemberForCreationDto memberForCreation, bool trackChanges) : IRequest<MemberDto>;
}
