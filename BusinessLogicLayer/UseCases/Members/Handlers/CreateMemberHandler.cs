using AutoMapper;
using Events.Application.DataTransferObjects.Member;
using Events.Application.Interfaces;
using Events.Application.UseCases.Members.Commands;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using MediatR;

namespace Events.Application.UseCases.Members.Handlers
{
    public sealed class CreateMemberHandler : IRequestHandler<CreateMemberCommand, MemberDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateMemberHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MemberDto> Handle(CreateMemberCommand request, CancellationToken token)
        {
            var eventById = await _repository.Event.GetEventAsync(request.eventId, request.trackChanges, token);
            if (eventById is null) throw new EventNotFoundException(request.eventId);

            if (eventById.Members.Where(c => c.UserId.Equals(request.userId)).Count() != 0) throw new CreateMemberBadRequestException();

            var member = _mapper.Map<Member>(request.memberForCreation);
            _repository.Member.CreateMember(request.userId, request.eventId, member);

            if (eventById.MaxMemberCount == eventById.MemberCount) throw new FreePlacesBadRequestException();
            else eventById.MemberCount++;

            await _repository.SaveAsync();

            var memberToReturn = _mapper.Map<MemberDto>(member);
            return memberToReturn;
        }
    }
}
