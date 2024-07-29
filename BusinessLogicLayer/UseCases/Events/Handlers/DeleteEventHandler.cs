using AutoMapper;
using Events.Application.Interfaces;
using Events.Application.UseCases.Events.Commands;
using Events.Domain.Exceptions;
using MediatR;

namespace Events.Application.UseCases.Events.Handlers
{
    public sealed class DeleteEventHandler : IRequestHandler<DeleteEventCommand, Unit>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public DeleteEventHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken token)
        {
            var eventForDelete = await _repository.Event.GetEventAsync(request.eventId, request.trackChanges, token);
            if (eventForDelete is null) throw new EventNotFoundException(request.eventId);

            if (eventForDelete.Images.Count != 0)
            {
                foreach (var image in eventForDelete.Images)
                {
                    await _repository.Cloudinary.DeletePhotoAsync(image.Url);
                }
            }
            _repository.Event.DeleteEvent(eventForDelete);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
