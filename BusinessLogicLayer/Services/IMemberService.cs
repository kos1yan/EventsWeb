using Shared.DataTransferObjects.Member;

namespace BusinessLogicLayer.Services
{
    public interface IMemberService
    {
        Task<List<MemberDto>> GetMembersAsync(Guid eventId, bool trackChanges);
        Task<MemberDto> GetMemberAsync(Guid memberId, bool trackChanges);
        Task<MemberDto> CreateMemberAsync(string userId, Guid eventId, MemberForCreationDto memberForCreation, bool trackChanges);
        Task DeleteMemberAsync(string userId, Guid eventId, bool trackChanges);
    }
}
