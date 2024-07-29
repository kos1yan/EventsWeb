using AutoMapper;
using Events.Application.DataTransferObjects.Event;
using Events.Application.DataTransferObjects.Image;
using Events.Application.Interfaces;
using Events.Application.UseCases.Events.Commands;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.UseCases.Events.Handlers
{
    public sealed class CreateEventHandler : IRequestHandler<CreateEventCommand, EventDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateEventHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken token)
        {
            var newEvent = _mapper.Map<Event>(request.eventForCreation);

            if (request.eventForCreation.Images != null)
            {
                foreach (var image in request.eventForCreation.Images)
                {
                    var result = await _repository.Cloudinary.AddPhotoAsync(image, token);
                    var imageUrl = new Image() { Url = result.Url.ToString() };
                    newEvent.Images.Add(imageUrl);
                }
            }

            _repository.Event.CreateEvent(newEvent);
            await _repository.SaveAsync();

            var eventToReturn = _mapper.Map<EventDto>(newEvent);
            eventToReturn.FreePlaces = newEvent.MaxMemberCount;
            eventToReturn.Images = _mapper.Map<List<ImageDto>>(newEvent.Images);

            return eventToReturn;
        }
    }
}
