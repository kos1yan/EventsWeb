using Events.Application.DataTransferObjects.Member;
using MediatR;

namespace Events.Application.UseCases.Members.Queries
{
    public sealed record GetMembersQuery(Guid eventId, bool trackChanges) : IRequest<List<MemberDto>>;
}
