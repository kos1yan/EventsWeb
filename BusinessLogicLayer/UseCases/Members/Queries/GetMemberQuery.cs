using Events.Application.DataTransferObjects.Member;
using MediatR;

namespace Events.Application.UseCases.Members.Queries
{
    public sealed record GetMemberQuery(Guid memberId, bool trackChanges) : IRequest<MemberDto>;
}
