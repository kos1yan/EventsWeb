using AutoMapper;
using Events.Application.DataTransferObjects.Category;
using Events.Application.DataTransferObjects.Event;
using Events.Application.Interfaces;
using Events.Application.MappingProfiles;
using Events.Application.UseCases.Events.Commands;
using Events.Application.UseCases.Events.Handlers;
using Events.Application.UseCases.Events.Queries;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using Events.Domain.RequestFeatures;
using Moq;

namespace Events.Tests.Handlers
{
    public class EventHandleTests
    {
        private readonly Mock<IRepositoryManager> _repository;
        private readonly IMapper _mapper;
        private Event _event;

        public EventHandleTests()
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
        }

        [Fact]
        public async Task CreateEventHandler_ReturnEventDto()
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
            var createEventCommand = new CreateEventCommand(eventForCreation);
            var newEvent = _mapper.Map<Event>(eventForCreation);
            var token = CancellationToken.None;
            _repository.Setup(c => c.Event.CreateEvent(newEvent));
            _repository.Setup(c => c.SaveAsync());

            var eventToReturn = _mapper.Map<EventDto>(newEvent);
            var createEventHandler = new CreateEventHandler(_repository.Object, _mapper);

            //Act

            var handleResult = await createEventHandler.Handle(createEventCommand, token);

            //Assert

