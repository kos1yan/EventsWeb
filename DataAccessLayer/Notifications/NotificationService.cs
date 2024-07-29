using AutoMapper;
using Events.Application.Interfaces;
using Events.Domain.Exceptions;
using Events.Domain.RequestFeatures;
using FirebaseAdmin.Messaging;

namespace Events.Infrastructure.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public NotificationService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task SendMessageAsync(NotificationRequest request, bool trackChanges, CancellationToken token)
        {
            var eventById = await _repository.Event.GetEventAsync(request.EventId, trackChanges, token);
            if (eventById is null) throw new EventNotFoundException(request.EventId);

            var tokens = new List<string>();

            foreach(var member in eventById.Members)
            {
                var deviceToken = member.User.DeviceToken;
                if(deviceToken != null) tokens.Add(deviceToken);
            }

            var message = new MulticastMessage()
            {
                Tokens = tokens,
                Notification = new Notification
                {
                    Title = request.Title,
                    Body = request.Body,
                }
            };

            await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
        }
    }
}
