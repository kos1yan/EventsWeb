using Events.Domain.RequestFeatures;
using MediatR;

namespace Events.Application.UseCases.Notifications.Commands
{
    public sealed record SendMessageCommand(NotificationRequest request, bool trackChanges) : IRequest<Unit>;
}
