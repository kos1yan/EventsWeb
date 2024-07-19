using AutoMapper;
using BusinessLogicLayer.MappingProfiles;
using BusinessLogicLayer.Services.Implementations;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Moq;
using Shared.DataTransferObjects.Member;
using Shared.Exceptions;

namespace Events.Tests.Services
{
    public class MemberServiceTests
    {
        private readonly Mock<IRepositoryManager> _repository;
        private readonly IMapper _mapper;
        private Member _member;
        private MemberService _memberService;

        public MemberServiceTests()
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

            _memberService = new MemberService(_repository.Object, _mapper);
        }

        [Fact]
        public async Task MemberService_CreateMemberAsync_ReturnMemberDto()
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
                Members = new List<Member> { new Member { UserId = "ws" } } ,
                MaxMemberCount = 18,
                MemberCount = 7,
            };
            var userId = "id";
            var eventId = Guid.NewGuid();
            var trackChanges = false;
            var member = _mapper.Map<Member>(memberForCreation);

            _repository.Setup(c => c.Member.CreateMember(userId, eventId, member));
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges)).ReturnsAsync(eventById);
            _repository.Setup(c => c.SaveAsync());

            var memberToReturn = _mapper.Map<MemberDto>(member);

            //Act

            var serviceResult = await _memberService.CreateMemberAsync(userId, eventId, memberForCreation, trackChanges);

            //Assert

            Assert.NotNull(serviceResult);
            Assert.IsType<MemberDto>(serviceResult);
            Assert.Equal(memberForCreation.Name, serviceResult.Name);
            Assert.Equal(memberForCreation.Surname, serviceResult.Surname);
            Assert.Equal(memberForCreation.DateOfBirth, serviceResult.DateOfBirth);
            Assert.Equal(memberForCreation.Email, serviceResult.Email);
            Assert.Equal(8, eventById.MemberCount);
        }

        [Fact]
        public async Task MemberService_CreateMemberAsync_ThrowEventNotFoundException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var userId = "id";
            var trackChanges = false;
            var memberForCreation = new Mock<MemberForCreationDto>();
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges));


            //Act



            //Assert

            await Assert.ThrowsAnyAsync<EventNotFoundException>(async () => 
            await _memberService.CreateMemberAsync(userId, eventId, memberForCreation.Object, trackChanges));

        }

        [Fact]
        public async Task MemberService_CreateMemberAsync_ThrowFreePlacesBadRequestException()
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

            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges)).ReturnsAsync(eventById);
            var member = _mapper.Map<Member>(memberForCreation);
            _repository.Setup(c => c.Member.CreateMember(userId, eventId, member));


            //Act



            //Assert

            await Assert.ThrowsAnyAsync<FreePlacesBadRequestException>(async () =>
            await _memberService.CreateMemberAsync(userId, eventId, memberForCreation, trackChanges));

        }

        [Fact]
        public async Task MemberService_CreateMemberAsync_ThrowCreateMemberBadRequestException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var userId = "id";
            var trackChanges = false;
            var memberForCreation = new Mock<MemberForCreationDto>();
            var eventById = new Event { Members = new List<Member> { new Member { UserId = userId } } };

            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges)).ReturnsAsync(eventById);

            //Act



            //Assert

            await Assert.ThrowsAnyAsync<CreateMemberBadRequestException>(async () =>
            await _memberService.CreateMemberAsync(userId, eventId, memberForCreation.Object, trackChanges));

        }

        [Fact]
        public async Task MemberService_DeleteMemberAsync_ThrowMemberNotFoundException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var userId = "id";
            var trackChanges = true;

            _repository.Setup(c => c.Member.GetMemberAsync(userId, eventId, trackChanges));


            //Act



            //Assert

            await Assert.ThrowsAnyAsync<MemberNotFoundException>(async () =>
            await _memberService.DeleteMemberAsync(userId, eventId, trackChanges));

        }

        [Fact]
        public async Task MemberService_DeleteMemberAsync_CheckMemberCount()
        {
            //Arrange
            
            var eventId = Guid.NewGuid();
            var trackChanges = true;
            var userId = "id";
            var member = _member;
            _repository.Setup(c => c.Member.GetMemberAsync(userId, eventId, trackChanges)).ReturnsAsync(_member);
            _repository.Setup(c => c.Member.DeleteMember(_member));
            _repository.Setup(c => c.SaveAsync());
            

            //Act

            await _memberService.DeleteMemberAsync(userId, eventId, trackChanges);

            //Assert

            Assert.Equal(4, member.Event.MemberCount);

        }

        [Fact]
        public async Task MemberService_GetMemberAsync_ThrowMemberNotFoundException()
        {
            //Arrange

            var memberId = Guid.NewGuid();
            var trackChanges = false;

            _repository.Setup(c => c.Member.GetMemberAsync(memberId, trackChanges));


            //Act



            //Assert

            await Assert.ThrowsAnyAsync<MemberNotFoundException>(async () =>
            await _memberService.GetMemberAsync(memberId, trackChanges));

        }

        [Fact]
        public async Task MemberService_GetMemberAsync_ReturnMemberDto()
        {
            //Arrange

            var memberId = Guid.NewGuid();
            var trackChanges = false;

            _repository.Setup(c => c.Member.GetMemberAsync(memberId, trackChanges)).ReturnsAsync(_member);

            var member = _mapper.Map<MemberDto>(_member);

            //Act

            var serviceResult = await _memberService.GetMemberAsync(memberId, trackChanges);

            //Assert

            Assert.NotNull(serviceResult);
            Assert.IsType<MemberDto>(serviceResult);
            Assert.Equal(member.Name, serviceResult.Name);
            Assert.Equal(member.Surname, serviceResult.Surname);
            Assert.Equal(member.DateOfBirth, serviceResult.DateOfBirth);
            Assert.Equal(member.Email, serviceResult.Email);
        }

        [Fact]
        public async Task MemberService_GetMembersAsync_ThrowEventNotFoundException()
        {
            //Arrange

            var eventId = Guid.NewGuid();
            var trackChanges = false;
            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges));


            //Act



            //Assert

            await Assert.ThrowsAnyAsync<EventNotFoundException>(async () =>
            await _memberService.GetMembersAsync(eventId, trackChanges));

        }

        [Fact]
        public async Task MemberService_GetMembersAsync_ReturnMemberDtos()
        {
            //Arrange

            var trackChanges = false;
            var eventId = Guid.NewGuid();
            var eventById = new Event();
            var members = new List<Member> { _member };

            _repository.Setup(c => c.Event.GetEventAsync(eventId, trackChanges)).ReturnsAsync(eventById);
            _repository.Setup(c => c.Member.GetMembersAsync(eventId, trackChanges)).ReturnsAsync(members);

            var member = _mapper.Map<List<MemberDto>>(members);

            //Act

            var serviceResult = await _memberService.GetMembersAsync(eventId, trackChanges);

            //Assert

            Assert.NotNull(serviceResult);
            Assert.IsType<List<MemberDto>>(serviceResult);
            Assert.Equal(_member.Name, serviceResult[0].Name);
            Assert.Equal(_member.Surname, serviceResult[0].Surname);
            Assert.Equal(_member.DateOfBirth, serviceResult[0].DateOfBirth);
            Assert.Equal(_member.Email, serviceResult[0].Email);
        }
    }
}
