using Events.Application.DataTransferObjects.Event;
using MediatR;

namespace Events.Application.UseCases.Events.Commands
{
    public sealed record CreateEventCommand(EventForCreationDto eventForCreation) : IRequest<EventDto>;

}
