using Events.Domain.RequestFeatures;

namespace Events.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendMessageAsync(NotificationRequest request, bool trackChanges, CancellationToken token);
    }
}
