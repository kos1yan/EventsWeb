using Shared.DataTransferObjects.Event;
using Shared.RequestFeatures;

namespace BusinessLogicLayer.Services
{
    public interface IEventService
    {
        Task<(List<EventForReviewDto> events, MetaData metaData)> GetEventsAsync(string userId, EventParameters eventParameters, bool trackChanges);
        Task<(List<EventForReviewDto> events, MetaData metaData)> GetUserEventsAsync(string userId, EventParameters eventParameters, bool trackChanges);
        Task<EventDto> GetEventAsync(string userId, Guid eventId, bool trackChanges);
        Task<EventDto> CreateEventAsync(EventForCreationDto eventForCreation);
        Task DeleteEventAsync(Guid eventId, bool trackChanges);
        Task<EventDto> UpdateEventAsync(Guid eventId, EventForUpdateDto eventForUpdate, bool trackChanges);
    }
}
