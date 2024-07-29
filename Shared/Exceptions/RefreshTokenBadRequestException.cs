
namespace Events.Domain.Exceptions
{
    public class RefreshTokenBadRequestException : BadRequestException
    {
        public RefreshTokenBadRequestException() : base("Invalid client request. The refresh token has some invalid values.") { }
        
    }
}
