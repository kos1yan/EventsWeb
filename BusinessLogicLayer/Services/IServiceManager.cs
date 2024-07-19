
namespace BusinessLogicLayer.Services
{
    public interface IServiceManager
    {
        IEventService EventService { get; }
        IAuthenticationService AuthenticationService { get; }
        ITokenService TokenService { get; }
        IMemberService MemberService { get; }
        INotificationService NotificationService { get; }
    }
}
