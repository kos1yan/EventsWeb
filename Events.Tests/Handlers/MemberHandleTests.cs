using AutoMapper;
using Events.Application.DataTransferObjects.Member;
using Events.Application.Interfaces;
using Events.Application.MappingProfiles;
using Events.Application.UseCases.Members.Commands;
using Events.Application.UseCases.Members.Handlers;
using Events.Application.UseCases.Members.Queries;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using Moq;

namespace Events.Tests.Handlers
{
    public class MemberHandleTests
    {
        private readonly Mock<IRepositoryManager> _repository;
        private readonly IMapper _mapper;
        private Member _member;

        public MemberHandleTests()
        {
            _repository = new Mock<IRepositoryManager>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MemberMappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
            _member = new Member
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Surname = "Surname",
                DateOfBirth = "DateOfBirth",
                Email = "Email",
                EventId = Guid.NewGuid(),
                Event = new Event { MemberCount = 5 }
            };
        }

        [Fact]
        public async Task CreateMemberHandler_ReturnMemberDto()
        {
            //Arrange

            var memberForCreation = new MemberForCreationDto
            {
                Name = "Name",
                Surname = "Surname",
                DateOfBirth = "DateOfBirth",
                Email = "Email"
            };
            var eventById = new Event
            {
                Members = new List<Member> { new Member { UserId = "ws" } },
                MaxMemberCount = 18,
                MemberCount = 7,
            };
            var userId = "id";
            var eventId = Guid.NewGuid();
            var trackChanges = false;
            var member = _mapper.Map<Member>(memberForCreation);
            var token = CancellationToken.None;

            _repository.Setup(c => c.Member.CreateMember(userId, eventId, member));
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges, token)).ReturnsAsync(eventById);
            _repository.Setup(c => c.SaveAsync());

            var memberToReturn = _mapper.Map<MemberDto>(member);

            var createMemberHandler = new CreateMemberHandler(_repository.Object, _mapper);
            var createMemberCommand = new CreateMemberCommand(userId, eventId, memberForCreation, trackChanges);

            //Act

            var handleResult = await createMemberHandler.Handle(createMemberCommand, token);

            //Assert

            Assert.NotNull(handleResult);
            Assert.IsType<MemberDto>(handleResult);
            Assert.Equal(memberForCreation.Name, handleResult.Name);
            Assert.Equal(memberForCreation.Surname, handleResult.Surname);
            Assert.Equal(memberForCreation.DateOfBirth, handleResult.DateOfBirth);
            Assert.Equal(memberForCreation.Email, handleResult.Email);
            Assert.Equal(8, eventById.MemberCount);
        }

        [Fact]
        public async Task CreateMemberHandler_ThrowEventNotFoundException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var userId = "id";
            var trackChanges = false;
            var token = CancellationToken.None;
            var memberForCreation = new Mock<MemberForCreationDto>();
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges, token));

            var createMemberHandler = new CreateMemberHandler(_repository.Object, _mapper);
            var createMemberCommand = new CreateMemberCommand(userId, eventId, memberForCreation.Object, trackChanges);


            //Act



            //Assert

            await Assert.ThrowsAnyAsync<EventNotFoundException>(async () =>
            await createMemberHandler.Handle(createMemberCommand, token));

        }

        [Fact]
        public async Task CreateMemberHandler_ThrowFreePlacesBadRequestException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var userId = "id";
            var trackChanges = false;
            var memberForCreation = new MemberForCreationDto
            {
                Name = "Name",
                Surname = "Surname",
                DateOfBirth = "DateOfBirth",
                Email = "Email"
            };
            var eventById = new Event
            {
                Members = new List<Member> { new Member { UserId = "ws" } },
                MaxMemberCount = 18,
                MemberCount = 18,
            };
            var token = CancellationToken.None;

            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges, token)).ReturnsAsync(eventById);
            var member = _mapper.Map<Member>(memberForCreation);
            _repository.Setup(c => c.Member.CreateMember(userId, eventId, member));

            var createMemberHandler = new CreateMemberHandler(_repository.Object, _mapper);
            var createMemberCommand = new CreateMemberCommand(userId, eventId, memberForCreation, trackChanges);

            //Act



            //Assert

            await Assert.ThrowsAnyAsync<FreePlacesBadRequestException>(async () =>
            await createMemberHandler.Handle(createMemberCommand, token));

        }

        [Fact]
        public async Task CreateMemberHandler_ThrowCreateMemberBadRequestException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var userId = "id";
            var trackChanges = false;
            var token = CancellationToken.None;
            var memberForCreation = new Mock<MemberForCreationDto>();
            var eventById = new Event { Members = new List<Member> { new Member { UserId = userId } } };

            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges, token)).ReturnsAsync(eventById);

            var createMemberHandler = new CreateMemberHandler(_repository.Object, _mapper);
            var createMemberCommand = new CreateMemberCommand(userId, eventId, memberForCreation.Object, trackChanges);

            //Act



            //Assert

            await Assert.ThrowsAnyAsync<CreateMemberBadRequestException>(async () =>
            await createMemberHandler.Handle(createMemberCommand, token));

        }

        [Fact]
        public async Task DeleteMemberHandler_ThrowMemberNotFoundException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var userId = "id";
            var trackChanges = true;
            var token = CancellationToken.None;

            _repository.Setup(c => c.Member.GetMemberAsync(userId, eventId, trackChanges, token));

            var deleteMemberHandler = new DeleteMemberHandler(_repository.Object, _mapper);
            var deleteMemberCommand = new DeleteMemberCommand(userId, eventId, trackChanges);

            //Act



            //Assert

            await Assert.ThrowsAnyAsync<MemberNotFoundException>(async () =>
            await deleteMemberHandler.Handle(deleteMemberCommand, token));

        }

        [Fact]
        public async Task DeleteMemberHandler_CheckMemberCount()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = true;
            var userId = "id";
            var member = _member;
            var token = CancellationToken.None;
            _repository.Setup(c => c.Member.GetMemberAsync(userId, eventId, trackChanges, token)).ReturnsAsync(_member);
            _repository.Setup(c => c.Member.DeleteMember(_member));
            _repository.Setup(c => c.SaveAsync());

            var deleteMemberHandler = new DeleteMemberHandler(_repository.Object, _mapper);
            var deleteMemberCommand = new DeleteMemberCommand(userId, eventId, trackChanges);

            //Act

            await deleteMemberHandler.Handle(deleteMemberCommand, token);

            //Assert

            Assert.Equal(4, member.Event.MemberCount);

        }

        [Fact]
        public async Task GetMemberHandler_ThrowMemberNotFoundException()
        {
            //Arrange

            var memberId = Guid.NewGuid();
            var trackChanges = false;
            var token = CancellationToken.None;

            _repository.Setup(c => c.Member.GetMemberAsync(memberId, trackChanges, token));

            var getMemberHandler = new GetMemberHandler(_repository.Object, _mapper);
            var getMemberQuery = new GetMemberQuery(memberId, trackChanges);

            //Act



            //Assert

            await Assert.ThrowsAnyAsync<MemberNotFoundException>(async () =>
            await getMemberHandler.Handle(getMemberQuery, token));

        }

        [Fact]
        public async Task GetMemberHandler_ReturnMemberDto()
        {
            //Arrange

            var memberId = Guid.NewGuid();
            var trackChanges = false;
            var token = CancellationToken.None;

            _repository.Setup(c => c.Member.GetMemberAsync(memberId, trackChanges, token)).ReturnsAsync(_member);

            var member = _mapper.Map<MemberDto>(_member);

            var getMemberHandler = new GetMemberHandler(_repository.Object, _mapper);
            var getMemberQuery = new GetMemberQuery(memberId, trackChanges);

            //Act

            var handleResult = await getMemberHandler.Handle(getMemberQuery, token);

            //Assert

            Assert.NotNull(handleResult);
            Assert.IsType<MemberDto>(handleResult);
            Assert.Equal(member.Name, handleResult.Name);
            Assert.Equal(member.Surname, handleResult.Surname);
            Assert.Equal(member.DateOfBirth, handleResult.DateOfBirth);
            Assert.Equal(member.Email, handleResult.Email);
        }

        [Fact]
        public async Task GetMembersHandler_ThrowEventNotFoundException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = false;
            var token = CancellationToken.None;
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges, token));

            var getMembersHandler = new GetMembersHandler(_repository.Object, _mapper);
            var getMembersQuery = new GetMembersQuery(eventId, trackChanges);

            //Act



            //Assert

            await Assert.ThrowsAnyAsync<EventNotFoundException>(async () =>
            await getMembersHandler.Handle(getMembersQuery, token));

        }

        [Fact]
        public async Task GetMembersHandler_ReturnMemberDtos()
        {
            //Arrange

            var trackChanges = false;
            var eventId = Guid.NewGuid();
            var eventById = new Event();
            var members = new List<Member> { _member };
            var token = CancellationToken.None;

            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges, token)).ReturnsAsync(eventById);
            _repository.Setup(c => c.Member.GetMembersAsync(eventId, trackChanges, token)).ReturnsAsync(members);

            var member = _mapper.Map<List<MemberDto>>(members);

            var getMembersHandler = new GetMembersHandler(_repository.Object, _mapper);
            var getMembersQuery = new GetMembersQuery(eventId, trackChanges);

            //Act

            var handleResult = await getMembersHandler.Handle(getMembersQuery, token);

            //Assert

            Assert.NotNull(handleResult);
            Assert.IsType<List<MemberDto>>(handleResult);
            Assert.Equal(_member.Name, handleResult[0].Name);
            Assert.Equal(_member.Surname, handleResult[0].Surname);
            Assert.Equal(_member.DateOfBirth, handleResult[0].DateOfBirth);
            Assert.Equal(_member.Email, handleResult[0].Email);
        }
    }
}
