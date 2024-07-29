using AutoMapper;
using Events.Application.DataTransferObjects.Member;
using Events.Application.Interfaces;
using Events.Application.UseCases.Members.Queries;
using Events.Domain.Exceptions;
using MediatR;

namespace Events.Application.UseCases.Members.Handlers
{
    public sealed class GetMembersHandler : IRequestHandler<GetMembersQuery, List<MemberDto>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetMembersHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MemberDto>> Handle(GetMembersQuery request, CancellationToken token)
        {
            var eventById = await _repository.Event.GetEventAsync(request.eventId, request.trackChanges, token);
            if (eventById is null) throw new EventNotFoundException(request.eventId);

            var members = await _repository.Member.GetMembersAsync(request.eventId, request.trackChanges, token);
            var memberDtos = _mapper.Map<List<MemberDto>>(members);

            return memberDtos;
        }
    }
}
