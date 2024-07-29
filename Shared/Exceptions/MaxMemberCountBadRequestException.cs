
namespace Events.Domain.Exceptions
{
    public sealed class MaxMemberCountBadRequestException : BadRequestException
    {
        public MaxMemberCountBadRequestException(int MemberCount)
            : base($"Incorrect maximum number of member. Number of registered members {MemberCount}") { }
    }
}
