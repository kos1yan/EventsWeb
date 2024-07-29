using Events.Application.Interfaces;
using Events.Application.UseCases.Notifications.Commands;
using MediatR;

namespace Events.Application.UseCases.Notifications.Handlers
{
    internal sealed class SendMessageHandler : IRequestHandler<SendMessageCommand, Unit>
    {
        private readonly INotificationService _notificationService;

        public SendMessageHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<Unit> Handle(SendMessageCommand request, CancellationToken token)
        {
            await _notificationService.SendMessageAsync(request.request, request.trackChanges, token);

            return Unit.Value;
        }
    }
}
