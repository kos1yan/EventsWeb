
namespace Events.Domain.Exceptions
{
    public sealed class MemberNotFoundException : NotFoundException
    {
        public MemberNotFoundException(Guid memberId) : base($"The member with id: {memberId} doesn't exist in the database.") { }
        public MemberNotFoundException(string userId, Guid eventId) : base($"The user with id: {userId} doesn't exist in the event with id {eventId}.") { }
    }
}
