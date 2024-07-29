
namespace Events.Domain.Exceptions
{
    public class IdentityException : BadRequestException
    {
        public IdentityException(string message) : base(message) { }
    }
}