            Assert.NotNull(handleResult);
            Assert.IsType<EventDto>(handleResult);
            Assert.Equal(eventForCreation.Name, handleResult.Name);
            Assert.Equal(eventForCreation.Adress, handleResult.Adress);
            Assert.Equal(eventForCreation.Description, handleResult.Description);
            Assert.Equal(eventForCreation.MaxMemberCount, handleResult.FreePlaces);
            Assert.Equal(eventForCreation.Date, handleResult.Date);
        }

        [Fact]
        public async Task DeleteEventHandler_ThrowEventNotFoundException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = false;
            var token = CancellationToken.None;
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges, token));
            var deleteEventHandler = new DeleteEventHandler(_repository.Object, _mapper);
            var deleteEventCommand = new DeleteEventCommand(eventId, trackChanges);

            //Act



            //Assert

            await Assert.ThrowsAnyAsync<EventNotFoundException>(async () => await deleteEventHandler.Handle(deleteEventCommand, token));

        }

        [Fact]
        public async Task GetEventHandler_ReturnEventDto()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = false;
            var userId = "id";
            var token = CancellationToken.None;
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges, token)).ReturnsAsync(_event);

            var eventToReturn = _mapper.Map<EventDto>(_event);
            eventToReturn.Category = _mapper.Map<CategoryDto>(_event.Category);

            var getEventHandler = new GetEventHandler(_repository.Object, _mapper);
            var getEventQuery = new GetEventQuery(userId, eventId, trackChanges);

            //Act

            var handleResult = await getEventHandler.Handle(getEventQuery, token);

            //Assert

            Assert.NotNull(handleResult);
            Assert.IsType<EventDto>(handleResult);
            Assert.Equal(_event.Name, handleResult.Name);
            Assert.Equal(_event.Id, handleResult.Id);
            Assert.Equal(_event.Adress, handleResult.Adress);
            Assert.Equal(_event.Description, handleResult.Description);
            Assert.Equal(_event.MaxMemberCount - _event.MemberCount, handleResult.FreePlaces);
            Assert.Equal(_event.Date, handleResult.Date);
            Assert.Equal(_event.Category.Id, handleResult.Category.Id);
            Assert.Equal(_event.Category.Name, handleResult.Category.Name);
            Assert.Equal(_event.Date, handleResult.Date);
            Assert.True(handleResult.IsSubscribed);

        }

        [Fact]
        public async Task GetEventHandler_ThrowEventNotFoundException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = false;
            var userId = "Id";
            var token = CancellationToken.None;
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges, token));

            var getEventHandler = new GetEventHandler(_repository.Object, _mapper);
            var getEventQuery = new GetEventQuery(userId, eventId, trackChanges);

            //Act



            //Assert

            await Assert.ThrowsAnyAsync<EventNotFoundException>(async () => await getEventHandler.Handle(getEventQuery, token));

        }

        [Fact]
        public async Task GetEventsHandler_ReturnEventForReviewDtosAndMetaData()
        {
            //Arrange

            var eventParameters = new Mock<EventParameters>();
            var trackChanges = false;
            var count = 1;
            var userId = "id";
            var token = CancellationToken.None;

            var eventWithMetaData = new PagedList<Event>(new List<Event> { _event }, count, 1, 1);
            _repository.Setup(c => c.Event.GetEventsAsync(eventParameters.Object, trackChanges, token)).ReturnsAsync(eventWithMetaData);
            var eventsDto = new List<EventForReviewDto>();

            for (int i = 0; i < eventWithMetaData.Count; i++)
            {
                var eventDto = _mapper.Map<EventForReviewDto>(eventWithMetaData[i]);
                eventsDto.Add(eventDto);
            }

            var getEventsHandler = new GetEventsHandler(_repository.Object, _mapper);
            var getEventsQuery = new GetEventsQuery(userId, eventParameters.Object, trackChanges);

            //Act

            var handleResult = await getEventsHandler.Handle(getEventsQuery, token);

            //Assert

            Assert.IsType<List<EventForReviewDto>>(handleResult.events);
            Assert.IsType<MetaData>(handleResult.metaData);
            Assert.Equal(_event.Id, handleResult.events[count - 1].Id);
            Assert.Equal(_event.Name, handleResult.events[count - 1].Name);
            Assert.Equal(_event.MaxMemberCount - _event.MemberCount, handleResult.events[count - 1].FreePlaces);
            Assert.True(handleResult.events[count - 1].IsSubscribed);
        }

        [Fact]
        public async Task UpdateEventHandler_ThrowEventNotFoundException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = false;
            var eventForUpdate = new Mock<EventForUpdateDto>();
            var token = CancellationToken.None;
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges, token));

            var updateEventHandler = new UpdateEventHandler(_repository.Object, _mapper);
            var updateEventCommand = new UpdateEventCommand(eventId, eventForUpdate.Object, trackChanges);

            //Act



            //Assert

            await Assert.ThrowsAnyAsync<EventNotFoundException>(async () =>
            await updateEventHandler.Handle(updateEventCommand, token));

        }

        [Fact]
        public async Task UpdateEventHandler_ThrowMaxMemberCountBadRequestException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = false;
            var eventForUpdate = new EventForUpdateDto { MaxMemberCount = 1 };
            var token = CancellationToken.None;
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges, token)).ReturnsAsync(_event);

            var updateEventHandler = new UpdateEventHandler(_repository.Object, _mapper);
            var updateEventCommand = new UpdateEventCommand(eventId, eventForUpdate, trackChanges);

            //Act



            //Assert

            await Assert.ThrowsAnyAsync<MaxMemberCountBadRequestException>(async () =>
            await updateEventHandler.Handle(updateEventCommand, token));

        }

        [Fact]
        public async Task UpdateEventHandler_TestMapping()
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
            var token = CancellationToken.None;

            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges, token)).ReturnsAsync(_event);

            var updateEventHandler = new UpdateEventHandler(_repository.Object, _mapper);
            var updateEventCommand = new UpdateEventCommand(eventId, eventForUpdate, trackChanges);

            //Act

            await updateEventHandler.Handle(updateEventCommand, token);

            //Assert

            Assert.Equal(_event.Name, eventForUpdate.Name);
            Assert.Equal(_event.Adress, eventForUpdate.Adress);
            Assert.Equal(_event.Description, eventForUpdate.Description);
            Assert.Equal(_event.MaxMemberCount, eventForUpdate.MaxMemberCount);
            Assert.Equal(_event.Date, eventForUpdate.Date);
            Assert.Equal(_event.CategoryId, eventForUpdate.CategoryId);
        }

        [Fact]
        public async Task GetUserEventsHandler_ReturnEventForReviewDtosAndMetaData()
        {
            //Arrange

            var eventParameters = new Mock<EventParameters>();
            var trackChanges = false;
            var count = 1;
            var userId = "id";
            var token = CancellationToken.None;

            var eventWithMetaData = new PagedList<Event>(new List<Event> { _event }, count, 1, 1);
            _repository.Setup(c => c.Member.GetUserEventsAsync(userId, eventParameters.Object, trackChanges, token)).ReturnsAsync(eventWithMetaData);
            var eventsDto = new List<EventForReviewDto>();

            for (int i = 0; i < eventWithMetaData.Count; i++)
            {
                var eventDto = _mapper.Map<EventForReviewDto>(eventWithMetaData[i]);
                eventsDto.Add(eventDto);
            }

            var getUserEventsHandler = new GetUserEventsHandler(_repository.Object, _mapper);
            var getUserEventsQuery = new GetUserEventsQuery(userId, eventParameters.Object, trackChanges);

            //Act

            var handleResult = await getUserEventsHandler.Handle(getUserEventsQuery, token);

            //Assert

            Assert.IsType<List<EventForReviewDto>>(handleResult.events);
            Assert.IsType<MetaData>(handleResult.metaData);
            Assert.Equal(_event.Id, handleResult.events[count - 1].Id);
            Assert.Equal(_event.Name, handleResult.events[count - 1].Name);
            Assert.Equal(_event.MaxMemberCount - _event.MemberCount, handleResult.events[count - 1].FreePlaces);
            Assert.True(handleResult.events[count - 1].IsSubscribed);
        }


    }
}
