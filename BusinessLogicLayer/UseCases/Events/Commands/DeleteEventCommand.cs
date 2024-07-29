using MediatR;

namespace Events.Application.UseCases.Events.Commands
{
    public sealed record DeleteEventCommand(Guid eventId, bool trackChanges) : IRequest<Unit>;
}
