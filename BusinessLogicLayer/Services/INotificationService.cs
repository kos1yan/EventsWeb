using Shared.RequestFeatures;

namespace BusinessLogicLayer.Services
{
    public interface INotificationService
    {
        Task SendMessageAsync(NotificationRequest request, bool trackChanges);
    }
}
