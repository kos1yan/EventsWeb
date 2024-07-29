
namespace Events.Domain.Exceptions
{
    public class ClaimsBadRequestException : UnauthorizedAccessException
    {
        public ClaimsBadRequestException() : base("User don't have claims!") { }
        
    }
}
