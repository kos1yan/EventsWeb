using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Shared.DataTransferObjects.Category;
using Shared.DataTransferObjects.Event;
using Shared.DataTransferObjects.Image;
using Shared.Exceptions;
using Shared.RequestFeatures;

namespace BusinessLogicLayer.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public EventService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<EventDto> CreateEventAsync(EventForCreationDto eventForCreation)
        {
            var newEvent = _mapper.Map<Event>(eventForCreation);

            if (eventForCreation.Images != null)
            {
                foreach (var image in eventForCreation.Images)
                {
                    var result = await _repository.Cloudinary.AddPhotoAsync(image);
                    var imageUrl = new Image() { Url = result.Url.ToString() };
                    newEvent.Images.Add(imageUrl);
                }
            }

            _repository.Event.CreateEvent(newEvent);
            await _repository.SaveAsync();

            var eventToReturn = _mapper.Map<EventDto>(newEvent);
            eventToReturn.FreePlaces = newEvent.MaxMemberCount;
            eventToReturn.Images = _mapper.Map<List<ImageDto>>(newEvent.Images);

            return eventToReturn;
        }

        public async Task DeleteEventAsync(Guid eventId, bool trackChanges)
        {
            var eventForDelete = await _repository.Event.GetEventAsync(eventId, trackChanges);
            if(eventForDelete is null) throw new EventNotFoundException(eventId);

            if (eventForDelete.Images.Count != 0)
            {
                foreach (var image in eventForDelete.Images)
                {
                    await _repository.Cloudinary.DeletePhotoAsync(image.Url);
                }
            }
            _repository.Event.DeleteEvent(eventForDelete);
            await _repository.SaveAsync();
        }

        public async Task<EventDto> GetEventAsync(string userId, Guid eventId, bool trackChanges)
        {
            var eventById = await _repository.Event.GetEventAsync(eventId, trackChanges);
            if (eventById is null) throw new EventNotFoundException(eventId);

            var eventToReturn = _mapper.Map<EventDto>(eventById);
            eventToReturn.FreePlaces = eventById.MaxMemberCount - eventById.MemberCount;
            eventToReturn.IsSubscribed = !eventById.Members.TrueForAll((member) => !member.UserId.Equals(userId));
            eventToReturn.Images = _mapper.Map<List<ImageDto>>(eventById.Images);
            eventToReturn.Category = _mapper.Map<CategoryDto>(eventById.Category);

            return eventToReturn;
        }

        public async Task<(List<EventForReviewDto> events, MetaData metaData)> GetEventsAsync(string userId, EventParameters eventParameters, bool trackChanges)
        {
            var eventWithMetaData = await _repository.Event.GetEventsAsync(eventParameters, trackChanges);
            var eventsDto = new List<EventForReviewDto>();

            for (int i = 0; i < eventWithMetaData.Count; i++)
            {
                var eventDto = _mapper.Map<EventForReviewDto>(eventWithMetaData[i]);

                eventDto.FreePlaces = eventWithMetaData[i].MaxMemberCount - eventWithMetaData[i].MemberCount;
                eventDto.Images = _mapper.Map<List<ImageDto>>(eventWithMetaData[i].Images);
                eventDto.IsSubscribed = !eventWithMetaData[i].Members.TrueForAll((member) => !member.UserId.Equals(userId));

                eventsDto.Add(eventDto);
            }

            return (events: eventsDto, metaData: eventWithMetaData.MetaData);
        }

        public async Task<EventDto> UpdateEventAsync(Guid eventId, EventForUpdateDto eventForUpdate, bool trackChanges)
        {
            var eventById = await _repository.Event.GetEventAsync(eventId, trackChanges);
            if (eventById is null) throw new EventNotFoundException(eventId);

            if(eventForUpdate.MaxMemberCount < eventById.MemberCount) throw new MaxMemberCountBadRequestException(eventById.MemberCount);

            if (eventForUpdate.DeletedImages != null)
            {
                foreach (var image in eventForUpdate.DeletedImages)
                {
                    await _repository.Cloudinary.DeletePhotoAsync(image);
                    var imageForDelete = eventById.Images.Where(s => s.Url.Equals(image)).SingleOrDefault();
                    eventById.Images.Remove(imageForDelete);
                }
            }

            if (eventForUpdate.NewImages != null)
            {
                foreach (var image in eventForUpdate.NewImages)
                {
                    var result = await _repository.Cloudinary.AddPhotoAsync(image);
                    var imageUrl = new Image() { Url = result.Url.ToString() };
                    eventById.Images.Add(imageUrl);
                }
            }

            _mapper.Map(eventForUpdate, eventById);
            var eventToReturn = _mapper.Map<EventDto>(eventById);
          
            await _repository.SaveAsync();

            return eventToReturn;
        }

        public async Task<(List<EventForReviewDto> events, MetaData metaData)> GetUserEventsAsync(string userId, EventParameters eventParameters, bool trackChanges)
        {
            var eventWithMetaData = await _repository.Member.GetUserEventsAsync(userId, eventParameters, trackChanges);
            var eventsDto = new List<EventForReviewDto>();

            for (int i = 0; i < eventWithMetaData.Count; i++)
            {
                var eventDto = _mapper.Map<EventForReviewDto>(eventWithMetaData[i]);

                eventDto.FreePlaces = eventWithMetaData[i].MaxMemberCount - eventWithMetaData[i].MemberCount;
                eventDto.Images = _mapper.Map<List<ImageDto>>(eventWithMetaData[i].Images);
                eventDto.IsSubscribed = !eventWithMetaData[i].Members.TrueForAll((member) => !member.UserId.Equals(userId));

                eventsDto.Add(eventDto);
            }

            return (events: eventsDto, metaData: eventWithMetaData.MetaData);
        }
    }
}
