using AutoMapper;
using DataAccessLayer.Entities.ConfigurationModels;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Options;

namespace BusinessLogicLayer.Services.Implementations
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IEventService> _eventService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<ITokenService> _tokenService;
        private readonly Lazy<IMemberService> _memberlService;
        private readonly Lazy<INotificationService> _notificationService;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, IOptions<JwtConfiguration> configuration)
        {
            _eventService = new Lazy<IEventService>(() => new EventService(repositoryManager, mapper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(repositoryManager, mapper));
            _tokenService = new Lazy<ITokenService>(() => new TokenService(repositoryManager, configuration));
            _memberlService = new Lazy<IMemberService>(() => new MemberService(repositoryManager, mapper));
            _notificationService = new Lazy<INotificationService>(() => new NotificationService(repositoryManager, mapper));
        }

        public IEventService EventService => _eventService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public ITokenService TokenService => _tokenService.Value;
        public IMemberService MemberService => _memberlService.Value;
        public INotificationService NotificationService => _notificationService.Value;


    }
}
