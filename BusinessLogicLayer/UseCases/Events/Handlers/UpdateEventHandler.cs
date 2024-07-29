using AutoMapper;
using Events.Application.DataTransferObjects.Event;
using Events.Application.Interfaces;
using Events.Application.UseCases.Events.Commands;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using MediatR;

namespace Events.Application.UseCases.Events.Handlers
{
    public sealed class UpdateEventHandler : IRequestHandler<UpdateEventCommand, EventDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateEventHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EventDto> Handle(UpdateEventCommand request, CancellationToken token)
        {
            var eventById = await _repository.Event.GetEventAsync(request.eventId, request.trackChanges, token);
            if (eventById is null) throw new EventNotFoundException(request.eventId);

            if (request.eventForUpdate.MaxMemberCount < eventById.MemberCount) throw new MaxMemberCountBadRequestException(eventById.MemberCount);

            if (request.eventForUpdate.DeletedImages != null)
            {
                foreach (var image in request.eventForUpdate.DeletedImages)
                {
                    await _repository.Cloudinary.DeletePhotoAsync(image);
                    var imageForDelete = eventById.Images.Where(s => s.Url.Equals(image)).SingleOrDefault();
                    eventById.Images.Remove(imageForDelete);
                }
            }

            if (request.eventForUpdate.NewImages != null)
            {
                foreach (var image in request.eventForUpdate.NewImages)
                {
                    var result = await _repository.Cloudinary.AddPhotoAsync(image, token);
                    var imageUrl = new Image() { Url = result.Url.ToString() };
                    eventById.Images.Add(imageUrl);
                }
            }

            _mapper.Map(request.eventForUpdate, eventById);
            var eventToReturn = _mapper.Map<EventDto>(eventById);

            await _repository.SaveAsync();

            return eventToReturn;
        }
    }
}
