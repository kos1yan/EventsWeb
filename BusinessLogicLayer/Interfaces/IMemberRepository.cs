using Events.Domain.Entities;
using Events.Domain.RequestFeatures;

namespace Events.Application.Interfaces
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetMembersAsync(Guid eventId, bool trackChanges, CancellationToken token);
        Task<PagedList<Event>> GetUserEventsAsync(string userId, EventParameters eventParameters, bool trackChanges, CancellationToken token);
        Task<Member> GetMemberAsync(Guid memberId, bool trackChanges, CancellationToken token);
        Task<Member> GetMemberAsync(string userId, Guid eventId, bool trackChanges, CancellationToken token);
        void CreateMember(string userId, Guid eventId, Member member);
        void DeleteMember(Member member);
    }
}
