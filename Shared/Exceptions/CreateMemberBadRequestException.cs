
namespace Shared.Exceptions
{
    public class CreateMemberBadRequestException : BadRequestException
    {
        public CreateMemberBadRequestException() : base("The user is already subscribed to the event!") { }
         
    }
}
