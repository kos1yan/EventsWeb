using AutoMapper;
using BusinessLogicLayer.MappingProfiles;
using BusinessLogicLayer.Services.Implementations;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Moq;
using Shared.DataTransferObjects.Category;
using Shared.DataTransferObjects.Event;
using Shared.Exceptions;
using Shared.RequestFeatures;

namespace Events.Tests.Services
{
    public class EventServiceTests
    {
        private readonly Mock<IRepositoryManager> _repository;
        private readonly IMapper _mapper;
        private Event _event;
        private EventService _eventService;

        public EventServiceTests()
        {
            _repository = new Mock<IRepositoryManager>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfiles(
                    [
                    new EventMappingProfile(),
                    new ImageMappingProfile(),
                    new CategoryMappingProfile()
                    ]);
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
            _event = new Event
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Adress = "Adress",
                Date = "Date",
                MaxMemberCount = 15,
                MemberCount = 7,
                Category = new Category { Id = 1, Name = "Sport" },
                Members = new List<Member> { new Member { UserId = "id" } } 
            };

            _eventService = new EventService(_repository.Object, _mapper);
        }

        [Fact]
        public async Task EventService_CreateEventAsync_ReturnEventDto()
        {
            //Arrange
            
            var eventForCreation = new EventForCreationDto
            {
                Name = "Name",
                Description = "Description",
                Adress = "Adress",
                Date = "Date",
                MaxMemberCount = 15,
                CategoryId = 1
            };
            var newEvent = _mapper.Map<Event>(eventForCreation);

            _repository.Setup(c => c.Event.CreateEvent(newEvent));
            _repository.Setup(c => c.SaveAsync());

            var eventToReturn = _mapper.Map<EventDto>(newEvent);

            //Act

            var serviceResult = await _eventService.CreateEventAsync(eventForCreation);

            //Assert

            Assert.NotNull(serviceResult);
            Assert.IsType<EventDto>(serviceResult);
            Assert.Equal(eventForCreation.Name, serviceResult.Name);
            Assert.Equal(eventForCreation.Adress, serviceResult.Adress);
            Assert.Equal(eventForCreation.Description, serviceResult.Description);
            Assert.Equal(eventForCreation.MaxMemberCount, serviceResult.FreePlaces);
            Assert.Equal(eventForCreation.Date, serviceResult.Date);
        }

