using AutoMapper;
using Events.Application.DataTransferObjects.Event;
using Events.Application.DataTransferObjects.Image;
using Events.Application.Interfaces;
using Events.Application.UseCases.Events.Queries;
using Events.Domain.RequestFeatures;
using MediatR;

namespace Events.Application.UseCases.Events.Handlers
{
    public sealed class GetEventsHandler : IRequestHandler<GetEventsQuery, (List<EventForReviewDto> events, MetaData metaData)>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetEventsHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(List<EventForReviewDto> events, MetaData metaData)> Handle(GetEventsQuery request, CancellationToken token)
        {
            var eventWithMetaData = await _repository.Event.GetEventsAsync(request.eventParameters, request.trackChanges, token);
            var eventsDto = new List<EventForReviewDto>();

            for (int i = 0; i < eventWithMetaData.Count; i++)
            {
                var eventDto = _mapper.Map<EventForReviewDto>(eventWithMetaData[i]);

                eventDto.FreePlaces = eventWithMetaData[i].MaxMemberCount - eventWithMetaData[i].MemberCount;
                eventDto.Images = _mapper.Map<List<ImageDto>>(eventWithMetaData[i].Images);
                eventDto.IsSubscribed = !eventWithMetaData[i].Members.TrueForAll((member) => !member.UserId.Equals(request.userId));

                eventsDto.Add(eventDto);
            }

            return (events: eventsDto, metaData: eventWithMetaData.MetaData);
        }
    }
}
