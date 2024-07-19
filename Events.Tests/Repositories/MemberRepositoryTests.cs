using DataAccessLayer.DbContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Implementations;
using Shared.RequestFeatures;

namespace Events.Tests.Repositories
{
    public class MemberRepositoryTests
    {
        private readonly EventContext _eventContext;
        private MemberRepository _memberRepository;

        public MemberRepositoryTests()
        {
            _eventContext = DbContextFactory.GetDatabaseContext("MemberRepositoryTestsDb");
            _memberRepository = new MemberRepository(_eventContext);
        }

        [Fact]
        public async Task MemberRepository_GetMemberAsync_ReturnMemberByMemberId()
        {
            //Arrange

            var memberId = new Guid("d8541560-f9ff-4484-b6cd-544498669cc7");
            var trackChanges = false;

            //Act

            var result = await _memberRepository.GetMemberAsync(memberId, trackChanges);

            //Assert

            Assert.IsType<Member>(result);
            Assert.Equal(memberId, result.Id);
        }

        [Fact]
        public async Task MemberRepository_GetMemberAsync_ReturnMember()
        {
            //Arrange

            var eventId = new Guid("e0e44b16-757f-48d9-ac03-61fa3662559c");
            var userId = "dd1513b5-6f40-49f4-b90d-18f6efd50a76";
            var trackChanges = false;

            //Act

            var result = await _memberRepository.GetMemberAsync(userId, eventId, trackChanges);

            //Assert

            Assert.IsType<Member>(result);
            Assert.Equal(new Guid("d8541560-f9ff-4484-b6cd-544498669cc7"), result.Id);
        }


        [Fact]
        public async Task MemberRepository_GetMembersAsync_ReturnMembers()
        {
            //Arrange

            var eventId = new Guid("e0e44b16-757f-48d9-ac03-61fa3662559c");
            var trackChanges = false;

            //Act

            var result = await _memberRepository.GetMembersAsync(eventId, trackChanges);

            //Assert

            Assert.IsType<List<Member>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task MemberRepository_GetUserEventsAsync_ReturnFilteredByDate()
        {
            //Arrange

            var userId = "dd1513b5-6f40-49f4-b90d-18f6efd50a76";
            var eventParameters = new EventParameters
            {
                Date = "Date1"
            };
            var trackChanges = false;

            //Act

            var result = await _memberRepository.GetUserEventsAsync(userId, eventParameters, trackChanges);

            //Assert

            Assert.IsType<PagedList<Event>>(result);
            Assert.Single(result);
            Assert.Equal(new Guid("e0e44b16-757f-48d9-ac03-61fa3662559c"), result[0].Id);
        }

        [Fact]
        public async Task MemberRepository_GetUserEventsAsync_ReturnFilteredByCategory()
        {
            //Arrange

            var userId = "dd1513b5-6f40-49f4-b90d-18f6efd50a76";
            var eventParameters = new EventParameters
            {
                Category = 1
            };
            var trackChanges = false;

            //Act

            var result = await _memberRepository.GetUserEventsAsync(userId, eventParameters, trackChanges);

            //Assert

            Assert.IsType<PagedList<Event>>(result);
            Assert.Single(result);
            Assert.Equal(new Guid("e0e44b16-757f-48d9-ac03-61fa3662559c"), result[0].Id);
        }

        [Fact]
        public async Task MemberRepository_GetUserEventsAsync_ReturnFilteredByAdress()
        {
            //Arrange

            var userId = "dd1513b5-6f40-49f4-b90d-18f6efd50a76";
            var eventParameters = new EventParameters
            {
                Adress = "Adress1"
            };
            var trackChanges = false;

            //Act

            var result = await _memberRepository.GetUserEventsAsync(userId, eventParameters, trackChanges);

            //Assert

            Assert.IsType<PagedList<Event>>(result);
            Assert.Single(result);
            Assert.Equal(new Guid("e0e44b16-757f-48d9-ac03-61fa3662559c"), result[0].Id);
        }

        [Fact]
        public async Task MemberRepository_GetUserEventsAsync_ReturnSearchedByName()
        {
            //Arrange

            var userId = "dd1513b5-6f40-49f4-b90d-18f6efd50a76";
            var eventParameters = new EventParameters
            {
                SearchByName = "Na"
            };
            var trackChanges = false;

            //Act

            var result = await _memberRepository.GetUserEventsAsync(userId, eventParameters, trackChanges);

            //Assert

            Assert.IsType<PagedList<Event>>(result);
            Assert.Single(result);
        }
    }
}
