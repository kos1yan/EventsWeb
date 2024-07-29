using Events.Application.DataTransferObjects.Event;
using MediatR;

namespace Events.Application.UseCases.Events.Queries
{
    public sealed record GetEventQuery(string userId, Guid eventId, bool trackChanges) : IRequest<EventDto>;

}
