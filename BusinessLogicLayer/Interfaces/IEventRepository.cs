using Events.Domain.Entities;
using Events.Domain.RequestFeatures;

namespace Events.Application.Interfaces
{
    public interface IEventRepository
    {
        Task<PagedList<Event>> GetEventsAsync(EventParameters eventParameters, bool trackChanges, CancellationToken token);
        Task<Event> GetEventAsync(Guid eventId, bool trackChanges, CancellationToken token);
        void CreateEvent(Event eventForCreate);
        void DeleteEvent(Event eventForDelete);
    }
}
