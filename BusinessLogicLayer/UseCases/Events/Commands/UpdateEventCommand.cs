using Events.Application.DataTransferObjects.Event;
using MediatR;

namespace Events.Application.UseCases.Events.Commands
{
    public sealed record UpdateEventCommand(Guid eventId, EventForUpdateDto eventForUpdate, bool trackChanges) : IRequest<EventDto>;
}
