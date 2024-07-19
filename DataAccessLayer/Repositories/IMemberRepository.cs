using DataAccessLayer.Entities;
using Shared.RequestFeatures;

namespace DataAccessLayer.Repositories
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetMembersAsync(Guid eventId, bool trackChanges);
        Task<PagedList<Event>> GetUserEventsAsync(string userId, EventParameters eventParameters, bool trackChanges);
        Task<Member> GetMemberAsync(Guid memberId, bool trackChanges);
        Task<Member> GetMemberAsync(string userId, Guid eventId, bool trackChanges);
        void CreateMember(string userId, Guid eventId, Member member);
        void DeleteMember(Member member);
    }
}
