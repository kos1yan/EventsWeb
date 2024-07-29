using Events.Application.Interfaces;
using Events.Domain.Entities;
using Events.Domain.RequestFeatures;
using Events.Infrastructure.DbContext;
using Events.Infrastructure.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repositories
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(EventContext eventContext) : base(eventContext) { }

        public void CreateEvent(Event eventForCreate)
        {
            Create(eventForCreate);
        }

        public void DeleteEvent(Event eventForDelete)
        {
            Delete(eventForDelete);
        }

        public async Task<Event> GetEventAsync(Guid eventId, bool trackChanges, CancellationToken token)
        {
            return await FindByCondition(s => s.Id.Equals(eventId), trackChanges)
                .Include(s => s.Images)
                .Include(s => s.Category)
                .Include(s => s.Members)
                .ThenInclude(s => s.User)
                .SingleOrDefaultAsync(token);
        }

        public async Task<PagedList<Event>> GetEventsAsync(EventParameters eventParameters, bool trackChanges, CancellationToken token)
        {
            var events = await FindAll(trackChanges).Include(s => s.Category)
                .Include(s => s.Images)
                .Include(s => s.Members)
                .SearchByName(eventParameters.SearchByName)
                .FilterByAdress(eventParameters.Adress)
                .FilterByCategory(eventParameters.Category)
                .FilterByDate(eventParameters.Date)
                .Skip((eventParameters.PageNumber - 1) * eventParameters.PageSize)
                .Take(eventParameters.PageSize)
                .ToListAsync(token);

            var count = await FindAll(trackChanges).SearchByName(eventParameters.SearchByName)
                .FilterByAdress(eventParameters.Adress)
                .FilterByCategory(eventParameters.Category)
                .FilterByDate(eventParameters.Date)
                .CountAsync(token);

            return new PagedList<Event>(events, count, eventParameters.PageNumber, eventParameters.PageSize);
        }
    }
}