        [Fact]
        public async Task EventService_DeleteEventAsync_ThrowEventNotFoundException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = false;
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges));


            //Act



            //Assert

            await Assert.ThrowsAnyAsync<EventNotFoundException>(async () => await _eventService.DeleteEventAsync(eventId, trackChanges));

        }

        [Fact]
        public async Task EventService_GetEventAsync_ReturnEventDto()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = false;
            var userId = "id";

            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges)).ReturnsAsync(_event);

            var eventToReturn = _mapper.Map<EventDto>(_event);
            eventToReturn.Category = _mapper.Map<CategoryDto>(_event.Category);

            //Act

            var serviceResult = await _eventService.GetEventAsync(userId, eventId, trackChanges);

            //Assert

            Assert.NotNull(serviceResult);
            Assert.IsType<EventDto>(serviceResult);
            Assert.Equal(_event.Name, serviceResult.Name);
            Assert.Equal(_event.Id, serviceResult.Id);
            Assert.Equal(_event.Adress, serviceResult.Adress);
            Assert.Equal(_event.Description, serviceResult.Description);
            Assert.Equal(_event.MaxMemberCount - _event.MemberCount, serviceResult.FreePlaces);
            Assert.Equal(_event.Date, serviceResult.Date);
            Assert.Equal(_event.Category.Id, serviceResult.Category.Id);
            Assert.Equal(_event.Category.Name, serviceResult.Category.Name);
            Assert.Equal(_event.Date, serviceResult.Date);
            Assert.True(serviceResult.IsSubscribed);

        }

        [Fact]
        public async Task EventService_GetEventAsync_ThrowEventNotFoundException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = false;
            var userId = "Id";
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges));


            //Act



            //Assert

            await Assert.ThrowsAnyAsync<EventNotFoundException>(async () => await _eventService.GetEventAsync(userId, eventId, trackChanges));

        }

        [Fact]
        public async Task EventService_GetEventsAsync_ReturnEventForReviewDtosAndMetaData()
        {
            //Arrange

            var eventParameters = new Mock<EventParameters>();
            var trackChanges = false;
            var count = 1;
            var userId = "id";


            var eventWithMetaData = new PagedList<Event>(new List<Event> { _event }, count, 1,1);
            _repository.Setup(c => c.Event.GetEventsAsync(eventParameters.Object, trackChanges)).ReturnsAsync(eventWithMetaData);
            var eventsDto = new List<EventForReviewDto>();

            for (int i = 0; i < eventWithMetaData.Count; i++)
            {
                var eventDto = _mapper.Map<EventForReviewDto>(eventWithMetaData[i]);
                eventsDto.Add(eventDto);
            }

            //Act

            var serviceResult = await _eventService.GetEventsAsync(userId, eventParameters.Object, trackChanges);

            //Assert

            Assert.IsType<List<EventForReviewDto>>(serviceResult.events);
            Assert.IsType<MetaData>(serviceResult.metaData);
            Assert.Equal(_event.Id, serviceResult.events[count-1].Id);
            Assert.Equal(_event.Name, serviceResult.events[count-1].Name);
            Assert.Equal(_event.MaxMemberCount - _event.MemberCount, serviceResult.events[count-1].FreePlaces);
            Assert.True(serviceResult.events[count-1].IsSubscribed);
        }

        [Fact]
        public async Task EventService_UpdateEventAsync_ThrowEventNotFoundException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = false;
            var eventForUpdate = new Mock<EventForUpdateDto>();
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges));


            //Act



            //Assert

            await Assert.ThrowsAnyAsync<EventNotFoundException>(async () => 
            await _eventService.UpdateEventAsync(eventId, eventForUpdate.Object, trackChanges));

        }

        [Fact]
        public async Task EventService_UpdateEventAsync_ThrowMaxMemberCountBadRequestException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = false;
            var eventForUpdate = new EventForUpdateDto { MaxMemberCount = 1};
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges)).ReturnsAsync(_event);


            //Act



            //Assert

            await Assert.ThrowsAnyAsync<MaxMemberCountBadRequestException>(async () =>
            await _eventService.UpdateEventAsync(eventId, eventForUpdate, trackChanges));

        }

        [Fact]
        public async Task EventService_UpdateEventAsync_TestMapping()
        {
            //Arrange

            var eventForUpdate = new EventForUpdateDto
            {
                Name = "Name",
                Description = "Description",
                Adress = "Adress",
                Date = "Date",
                MaxMemberCount = 15,
                CategoryId = 1
            };
            var eventId = Guid.NewGuid();
            var trackChanges = false;

            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges)).ReturnsAsync(_event);

            //Act

            await _eventService.UpdateEventAsync(eventId, eventForUpdate, trackChanges);

            //Assert

            Assert.Equal(_event.Name, eventForUpdate.Name);
            Assert.Equal(_event.Adress, eventForUpdate.Adress);
            Assert.Equal(_event.Description, eventForUpdate.Description);
            Assert.Equal(_event.MaxMemberCount, eventForUpdate.MaxMemberCount);
            Assert.Equal(_event.Date, eventForUpdate.Date);
            Assert.Equal(_event.CategoryId, eventForUpdate.CategoryId);
        }

        [Fact]
        public async Task EventService_GetUserEventsAsync_ReturnEventForReviewDtosAndMetaData()
        {
            //Arrange

            var eventParameters = new Mock<EventParameters>();
            var trackChanges = false;
            var count = 1;
            var userId = "id";

            var eventWithMetaData = new PagedList<Event>(new List<Event> { _event }, count, 1, 1);
            _repository.Setup(c => c.Member.GetUserEventsAsync(userId, eventParameters.Object, trackChanges)).ReturnsAsync(eventWithMetaData);
            var eventsDto = new List<EventForReviewDto>();

            for (int i = 0; i < eventWithMetaData.Count; i++)
            {
                var eventDto = _mapper.Map<EventForReviewDto>(eventWithMetaData[i]);
                eventsDto.Add(eventDto);
            }

            //Act

            var serviceResult = await _eventService.GetUserEventsAsync(userId, eventParameters.Object, trackChanges);

            //Assert

            Assert.IsType<List<EventForReviewDto>>(serviceResult.events);
            Assert.IsType<MetaData>(serviceResult.metaData);
            Assert.Equal(_event.Id, serviceResult.events[count - 1].Id);
            Assert.Equal(_event.Name, serviceResult.events[count - 1].Name);
            Assert.Equal(_event.MaxMemberCount - _event.MemberCount, serviceResult.events[count - 1].FreePlaces);
            Assert.True(serviceResult.events[count - 1].IsSubscribed);
        }


    }
}
