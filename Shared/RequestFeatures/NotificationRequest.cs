
namespace Shared.RequestFeatures
{
    public class NotificationRequest
    {
        public string? Title { get; init; }
        public string? Body { get; init; }
        public Guid EventId { get; init; }
    }
}
