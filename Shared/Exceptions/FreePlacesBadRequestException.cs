
namespace Shared.Exceptions
{
    public class FreePlacesBadRequestException : BadRequestException
    {
        public FreePlacesBadRequestException() : base("It is impossible to subscribe to the event. No free places") { }
        
    }
}
