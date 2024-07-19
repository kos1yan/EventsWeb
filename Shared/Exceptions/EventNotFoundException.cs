
namespace Shared.Exceptions
{
    public sealed class EventNotFoundException : NotFoundException
    {
        public EventNotFoundException(Guid eventId) : base($"The event with id: {eventId} doesn't exist in the database.") { }
    }
}
