using Events.Application.Interfaces;
using Events.Domain.Entities;
using Events.Domain.RequestFeatures;
using Events.Infrastructure.DbContext;
using Events.Infrastructure.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repositories
{
    public class MemberRepository : RepositoryBase<Member>, IMemberRepository
    {
        public MemberRepository(EventContext eventContext) : base(eventContext) { }

        public void CreateMember(string userId, Guid eventId, Member member)
        {
            member.EventId = eventId;
            member.UserId = userId;
            Create(member);
        }

        public void DeleteMember(Member member)
        {
            Delete(member);
        }

        public async Task<Member> GetMemberAsync(Guid memberId, bool trackChanges, CancellationToken token)
        {
            return await FindByCondition(s => s.Id.Equals(memberId), trackChanges).SingleOrDefaultAsync(token);
        }

        public async Task<Member> GetMemberAsync(string userId, Guid eventId, bool trackChanges, CancellationToken token)
        {
            return await FindByCondition(s => s.EventId.Equals(eventId) & s.UserId.Equals(userId), trackChanges)
                .Include(s => s.Event)
                .SingleOrDefaultAsync(token);
        }

        public async Task<List<Member>> GetMembersAsync(Guid eventId, bool trackChanges, CancellationToken token)
        {
            return await FindByCondition(s => s.EventId.Equals(eventId), trackChanges).ToListAsync(token);
        }

        public async Task<PagedList<Event>> GetUserEventsAsync(string userId, EventParameters eventParameters, bool trackChanges, CancellationToken token)
        {
            var events = await FindAll(trackChanges).Include(s => s.Event.Members).Include(s => s.Event.Images)
                .Where(s => s.UserId.Equals(userId))
                .Select(s => s.Event)
                .SearchByName(eventParameters.SearchByName)
                .FilterByAdress(eventParameters.Adress)
                .FilterByCategory(eventParameters.Category)
                .FilterByDate(eventParameters.Date)
                .Skip((eventParameters.PageNumber - 1) * eventParameters.PageSize)
                .Take(eventParameters.PageSize)
                .ToListAsync(token);

            var count = await FindAll(trackChanges).Include(s => s.Event)
                .Where(s => s.UserId.Equals(userId))
                .Select(s => s.Event)
                .SearchByName(eventParameters.SearchByName)
                .FilterByAdress(eventParameters.Adress)
                .FilterByCategory(eventParameters.Category)
                .FilterByDate(eventParameters.Date)
                .CountAsync(token);

            return new PagedList<Event>(events, count, eventParameters.PageNumber, eventParameters.PageSize);
        }
    }
}
