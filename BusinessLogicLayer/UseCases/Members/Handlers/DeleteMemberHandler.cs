using AutoMapper;
using Events.Application.Interfaces;
using Events.Application.UseCases.Members.Commands;
using Events.Domain.Exceptions;
using MediatR;

namespace Events.Application.UseCases.Members.Handlers
{
    public sealed class DeleteMemberHandler : IRequestHandler<DeleteMemberCommand, Unit>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public DeleteMemberHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteMemberCommand request, CancellationToken token)
        {
            var member = await _repository.Member.GetMemberAsync(request.userId, request.eventId, request.trackChanges, token);
            if (member is null) throw new MemberNotFoundException(request.userId, request.eventId);

            _repository.Member.DeleteMember(member);
            member.Event.MemberCount--;

            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
