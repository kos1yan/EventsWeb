using AutoMapper;
using Events.Application.DataTransferObjects.Category;
using Events.Application.DataTransferObjects.Event;
using Events.Application.DataTransferObjects.Image;
using Events.Application.Interfaces;
using Events.Application.UseCases.Events.Queries;
using Events.Domain.Exceptions;
using MediatR;

namespace Events.Application.UseCases.Events.Handlers
{
    public sealed class GetEventHandler : IRequestHandler<GetEventQuery, EventDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetEventHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EventDto> Handle(GetEventQuery request, CancellationToken token)
        {
            var eventById = await _repository.Event.GetEventAsync(request.eventId, request.trackChanges, token);
            if (eventById is null) throw new EventNotFoundException(request.eventId);

            var eventToReturn = _mapper.Map<EventDto>(eventById);
            eventToReturn.FreePlaces = eventById.MaxMemberCount - eventById.MemberCount;
            eventToReturn.IsSubscribed = !eventById.Members.TrueForAll((member) => !member.UserId.Equals(request.userId));
            eventToReturn.Images = _mapper.Map<List<ImageDto>>(eventById.Images);
            eventToReturn.Category = _mapper.Map<CategoryDto>(eventById.Category);

            return eventToReturn;
        }
    }
}
