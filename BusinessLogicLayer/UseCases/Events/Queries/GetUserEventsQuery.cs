using Events.Application.DataTransferObjects.Event;
using Events.Domain.RequestFeatures;
using MediatR;

namespace Events.Application.UseCases.Events.Queries
{
    public sealed record GetUserEventsQuery(string userId, EventParameters eventParameters, bool trackChanges) : IRequest<(List<EventForReviewDto> events, MetaData metaData)>;
}
