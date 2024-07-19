using DataAccessLayer.DbContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace DataAccessLayer.Repositories.Implementations
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

        public async Task<Event> GetEventAsync(Guid eventId, bool trackChanges)
        {
            return await FindByCondition(s => s.Id.Equals(eventId), trackChanges)
                .Include(s => s.Images)
                .Include(s => s.Category)
                .Include(s => s.Members)
                .ThenInclude(s => s.User)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<Event>> GetEventsAsync(EventParameters eventParameters, bool trackChanges)
        {
            var events = await FindAll(trackChanges).Include(s =>s.Category)
                .Include(s => s.Images)
                .Include(s => s.Members)   
                .SearchByName(eventParameters.SearchByName)
                .FilterByAdress(eventParameters.Adress)
                .FilterByCategory(eventParameters.Category)
                .FilterByDate(eventParameters.Date)
                .Skip((eventParameters.PageNumber - 1) * eventParameters.PageSize)
                .Take(eventParameters.PageSize)
                .ToListAsync();

            var count = await FindAll(trackChanges).SearchByName(eventParameters.SearchByName)
                .FilterByAdress(eventParameters.Adress)
                .FilterByCategory(eventParameters.Category)
                .FilterByDate(eventParameters.Date)
                .CountAsync();

            return new PagedList<Event>(events, count, eventParameters.PageNumber, eventParameters.PageSize);
        }
    }
}
