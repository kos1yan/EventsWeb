using DataAccessLayer.DbContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Implementations;
using Shared.RequestFeatures;

namespace Events.Tests.Repositories
{
    public class EventRepositoryTests
    {
        private readonly EventContext _eventContext;
        private EventRepository _eventRepository;

        public EventRepositoryTests()
        {
            _eventContext = DbContextFactory.GetDatabaseContext("EventRepositoryTestsDb");
            _eventRepository = new EventRepository(_eventContext);
        }

        [Fact]
        public async Task EventRepository_GetEventAsync_ReturnEvent()
        {
            //Arrange

            var eventId = new Guid("e0e44b16-757f-48d9-ac03-61fa3662559c");
            var trackChanges = false;

            //Act

            var result = await _eventRepository.GetEventAsync(eventId, trackChanges);

            //Assert

            Assert.IsType<Event>(result);
            Assert.Equal(eventId, result.Id);
        }


        [Fact]
        public async Task EventRepository_GetEventsAsync_ReturnPagedList()
        {
            //Arrange

            var eventParameters = new EventParameters
            {
                PageSize = 1,
            };
            var trackChanges = false;

            //Act

            var result = await _eventRepository.GetEventsAsync(eventParameters, trackChanges);

            //Assert

            Assert.IsType<PagedList<Event>>(result);
            Assert.Single(result);
        }
        [Fact]
        public async Task EventRepository_GetEventsAsync_ReturnFilteredByDate()
        {
            //Arrange

            var eventParameters = new EventParameters
            {
                Date = "Date2"
            };
            var trackChanges = false;

            //Act

            var result = await _eventRepository.GetEventsAsync(eventParameters, trackChanges);

            //Assert

            Assert.IsType<PagedList<Event>>(result);
            Assert.Single(result);
            Assert.Equal(new Guid("ec3a6eb9-726e-4e69-aa5c-78f2d1b75bb2"), result[0].Id);
        }

        [Fact]
        public async Task EventRepository_GetEventsAsync_ReturnFilteredByCategory()
        {
            //Arrange

            var eventParameters = new EventParameters
            {
                Category = 1
            };
            var trackChanges = false;

            //Act

            var result = await _eventRepository.GetEventsAsync(eventParameters, trackChanges);

            //Assert

            Assert.IsType<PagedList<Event>>(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public async Task EventRepository_GetEventsAsync_ReturnFilteredByAdress()
        {
            //Arrange

            var eventParameters = new EventParameters
            {
                Adress = "Adress2"
            };
            var trackChanges = false;

            //Act

            var result = await _eventRepository.GetEventsAsync(eventParameters, trackChanges);

            //Assert

            Assert.IsType<PagedList<Event>>(result);
            Assert.Single(result);
            Assert.Equal(new Guid("ec3a6eb9-726e-4e69-aa5c-78f2d1b75bb2"), result[0].Id);
        }

        [Fact]
        public async Task EventRepository_GetEventsAsync_ReturnSearchedByName()
        {
            //Arrange

            var eventParameters = new EventParameters
            {
                SearchByName = "Na"
            };
            var trackChanges = false;

            //Act

            var result = await _eventRepository.GetEventsAsync(eventParameters, trackChanges);

            //Assert

            Assert.IsType<PagedList<Event>>(result);
            Assert.Equal(4, result.Count);
        }
    }
}
