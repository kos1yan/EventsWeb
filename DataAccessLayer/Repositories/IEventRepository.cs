using DataAccessLayer.Entities;
using Shared.RequestFeatures;

namespace DataAccessLayer.Repositories
{
    public interface IEventRepository
    {
        Task<PagedList<Event>> GetEventsAsync(EventParameters eventParameters, bool trackChanges);
        Task<Event> GetEventAsync(Guid eventId, bool trackChanges);
        void CreateEvent(Event eventForCreate);
        void DeleteEvent(Event eventForDelete);
    }
}
