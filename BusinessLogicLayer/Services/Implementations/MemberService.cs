using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Shared.DataTransferObjects.Member;
using Shared.Exceptions;

namespace BusinessLogicLayer.Services.Implementations
{
    public class MemberService : IMemberService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public MemberService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<MemberDto> CreateMemberAsync(string userId, Guid eventId, MemberForCreationDto memberForCreation, bool trackChanges)
        {
            var eventById = await _repository.Event.GetEventAsync(eventId, trackChanges);
            if (eventById is null) throw new EventNotFoundException(eventId);

            if (eventById.Members.Where(c => c.UserId.Equals(userId)).Count() != 0) throw new CreateMemberBadRequestException();

            var member = _mapper.Map<Member>(memberForCreation);
            _repository.Member.CreateMember(userId, eventId, member);

            if (eventById.MaxMemberCount == eventById.MemberCount) throw new FreePlacesBadRequestException();
            else eventById.MemberCount++;

            await _repository.SaveAsync();

            var memberToReturn = _mapper.Map<MemberDto>(member);
            return memberToReturn;
        }

        public async Task DeleteMemberAsync(string userId, Guid eventId, bool trackChanges)
        {
            var member = await _repository.Member.GetMemberAsync(userId, eventId, trackChanges);
            if(member is null) throw new MemberNotFoundException(userId, eventId);

            _repository.Member.DeleteMember(member);
            member.Event.MemberCount--;

            await _repository.SaveAsync();
        }

        public async Task<MemberDto> GetMemberAsync(Guid memberId, bool trackChanges)
        {
            var memberById = await _repository.Member.GetMemberAsync(memberId, trackChanges);
            if (memberById is null) throw new MemberNotFoundException(memberId);

            var member = _mapper.Map<MemberDto>(memberById);
            return member;
        }

        public async Task<List<MemberDto>> GetMembersAsync(Guid eventId, bool trackChanges)
        {
            var eventById = await _repository.Event.GetEventAsync(eventId, trackChanges);
            if (eventById is null) throw new EventNotFoundException(eventId);

            var members = await _repository.Member.GetMembersAsync(eventId, trackChanges);
            var memberDtos = _mapper.Map<List<MemberDto>>(members);

            return memberDtos;
        }
    }
}
