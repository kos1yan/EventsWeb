using AutoMapper;
using Events.Application.DataTransferObjects.Member;
using Events.Application.Interfaces;
using Events.Application.UseCases.Members.Queries;
using Events.Domain.Exceptions;
using MediatR;

namespace Events.Application.UseCases.Members.Handlers
{
    public sealed class GetMemberHandler : IRequestHandler<GetMemberQuery, MemberDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetMemberHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MemberDto> Handle(GetMemberQuery request, CancellationToken token)
        {
            var memberById = await _repository.Member.GetMemberAsync(request.memberId, request.trackChanges, token);
            if (memberById is null) throw new MemberNotFoundException(request.memberId);

            var member = _mapper.Map<MemberDto>(memberById);
            return member;
        }
    }
}
