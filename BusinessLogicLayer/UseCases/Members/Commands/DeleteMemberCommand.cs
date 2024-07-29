using MediatR;

namespace Events.Application.UseCases.Members.Commands
{
    public sealed record DeleteMemberCommand(string userId, Guid eventId, bool trackChanges) : IRequest<Unit>;
}
